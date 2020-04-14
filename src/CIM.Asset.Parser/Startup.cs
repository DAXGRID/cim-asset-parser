using System.Text;
using System.Linq;
using CIM.Asset.Parser.Xmi;
using System.Xml.Linq;
using CIM.Asset.Parser.Cim;

namespace CIM.Asset.Parser
{
    public class Startup
    {
        private readonly IXmiExtractor _xmiExtractor;

        public Startup(IXmiExtractor xmiParse)
        {
            _xmiExtractor = xmiParse;
        }

        public void Start()
        {
            RegisterCodePages();
            Parse("../cim-model/cim.xml", Encoding.GetEncoding("windows-1252"));
        }

        private void Parse(string xmlFilePath, Encoding encoding)
        {
            var xElement = _xmiExtractor.LoadXElement(xmlFilePath, encoding);
            var classes = _xmiExtractor.GetXElementClasses(xElement);
            var generalizations = _xmiExtractor.GetGeneralizations(xElement);

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

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
