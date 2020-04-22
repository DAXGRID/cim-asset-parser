using System.Collections.Generic;

namespace CIM.Asset.Parser.Asset
{
    public class Entity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Entity> DerivedEntities { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; }
    }
}
