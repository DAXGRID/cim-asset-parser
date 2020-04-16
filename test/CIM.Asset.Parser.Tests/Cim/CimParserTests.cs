using Xunit;
using System.Linq;
using System.Text;
using FluentAssertions;
using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Xmi;
using FakeItEasy;
using System.IO;
using System.Xml;
using System.Collections.Generic;

namespace CIM.Asset.Parser.Tests.Cim
{
    public class CimParserTests
    {
        [Theory]
        [JsonFileData("TestData/power-transformer.json")]
        [JsonFileData("TestData/power-transformer-end.json")]
        public void Parse_ShouldReturnCimEntities_OnValidXElementInput(CimEntity expected)
        {
            var xmlFilePath = "TestData/cim.xml";
            var encoding = Encoding.GetEncoding("windows-1252");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var reader = new StreamReader(xmlFilePath, encoding, true);
            A.CallTo(() => xmlTextReaderFactory.Create(xmlFilePath, encoding)).Returns(new XmlTextReader(reader));

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory);

            var cimParser = new Parser.Cim.CimParser(xmiExtractor);
            var cimEntities = cimParser.Parse(xmlFilePath, encoding);


            cimEntities.Should().NotBeNull().Should();
            cimEntities.Should().NotBeEmpty();

            var actualEntity = cimEntities.FirstOrDefault(x => x.XmiId == expected.XmiId);

            actualEntity.Should().NotBeNull();
            actualEntity.Should().BeEquivalentTo(expected);
        }
    }
}
