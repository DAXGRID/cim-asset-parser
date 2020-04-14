using System.Text;
using System.Collections.Generic;

namespace CIM.Asset.Parser.Cim
{
    public interface ICimParser
    {
        IEnumerable<CimEntity> Parse(string xmlFilePath, Encoding encoding);
    }
}
