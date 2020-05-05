using System.Collections.Generic;

namespace CIM.Asset.Parser.Asset
{
    public class Namespace
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Entity> Entities { get; set; }
    }
}
