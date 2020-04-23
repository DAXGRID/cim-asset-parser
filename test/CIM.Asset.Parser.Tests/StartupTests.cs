using Xunit;
using FakeItEasy;
using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Asset;
using CIM.Asset.Parser.FileIO;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CIM.Asset.Parser.Tests
{
    public class StartupTests
    {
        [Fact]
        public void Start_ShouldCallSpecificMethodsInSequence_OnBeingCalled()
        {
            var cimParser = A.Fake<ICimParser>();
            var assetSchemaCreator = A.Fake<IAssetSchemaCreator>();
            var fileWriter = A.Fake<IFileWriter>();
            var logger = A.Fake<ILogger<Startup>>();

            A.CallTo(() => cimParser.Parse(A<string>._, A<Encoding>._)).Returns(A.Dummy<IEnumerable<CimEntity>>());
            A.CallTo(() => assetSchemaCreator.Create(A<IEnumerable<CimEntity>>._)).Returns(A.Dummy<Schema>());

            var startup = new Startup(cimParser, assetSchemaCreator, fileWriter, logger);
            startup.Start();

            A.CallTo(() => cimParser.Parse(A<string>._, A<Encoding>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => assetSchemaCreator.Create(A<IEnumerable<CimEntity>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fileWriter.Write(A<string>._, A<string>._)).MustHaveHappenedOnceExactly();
        }
    }
}
