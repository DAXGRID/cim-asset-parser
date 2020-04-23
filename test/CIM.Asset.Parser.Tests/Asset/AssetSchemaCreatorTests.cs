using Xunit;
using System;
using FluentAssertions;
using CIM.Asset.Parser.Asset;
using CIM.Asset.Parser.Cim;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using FakeItEasy;

namespace CIM.Asset.Parser.Tests.Asset
{
    public class AssetSchemaCreatorTests
    {
        [Theory]
        [JsonFileData("TestData/entity-group.json")]
        public void Create_ShouldReturnSchemaContaingCimEntities_OnSuppliedCimEntities(IEnumerable<CimEntity> cimEntities)
        {
            var logger = A.Fake<ILogger<AssetSchemaCreator>>();
            var assetSchemaCreator = new AssetSchemaCreator(logger);
            var schema = assetSchemaCreator.Create(cimEntities);

            schema.Namespaces.Count().Should().BePositive();
            schema.Namespaces.FirstOrDefault().Id.Should().Match(cimEntities.FirstOrDefault().Namespace);
            schema.Namespaces.FirstOrDefault().Entities.Count().Should().BeGreaterThan(0);
            schema.Namespaces.FirstOrDefault().Entities.FirstOrDefault().Attributes.Count().Should().BeGreaterThan(0);
            schema.Namespaces.FirstOrDefault()
                .Entities.FirstOrDefault(x => x.Name == "EnergyConnection")
                .DerivedEntities.Count().Should().BeGreaterThan(0);
            schema.Namespaces.FirstOrDefault().Entities.FirstOrDefault(x => x.Name == "AsynchronousMachineKind").DerivedEntities.Should().BeNull();
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
