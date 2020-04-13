using System.Text;

namespace CIM.Asset.Parser.Xmi
{
    public interface IXmiExtractor
    {
        void Parse(string xmlFilePath, Encoding encoding);
    }
}
