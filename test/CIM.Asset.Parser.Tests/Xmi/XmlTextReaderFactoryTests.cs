using Xunit;
using FluentAssertions;
using CIM.Asset.Parser.Xmi;
using System.Xml;
using System.Text;
using System;

namespace CIM.Asset.Parser.Tests.Xmi
{
    public class XmiTextReaderFactoryTests
    {
        [Fact]
        public void Create_ShouldReturnCreatedXmlTextReader_OnBeingCalledWithValidParameters()
        {
            var factory = new XmlTextReaderFactory();
            var xmlTextReader = factory.Create("TestData/example.xml", Encoding.UTF8);

            xmlTextReader.Should().NotBeNull();
            xmlTextReader.Should().BeOfType<XmlTextReader>();
        }

        [Fact]
        public void Create_ShouldThrowInvalidArgumentError_OnBeginCalledWithNullPath()
        {
            var factory = new XmlTextReaderFactory();

            factory.Invoking(x => x.Create(null, Encoding.UTF8)).Should().Throw<ArgumentNullException>();
        }
    }
}
