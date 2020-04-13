using System.Text;
using System.Xml.Linq;

namespace CIM.Asset.Parser.Xmi
{
    public interface IXmiExtractor
    {
        void Parse(string xmlFilePath, Encoding encoding);
        XElement LoadXElement(string xmlFilePath, Encoding encoding);
    }
}
