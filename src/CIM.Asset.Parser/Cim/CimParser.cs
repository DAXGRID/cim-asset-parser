using System;
using CIM.Asset.Parser.Xmi;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace CIM.Asset.Parser.Cim
{
    public class CimParser : ICimParser
    {
        private readonly IXmiExtractor _xmiExtractor;
        private readonly ILogger _logger;

        public CimParser(IXmiExtractor xmiExtractor, ILogger<CimParser> logger)
        {
            _xmiExtractor = xmiExtractor;
            _logger = logger;
        }

        public IEnumerable<CimEntity> Parse(string xmlFilePath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
                throw new ArgumentException($"{nameof(xmlFilePath)} null or empty is not valid");

            _logger.LogInformation($"Starting to parse XMI: '{xmlFilePath}'");

            var xElement = _xmiExtractor.LoadXElement(xmlFilePath, encoding);
            var classes = _xmiExtractor.GetXElementClasses(xElement);
            var generalizations = _xmiExtractor.GetGeneralizations(xElement);

            _logger.LogInformation($"Creating CIM entities");

            var cimEntities = CreateCimEntities(classes, generalizations);

            _logger.LogInformation($"Finished parsing XMI: '{xmlFilePath}' - returns {cimEntities.Count()} CIM entities");

            return cimEntities;
        }

        private IEnumerable<CimEntity> CreateCimEntities(IEnumerable<XElement> classes, IEnumerable<XElement> generalizations)
        {
            return classes?.AsParallel().Select(x => new CimEntity
                {
                    Name = x.Attribute(EnterpriseArchitectConfig.Name).Value?.ToString(),
                    XmiId = x.Attribute(EnterpriseArchitectConfig.XmiId).Value?.ToString(),
                    Description = GetDescription(x),
                    Attributes = GetAttributes(x),
                    Namespace = x.Attribute(EnterpriseArchitectConfig.Namespace)?.Value?.ToString(),
                    SuperType = GetSuperType(generalizations, x)
                });
        }


        private string GetDescription(XElement xElement)
        {
            var tags =  xElement.Descendants().OfType<XElement>().Where(y => y.Name.LocalName == EnterpriseArchitectConfig.TaggedValue);

            var description = tags
                .FirstOrDefault(y => y.Attribute(EnterpriseArchitectConfig.Tag)?.Value?.ToString() == EnterpriseArchitectConfig.Documentation)
                ?.Attribute(EnterpriseArchitectConfig.Value)?.Value?.ToString();

            return description;
        }

        private IEnumerable<Attribute> GetAttributes(XElement xElement)
        {
            var attributeElements = xElement.Descendants().OfType<XElement>().Where(y => y.Name.LocalName == EnterpriseArchitectConfig.Attribute);

            var attributes = attributeElements.AsParallel().Select(z => new Attribute
                {
                    Name = z.Attribute(EnterpriseArchitectConfig.Name).Value?.ToString(),
                    Description = z
                        .Descendants().OfType<XElement>()
                        .Where(t => t.Name.LocalName == EnterpriseArchitectConfig.TaggedValue)
                        ?.FirstOrDefault(n => n.Attribute(EnterpriseArchitectConfig.Tag)?.Value?.ToString() == EnterpriseArchitectConfig.Description)
                        ?.Attribute(EnterpriseArchitectConfig.Value).Value?.ToString() });

            return attributes;
        }

        private string GetSuperType(IEnumerable<XElement> generalizations, XElement entity)
        {
            var generalization = generalizations
                .FirstOrDefault(y => y.Attribute(EnterpriseArchitectConfig.Subtype)?.Value == entity.Attribute(EnterpriseArchitectConfig.XmiId).Value?.ToString())
                ?.Attribute(EnterpriseArchitectConfig.Supertype)?.Value.ToString();

            return generalization;
        }
    }
}
