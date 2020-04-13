using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CIM.Asset.Parser.Xmi
{
    public interface IXmiExtractor
    {
        void Parse(string xmlFilePath, Encoding encoding);
        XElement LoadXElement(string xmlFilePath, Encoding encoding);
        IEnumerable<XElement> GetXElementClasses(XElement xElement);
    }
}
