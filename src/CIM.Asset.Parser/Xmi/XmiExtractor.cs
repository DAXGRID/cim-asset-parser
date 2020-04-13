using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CIM.Asset.Parser.Xmi
{
    public class XmiExtractor : IXmiExtractor
    {
        private readonly IXmlTextReaderFactory _xmlTextReaderFactory;

        public XmiExtractor(IXmlTextReaderFactory xmlTextReaderFactory)
        {
            _xmlTextReaderFactory = xmlTextReaderFactory;
        }

        public void Parse(string xmlFilePath, Encoding encoding)
        {
            var xElement = LoadXElement(xmlFilePath, encoding);
            var classes = GetXElementClasses(xElement);
            var generalizations = GetGeneralizations(xElement);

            var cimEntities = classes?
                .Select(x => new CimEntity
                {
                    Name = x.Attribute(EnterpriseArchitectConfig.Name).Value?.ToString(),
                    XmiId = x.Attribute(EnterpriseArchitectConfig.XmiId).Value?.ToString(),
                    Description = x.Descendants().OfType<XElement>()
                        .Where(y => y.Name.LocalName == EnterpriseArchitectConfig.Generalization)
                        .FirstOrDefault(y => y.Attribute(EnterpriseArchitectConfig.Tag)?.Value?.ToString() == EnterpriseArchitectConfig.Documentation)?.Attribute(EnterpriseArchitectConfig.Value)?.Value?.ToString(),
                    Attributes = x.Descendants().OfType<XElement>()
                        .Where(y => y.Name.LocalName == EnterpriseArchitectConfig.Attribute)
                    .Select(z => new Attribute { Name = z.Attribute(EnterpriseArchitectConfig.Name).Value?.ToString(), Description = z.Descendants().OfType<XElement>().Where(t => t.Name.LocalName == EnterpriseArchitectConfig.TaggedValue)?.FirstOrDefault(n => n.Attribute(EnterpriseArchitectConfig.Tag)?.Value?.ToString() == EnterpriseArchitectConfig.Description)?.Attribute(EnterpriseArchitectConfig.Value).Value?.ToString() }),

                    Namespace = x.Attribute(EnterpriseArchitectConfig.Namespace)?.Value?.ToString(),
                    SuperType = generalizations.FirstOrDefault(y => y.Attribute(EnterpriseArchitectConfig.Subtype)?.Value == x.Attribute(EnterpriseArchitectConfig.XmiId).Value?.ToString())?.Attribute(EnterpriseArchitectConfig.Supertype)?.Value.ToString()
                });
        }

        public XElement LoadXElement(string xmlFilePath, Encoding encoding)
        {
            return XElement.Load(_xmlTextReaderFactory.Create(xmlFilePath, encoding));
        }

        public IEnumerable<XElement> GetXElementClasses(XElement xElement)
        {
            return GetOnLocalName(xElement, EnterpriseArchitectConfig.Class);
        }

        public IEnumerable<XElement> GetGeneralizations(XElement xElement)
        {
            return GetOnLocalName(xElement, EnterpriseArchitectConfig.Generalization);
        }

        public IEnumerable<XElement> GetOnLocalName(XElement xElement, string localName)
        {
            return xElement.Descendants().OfType<XElement>()
                .Where(x => x.Name.LocalName == localName);
        }
    }
}
