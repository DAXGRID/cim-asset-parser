using System.Xml.Linq;
using System.Linq;
using System.Text;

namespace CIM.Asset.Parser.Xmi
{
    public class XmiParse : IXmiParse
    {
        private readonly IXmlTextReaderFactory _xmlTextReaderFactory;

        public XmiParse(IXmlTextReaderFactory xmlTextReaderFactory)
        {
            _xmlTextReaderFactory = xmlTextReaderFactory;
        }

        public void Parse(string xmlFilePath, Encoding encoding)
        {
            var xElement = XElement.Load(_xmlTextReaderFactory.Create(xmlFilePath, encoding));

            var classes = xElement.Descendants().OfType<XElement>()
                .Where(x => x.Name.LocalName == "Class");

            var generalizations = xElement.Descendants().OfType<XElement>()
                .Where(y => y.Name.LocalName == EnterpriseArchitectConfig.Generalization);

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
    }
}
