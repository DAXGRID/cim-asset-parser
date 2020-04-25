using Xunit;
using System.Linq;
using System.Text;
using FluentAssertions;
using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Xmi;
using FakeItEasy;
using System.IO;
using System.Xml;
using System;
using Microsoft.Extensions.Logging;

namespace CIM.Asset.Parser.Tests.Cim
{
    public class CimParserTests
    {
        public CimParserTests()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Fact]
        public void Parse_ShouldReturnCimEntities_OnValidXElementInput()
        {
            var expectedPowerTransformer = TestDataHelper.LoadTestData<CimEntity>("TestData/power-transformer.json");
            var expectedPowerTransformerEnd = TestDataHelper.LoadTestData<CimEntity>("TestData/power-transformer-end.json");

            var xmlFilePath = "TestData/cim.xml";
            var encoding = Encoding.GetEncoding("windows-1252");

            var xmiExtractorLogger = A.Fake<ILogger<XmiExtractor>>();
            var cimParserLogger = A.Fake<ILogger<CimParser>>();
            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var reader = new StreamReader(xmlFilePath, encoding, true);
            A.CallTo(() => xmlTextReaderFactory.Create(xmlFilePath, encoding)).Returns(new XmlTextReader(reader));

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory, xmiExtractorLogger);

            var cimParser = new Parser.Cim.CimParser(xmiExtractor, cimParserLogger);
            var cimEntities = cimParser.Parse(xmlFilePath, encoding);

            var actualPowerTransformer = cimEntities.FirstOrDefault(x => x.XmiId == expectedPowerTransformer.XmiId);
            var actualPowerTransformerEnd = cimEntities.FirstOrDefault(x => x.XmiId == expectedPowerTransformerEnd.XmiId);

            actualPowerTransformer.Should().BeEquivalentTo(expectedPowerTransformer);
            actualPowerTransformerEnd.Should().BeEquivalentTo(expectedPowerTransformerEnd);
        }

        [Fact]
        public void Parse_ShouldHaveExtactCimEntityCount_OnValidXElementInput()
        {
            var expectedEntityCount = 1929;
            var xmlFilePath = "TestData/cim.xml";
            var encoding = Encoding.GetEncoding("windows-1252");

            var xmiExtractorLogger = A.Fake<ILogger<XmiExtractor>>();
            var cimParserLogger = A.Fake<ILogger<CimParser>>();
            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var reader = new StreamReader(xmlFilePath, encoding, true);
            A.CallTo(() => xmlTextReaderFactory.Create(xmlFilePath, encoding)).Returns(new XmlTextReader(reader));

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory, xmiExtractorLogger);

            var cimParser = new Parser.Cim.CimParser(xmiExtractor, cimParserLogger);
            var cimEntities = cimParser.Parse(xmlFilePath, encoding);

            cimEntities.Count().Should().Be(expectedEntityCount);
        }

        [Fact]
        public void Parse_ShouldHaveExtractTotalAttributeCount_OnValidXElementInput()
        {
            var xmlFilePath = "TestData/cim.xml";
            var encoding = Encoding.GetEncoding("windows-1252");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var xmiExtractorLogger = A.Fake<ILogger<XmiExtractor>>();
            var cimParserLogger = A.Fake<ILogger<CimParser>>();
            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var reader = new StreamReader(xmlFilePath, encoding, true);
            A.CallTo(() => xmlTextReaderFactory.Create(xmlFilePath, encoding)).Returns(new XmlTextReader(reader));

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory, xmiExtractorLogger);

            var cimParser = new Parser.Cim.CimParser(xmiExtractor, cimParserLogger);
            var cimEntities = cimParser.Parse(xmlFilePath, encoding);

            var count = 0;
            cimEntities.ToList().ForEach(x => { count += x.Attributes.Count(); });
            count.Should().Be(9365);
        }

        [Fact]
        public void Parse_ShouldReturnMagneticFieldWithStereoTypeFilled_OnValidXElementInput()
        {
            var xmlFilePath = "TestData/cim.xml";
            var encoding = Encoding.GetEncoding("windows-1252");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var xmiExtractorLogger = A.Fake<ILogger<XmiExtractor>>();
            var cimParserLogger = A.Fake<ILogger<CimParser>>();
            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var reader = new StreamReader(xmlFilePath, encoding, true);
            A.CallTo(() => xmlTextReaderFactory.Create(xmlFilePath, encoding)).Returns(new XmlTextReader(reader));

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory, xmiExtractorLogger);

            var cimParser = new Parser.Cim.CimParser(xmiExtractor, cimParserLogger);
            var cimEntities = cimParser.Parse(xmlFilePath, encoding);

            cimEntities.FirstOrDefault(x => x.Name == "MagneticField").StereoType.Should().Be("CIMDatatype");
        }

        [Fact]
        public void Parse_ShouldThrowArgumentException_OnBeingPasedNullOrEmptyString()
        {
            var xmiExtractor = A.Fake<IXmiExtractor>();
            var cimParserLogger = A.Fake<ILogger<CimParser>>();

            var cimParser = new Parser.Cim.CimParser(xmiExtractor, cimParserLogger);

            cimParser.Invoking(x => x.Parse(null, Encoding.UTF8)).Should().Throw<ArgumentException>();
            cimParser.Invoking(x => x.Parse("", Encoding.UTF8)).Should().Throw<ArgumentException>();
        }
    }
}
