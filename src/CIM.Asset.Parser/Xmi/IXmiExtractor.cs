using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CIM.Asset.Parser.Xmi
{
    public interface IXmiExtractor
    {
        XElement LoadXElement(string xmlFilePath, Encoding encoding);
        IEnumerable<XElement> GetXElementClasses(XElement xElement);
        IEnumerable<XElement> GetGeneralizations(XElement xElement);
    }
}
