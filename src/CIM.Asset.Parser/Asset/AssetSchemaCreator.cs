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

            return schema;
        }

        private IEnumerable<Namespace> CreateNamespaces(IEnumerable<CimEntity> cimEntities)
        {
            return cimEntities.Select(x => new Namespace
            {
                Id = x.Namespace,
                Entities = CreateEntities(cimEntities.Where(y => y.Namespace == x.Namespace)).ToList()
            });
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

            var derivedEntityIds = CreateObjectGraph(entities, cimEntities);
            CleanObjectGraph(entities, derivedEntityIds);

            return entities;
        }

        private List<string> CreateObjectGraph(List<Entity> entities, IEnumerable<CimEntity> cimEntities)
        {
            var derivedEntityIds = new List<string>();

            foreach (var entity in entities)
            {
                var cimSuperTypeId = cimEntities.FirstOrDefault(x => x.XmiId == entity.Id)?.SuperType;

                if (!(String.IsNullOrEmpty(cimSuperTypeId)))
                {
                    var superType = entities.FirstOrDefault(x => x.Id == cimSuperTypeId);

                    if (!(superType is null) && superType.DerivedEntities is null)
                        superType.DerivedEntities = new List<Entity>();

                    if (!(superType is null))
                    {
                        InsertDerivedEntity(superType, entity);
                        derivedEntityIds.Add(entity.Id);
                    }
                }
            }

            return derivedEntityIds;
        }

        private void CleanObjectGraph(List<Entity> entities, List<string> derivedEntityIds)
        {
            entities.RemoveAll(x => derivedEntityIds.Contains(x.Id));
        }

        private void InsertDerivedEntity(Entity superType, Entity derivedEntity)
        {
            if (superType is null)
                return;

            superType.DerivedEntities.Add(derivedEntity);
        }
    }
}
