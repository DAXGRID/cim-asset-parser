using System;
using System.Collections.Generic;
using System.Linq;
using CIM.Asset.Parser.Cim;
using Microsoft.Extensions.Logging;

namespace CIM.Asset.Parser.Asset
{
    public class AssetSchemaCreator : IAssetSchemaCreator
    {
        private readonly ILogger _logger;
        private Dictionary<string, CimEntity> _lookupCimEntities;
        private Dictionary<string, Entity> _lookupEntities;

        public AssetSchemaCreator(ILogger<AssetSchemaCreator> logger)
        {
            _logger = logger;
        }

        public Schema Create(IEnumerable<CimEntity> cimEntities)
        {
            if (cimEntities is null)
                throw new ArgumentNullException($"{nameof(cimEntities)} cannot be null");

            if (cimEntities.Count() <= 0)
                return new Schema { Namespaces = new List<Namespace>() };

            _logger.LogInformation($"Creating asset schema entities for '{cimEntities.Count()}' CIM entities");

            var namespaces = CreateNamespaces(cimEntities).ToList();
            var schema = new Schema { Namespaces = namespaces };

            _logger.LogInformation($"Finished creating asset schema entities for '{cimEntities.Count()}' CIM entities");

            return schema;
        }

        private IEnumerable<Namespace> CreateNamespaces(IEnumerable<CimEntity> cimEntities)
        {
            _logger.LogInformation("Creating lookup table for cim entities");
            _lookupCimEntities = cimEntities.ToDictionary(x => x.XmiId, x => x);

            _logger.LogInformation("Creating entities");
            var entities = CreateEntities(cimEntities);

            _logger.LogInformation($"Creating lookup table for entities {entities.Count()}");
            _lookupEntities = entities.ToDictionary(x => x.Id, x => x);

            var derivedEntityIds = CreateObjectGraph(entities, cimEntities);
            CleanObjectGraph(entities, derivedEntityIds);

            _logger.LogInformation($"Creating lookup for cim namespaces");
            var lookupNamespaces = cimEntities.ToLookup(x => x.Namespace, x => entities.FirstOrDefault(y => y.Id == x.XmiId));

            return lookupNamespaces.Select(x => new Namespace
                {
                    Id = x.Key,
                    Entities = x.Where(x => !(x is null)).ToList()
                });
        }

        private List<Entity> CreateEntities(IEnumerable<CimEntity> cimEntities)
        {
            return cimEntities.Select(x => new Entity
            {
                Id = x.XmiId,
                Name = x.Name,
                Description = x.Description,
                Attributes = x.Attributes.Select(y => new Asset.Attribute { Description = y.Description, Name = y.Name })
            }).ToList();
        }

        private List<string> CreateObjectGraph(List<Entity> entities, IEnumerable<CimEntity> cimEntities)
        {
            var derivedEntityIds = new List<string>();

            foreach (var entity in entities)
            {
                var cimSuperTypeId = _lookupCimEntities.ContainsKey(entity.Id) ? _lookupCimEntities[entity.Id]?.SuperType : null;

                if (!(String.IsNullOrEmpty(cimSuperTypeId)))
                {
                    var superType = _lookupEntities.ContainsKey(cimSuperTypeId) ? _lookupEntities[cimSuperTypeId] : null;

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

            _logger.LogInformation($"Cleaning object graph");
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
