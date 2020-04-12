using System.Xml;
using System.IO;
using System.Text;
using System;

namespace CIM.Asset.Parser.Xmi
{
    public class XmlTextReaderFactory : IXmlTextReaderFactory
    {
        public XmlTextReader Create(string path, Encoding encoding)
        {
            if (path is null)
                throw new ArgumentNullException($"{nameof(path)} cannot be null");

            var reader = new StreamReader(path, encoding, true);
            return new XmlTextReader(reader);
        }
    }
}
