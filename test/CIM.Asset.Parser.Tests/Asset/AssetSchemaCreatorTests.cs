using Xunit;
using System;
using FluentAssertions;
using CIM.Asset.Parser.Asset;
using CIM.Asset.Parser.Cim;
using System.Collections.Generic;
using System.Linq;

namespace CIM.Asset.Parser.Tests.Asset
{
    public class AssetSchemaCreatorTests
    {
        [Theory]
        [JsonFileData("TestData/entity-group.json")]
        public void Create_ShouldReturnSchemaContaingCimEntities_OnSuppliedCimEntities(IEnumerable<CimEntity> cimEntities)
        {
            var assetSchemaCreator = new AssetSchemaCreator();
            var schema = assetSchemaCreator.Create(cimEntities);

            schema.Namespaces.Count().Should().BePositive();
        }

        [Fact]
        public void Create_ShouldThrowNullArgumentException_OnCimEntitiesCollectionBeingNull()
        {
            var assetSchemaCreator = new AssetSchemaCreator();

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
            var assetSchemaCreator = new AssetSchemaCreator();

            var schema = assetSchemaCreator.Create(cimEntities);

            schema.Should().BeEquivalentTo(expectedSchema);
        }
    }
}
