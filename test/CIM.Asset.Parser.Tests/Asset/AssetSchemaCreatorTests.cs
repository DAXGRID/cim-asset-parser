using Xunit;
using System;
using FluentAssertions;
using CIM.Asset.Parser.Asset;
using CIM.Asset.Parser.Cim;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using Newtonsoft.Json;
using System.IO;

namespace CIM.Asset.Parser.Tests.Asset
{
    public class AssetSchemaCreatorTests
    {
        [Fact]
        public void Create_ShouldReturnSchemaContaingNamespacesContaingCimEntities_OnSuppliedCimEntities()
        {
            var cimEntities = TestDataHelper.LoadTestData<IEnumerable<CimEntity>>("TestData/entity-group.json");
            var logger = A.Fake<ILogger<AssetSchemaCreator>>();
            var assetSchemaCreator = new AssetSchemaCreator(logger);
            var schema = assetSchemaCreator.Create(cimEntities);

            schema.Should().NotBeNull();
            schema.Namespaces.Should().NotBeNull();
            schema.Namespaces.FirstOrDefault().Entities.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public void Create_ShouldReturnSchema_OnSuppliedCimEntities()
        {
            var cimEntities = TestDataHelper.LoadTestData<IEnumerable<CimEntity>>("TestData/entity-group.json");
            var expectedSchema = JsonConvert.DeserializeObject<Schema>(File.ReadAllText("TestData/expected-schema.json"));

            var logger = A.Fake<ILogger<AssetSchemaCreator>>();
            var assetSchemaCreator = new AssetSchemaCreator(logger);
            var schema = assetSchemaCreator.Create(cimEntities);

            schema.Should().BeEquivalentTo(expectedSchema);
        }

        [Fact]
        public void Create_ShouldReturnEnergyConnectionWithDerivedEntities_OnSuppliedCimEntities()
        {
            var cimEntities = TestDataHelper.LoadTestData<IEnumerable<CimEntity>>("TestData/entity-group.json");
            var logger = A.Fake<ILogger<AssetSchemaCreator>>();
            var assetSchemaCreator = new AssetSchemaCreator(logger);
            var schema = assetSchemaCreator.Create(cimEntities);

            schema.Namespaces.FirstOrDefault()
                .Entities.FirstOrDefault(x => x.Name == "EnergyConnection")
                .DerivedEntities.Count().Should().BeGreaterThan(0);
        }

        [Fact]
        public void Create_ShouldReturnAsyncronousMachineKindWithNoDerivedEntities_OnSuppliedCimEntities()
        {
            var cimEntities = TestDataHelper.LoadTestData<IEnumerable<CimEntity>>("TestData/entity-group.json");
            var logger = A.Fake<ILogger<AssetSchemaCreator>>();
            var assetSchemaCreator = new AssetSchemaCreator(logger);
            var schema = assetSchemaCreator.Create(cimEntities);

            schema.Namespaces.FirstOrDefault().Entities.FirstOrDefault(x => x.Name == "AsynchronousMachineKind").DerivedEntities.Should().BeEmpty();
        }

        [Fact]
        public void Create_ShouldReturnExactAttributeCount_OnSuppliedCimEntities()
        {
            var cimEntities = TestDataHelper.LoadTestData<IEnumerable<CimEntity>>("TestData/entity-group.json");
            var logger = A.Fake<ILogger<AssetSchemaCreator>>();
            var assetSchemaCreator = new AssetSchemaCreator(logger);
            var schema = assetSchemaCreator.Create(cimEntities);

            var attributesCount = 0;
            schema.Namespaces.ToList().ForEach(x => { x.Entities.ForEach(y => { attributesCount += y.Attributes.Count(); }); });
            attributesCount.Should().Be(171);
        }

        [Fact]
        public void Create_ShouldThrowNullArgumentException_OnCimEntitiesCollectionBeingNull()
        {
            var logger = A.Fake<ILogger<AssetSchemaCreator>>();
            var assetSchemaCreator = new AssetSchemaCreator(logger);

            assetSchemaCreator.Invoking(x => x.Create(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_ShouldReturnEmptySchema_OnCimEntitiesCollectionBeingEmpty()
        {
            var expectedSchema = new Schema
            {
                Namespaces = new List<Namespace>()
            };

            var cimEntities = new List<CimEntity>();

            var logger = A.Fake<ILogger<AssetSchemaCreator>>();
            var assetSchemaCreator = new AssetSchemaCreator(logger);

            var schema = assetSchemaCreator.Create(cimEntities);

            schema.Should().BeEquivalentTo(expectedSchema);
        }
    }
}
