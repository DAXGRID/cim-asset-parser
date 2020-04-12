using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using CIM.Asset.Parser.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace CIM.Asset.Parser
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = Config.Configure();
            var startup = serviceProvider.GetService<Startup>();
            startup.Start();

            // RegisterCodePages();
            // var xmlReader = CreateXmlReader("../cim-model/cim.xml");
            // LoadClasses(xmlReader);
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


            foreach (var cimEntity in cimEntities)
            {
                Console.WriteLine(cimEntity.Namespace + " " + cimEntity.Name);
                Console.WriteLine("++ " + cimEntity.Description);
                Console.WriteLine("// Supertype: " + cimEntity.SuperType);

                foreach (var tag in cimEntity.Attributes)
                {
                    Console.WriteLine("------ " + tag.Name);
                    Console.WriteLine("------Descrption------ " + tag.Description);
                }
            }
        }
    }
}
