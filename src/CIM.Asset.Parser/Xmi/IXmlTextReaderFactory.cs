using System.Xml;
using System.Text;

namespace CIM.Asset.Parser.Xmi
{
    public interface IXmlTextReaderFactory
    {
        public XmlTextReader Create(string path, Encoding encoding);
    }
}
