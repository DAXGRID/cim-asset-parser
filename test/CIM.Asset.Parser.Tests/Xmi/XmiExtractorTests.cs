using System;
using Xunit;
using FluentAssertions;
using CIM.Asset.Parser.Xmi;
using System.Text;
using FakeItEasy;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Linq;

namespace CIM.Asset.Parser.Tests.Xmi
{
    public class XmiExtractorTests
    {
        [Fact]
        public void LoadXElement_ShouldReturnXElement_OnValidPath()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var xmlPath = "TestData/cim.xml";
            var encoding = Encoding.GetEncoding("windows-1252");

            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var reader = new StreamReader(xmlPath, encoding, true);
            A.CallTo(() => xmlTextReaderFactory.Create(xmlPath, encoding)).Returns(new XmlTextReader(reader));

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory);
            var xElement = xmiExtractor.LoadXElement(xmlPath, encoding);

            xElement.Should().NotBeNull();
            xElement.Should().BeOfType<XElement>();
            xElement.Descendants().Count().Should().BeGreaterThan(0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void LoadXElement_ShouldThrowArgumentException_OnNullOrEmptyXmlPath(string xmlPath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("windows-1252");

            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory);

            xmiExtractor.Invoking(x => x.LoadXElement(xmlPath, encoding)).Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("PowerTransformer")]
        [InlineData("PowerTransformerEnd")]
        [InlineData("PetersenCoil")]
        [InlineData("WireSegment")]
        public void GetXElementClasses_ShouldReturnXElementsOfTypeClasses_OnValidXElement(string className)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var xmlPath = "TestData/cim.xml";
            var encoding = Encoding.GetEncoding("windows-1252");

            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var reader = new StreamReader(xmlPath, encoding, true);
            var xmlTextReader = new XmlTextReader(reader);
            var xElement = XElement.Load(xmlTextReader);

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory);
            var classes = xmiExtractor.GetXElementClasses(xElement);

            xElement.Should().NotBeNull();
            xElement.Should().BeOfType<XElement>();
            xElement.Descendants().Count().Should().BeGreaterThan(0);
            xElement.Descendants().FirstOrDefault(x => x.Attribute("name")?.Value?.ToString() == className).Should().NotBeNull();
        }

        [Fact]
        public void GetXElementClasses_ShouldThrowNullArgumentException_OnNullXElement()
        {
            XElement xElement = null;
            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory);

            xmiExtractor.Invoking(x => x.GetXElementClasses(xElement)).Should().Throw<ArgumentNullException>();
        }
    }
}
