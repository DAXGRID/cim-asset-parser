using System.Text;

namespace CIM.Asset.Parser.Xmi
{
    public interface IXmiParse
    {
        void Parse(string xmlFilePath, Encoding encoding);
    }
}
