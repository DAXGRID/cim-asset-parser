using System;
using CIM.Asset.Parser.Xmi;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;

namespace CIM.Asset.Parser.Cim
{
    public class CimParser : ICimParser
    {
        private readonly IXmiExtractor _xmiExtractor;

        public CimParser(IXmiExtractor xmiExtractor)
        {
            _xmiExtractor = xmiExtractor;
        }

        public IEnumerable<CimEntity> Parse(string xmlFilePath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
                throw new ArgumentException($"{nameof(xmlFilePath)} null or empty is not valid");

            var xElement = _xmiExtractor.LoadXElement(xmlFilePath, encoding);
            var classes = _xmiExtractor.GetXElementClasses(xElement);
            var generalizations = _xmiExtractor.GetGeneralizations(xElement);

            var cimEntities = classes?
                .Select(x => new CimEntity
                {
                    Name = x.Attribute(EnterpriseArchitectConfig.Name).Value?.ToString(),
                    XmiId = x.Attribute(EnterpriseArchitectConfig.XmiId).Value?.ToString(),
                    Description = x.Descendants().OfType<XElement>()
                    .Where(y => y.Name.LocalName == EnterpriseArchitectConfig.TaggedValue)
                        .FirstOrDefault(y => y.Attribute(EnterpriseArchitectConfig.Tag)?.Value?.ToString() == EnterpriseArchitectConfig.Documentation)?.Attribute(EnterpriseArchitectConfig.Value)?.Value?.ToString(),
                    Attributes = x.Descendants().OfType<XElement>()
                        .Where(y => y.Name.LocalName == EnterpriseArchitectConfig.Attribute)
                    .Select(z => new Attribute { Name = z.Attribute(EnterpriseArchitectConfig.Name).Value?.ToString(), Description = z.Descendants().OfType<XElement>().Where(t => t.Name.LocalName == EnterpriseArchitectConfig.TaggedValue)?.FirstOrDefault(n => n.Attribute(EnterpriseArchitectConfig.Tag)?.Value?.ToString() == EnterpriseArchitectConfig.Description)?.Attribute(EnterpriseArchitectConfig.Value).Value?.ToString() }),

                    Namespace = x.Attribute(EnterpriseArchitectConfig.Namespace)?.Value?.ToString(),
                    SuperType = generalizations.FirstOrDefault(y => y.Attribute(EnterpriseArchitectConfig.Subtype)?.Value == x.Attribute(EnterpriseArchitectConfig.XmiId).Value?.ToString())?.Attribute(EnterpriseArchitectConfig.Supertype)?.Value.ToString()
                });

            return cimEntities;
        }
    }
}
