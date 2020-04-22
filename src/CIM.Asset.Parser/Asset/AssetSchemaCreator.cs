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

            var namespaces = CreateNamespaces(cimEntities).ToList();
            var schema = new Schema { Namespaces = namespaces };

            var data = schema.Namespaces.FirstOrDefault().Entities.FirstOrDefault(x => x?.DerivedEntities?.Count() > 0);
            Console.WriteLine("Data: " + data?.Name);

            return schema;
        }

        private IEnumerable<Namespace> CreateNamespaces(IEnumerable<CimEntity> cimEntities)
        {
            var namespaces = cimEntities.Select(x => new Namespace
                {
                    Id = x.Namespace,
                    Entities = CreateEntities(cimEntities.Where(y => y.Namespace == x.Namespace)).ToList()
                });

            return namespaces;
        }

        private List<Entity> CreateEntities(IEnumerable<CimEntity> cimEntities)
        {
            var entities = cimEntities.Select(x => new Entity
                {
                    Id = x.XmiId,
                    Name = x.Name,
                    Description = x.Description,
                    Attributes = x.Attributes.Select(y => new Asset.Attribute { Description = y.Description, Name = y.Name })
                }).ToList();

            foreach (var entity in entities)
            {
                var cimSuperTypeId = cimEntities.FirstOrDefault(x => x.XmiId == entity.Id)?.SuperType;

                if (!(String.IsNullOrEmpty(cimSuperTypeId)))
                {
                    var superType = entities.FirstOrDefault(x => x.Id == cimSuperTypeId);

                    if (!(superType is null) && superType.DerivedEntities is null)
                        superType.DerivedEntities = new List<Entity>();

                    if (superType != null)
                        superType.DerivedEntities.Add(entity);
                }
            }

            return entities;
        }
    }
}
