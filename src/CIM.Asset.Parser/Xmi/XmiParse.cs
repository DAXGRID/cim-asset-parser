using System;
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
                            .Where(y => y.Name.LocalName == "Generalization");

            var cimEntities = classes?
                .Select(x => new CimEntity
                {
                    Name = x.Attribute("name").Value?.ToString(),
                    XmiId = x.Attribute("xmi.id").Value?.ToString(),
                    Description = x.Descendants().OfType<XElement>()
                            .Where(y => y.Name.LocalName == "TaggedValue")
                            .FirstOrDefault(y => y.Attribute("tag")?.Value?.ToString() == "documentation")?.Attribute("value")?.Value?.ToString(),
                    Attributes = x.Descendants().OfType<XElement>()
                            .Where(y => y.Name.LocalName == "Attribute")
                        .Select(z => new Attribute { Name = z.Attribute("name").Value?.ToString(), Description = z.Descendants().OfType<XElement>().Where(t => t.Name.LocalName == "TaggedValue")?.FirstOrDefault(n => n.Attribute("tag")?.Value?.ToString() == "description")?.Attribute("value").Value?.ToString() }),
                    Namespace = x.Attribute("namespace")?.Value?.ToString(),
                    SuperType = generalizations.FirstOrDefault(y => y.Attribute("subtype")?.Value == x.Attribute("xmi.id").Value?.ToString())?.Attribute("supertype")?.Value.ToString()
                });
        }
    }
}
