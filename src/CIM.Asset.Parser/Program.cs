using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace CIM.Asset.Parser
{
    class Program
    {
        public static void Main(string[] args)
        {
            RegisterCodePages();

            var xmlReader = CreateXmlReader("../cim-model/cim.xml");
            LoadClasses(xmlReader);
        }

        private static XmlTextReader CreateXmlReader(string path)
        {
            var reader = new StreamReader(path, Encoding.GetEncoding("windows-1252"), true);
            return new XmlTextReader(reader);
        }

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
        }

        private static void LoadClasses(XmlTextReader xmlReader)
        {
            var xElement = XElement.Load(xmlReader);

            var classes = xElement.Descendants().OfType<XElement>()
                .Where(x => x.Name.LocalName == "Class");

            var cimEntities = classes?
                .Select(x => new CimEntity
                    {
                        Name = x.Attribute("name").Value?.ToString(),
                        XmiId = x.Attribute("xmi.id").Value?.ToString(),
                        Description = x.Descendants().OfType<XElement>()
                            .Where(x => x.Name.LocalName == "TaggedValue")
                                .FirstOrDefault(x => x.Attribute("tag")?.Value?.ToString() == "documentation")?.Attribute("value")?.Value?.ToString(),
                        Attributes = x.Descendants().OfType<XElement>()
                            .Where(y => y.Name.LocalName == "Attribute").Select(z => new Attribute { Name = z.Attribute("name").Value?.ToString() }),
                        Namespace = x.Attribute("namespace")?.Value?.ToString(),
                    });

            foreach (var cimEntity in cimEntities)
            {
                Console.WriteLine(cimEntity.Namespace + " " + cimEntity.Name);
                Console.WriteLine("++ " + cimEntity.Description);

                foreach (var tag in cimEntity.Attributes)
                {
                    Console.WriteLine("------ " + tag.Name);
                }
            }

            var transformerNamespace = cimEntities.Where(x => x.Namespace == "EAPK_6C99E9CA_2035_4035_B77F_9217E17D86F4").OrderBy(x => x.Name);
            foreach (var transformer in transformerNamespace)
            {
                Console.WriteLine(transformer.Name);
            }
        }
    }
}
