using System.Collections.Generic;

namespace CIM.Asset.Parser.Cim
{
    public class CimEntity
    {
        public string Name { get; set; }
        public string XmiId { get; set; }
        public string Description { get; set; }
        public string Namespace { get; set; }
        public string SuperType { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; }
    }
}
