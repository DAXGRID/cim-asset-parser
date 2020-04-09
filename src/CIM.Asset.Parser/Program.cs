using System;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Linq;

namespace CIM.Asset.Parser
{
    class Program
    {
        public async static Task Main(string[] args)
        {
            var xmlReader = CreateXmlReader();

            await TestReader(xmlReader);

            //await LoadCimDocument(xmlReader);
        }

        private static XmlReader CreateXmlReader()
        {
            var settings = new XmlReaderSettings
            {
                Async = true
            };

            return XmlReader.Create(File.OpenText("../cim-model/cim.xml"), settings);
        }

        private static async Task LoadCimDocument(XmlReader xmlReader)
        {
            var document = new XmlDocument();
            document.Load(xmlReader);

            foreach(var node in document.DocumentElement.ChildNodes)
            {
                var text = node.ToString(); //or loop through its children as well
                Console.WriteLine(text);
            }
        }

        private static async Task TestReader(XmlReader xmlReader)
        {
            using (var reader = xmlReader)
            {
                while (await reader.ReadAsync())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            Console.WriteLine($"Start element {reader.Name} {reader.GetAttribute("tag")} {reader.GetAttribute("value")}");
                            break;
                        case XmlNodeType.Text:
                            Console.WriteLine("Text Node: {0}", reader.GetAttribute("tag"));
                            break;
                        case XmlNodeType.EndElement:
                            Console.WriteLine("End Element {0}", reader.Name);
                            break;
                        default:
                            Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.GetAttribute("tag"));
                            break;
                    }
                }
            }
        }
    }
}
