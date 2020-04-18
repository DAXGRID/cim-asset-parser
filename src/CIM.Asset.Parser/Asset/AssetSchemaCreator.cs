using System;
using System.Collections.Generic;
using System.Linq;
using CIM.Asset.Parser.Cim;

namespace CIM.Asset.Parser.Asset
{
    public class AssetSchemaCreator : IAssetSchemaCreator
    {
        public Schema Create(IEnumerable<CimEntity> cimEntities)
        {
            if (cimEntities is null)
                throw new ArgumentNullException($"{nameof(cimEntities)} cannot be null");

            if (cimEntities.Count() <= 0)
                return new Schema { Namespaces = new List<Namespace>() };

            var namespaces = CreateNamespaces(cimEntities);

            var schema = new Schema { Namespaces = namespaces };

            return schema;
        }

        private IEnumerable<Namespace> CreateNamespaces(IEnumerable<CimEntity> cimEntities)
        {
            var namespaces = cimEntities.Select(x => new Namespace { Name = x.Name, Id = x.XmiId, Entities = new List<Entity>() });

            return namespaces;
        }
    }
}
