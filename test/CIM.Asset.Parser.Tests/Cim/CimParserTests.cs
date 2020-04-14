using Xunit;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using FluentAssertions;
using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Xmi;
using FakeItEasy;
using System.IO;
using System.Xml;

namespace CIM.Asset.Parser.Tests.Cim
{
    public class CimParserTests
    {
        [Fact]
        public void Parse_ShouldReturnCimEntities_OnValidXElementInput()
        {
            var xmlFilePath = "TestData/cim.xml";
            var encoding = Encoding.GetEncoding("windows-1252");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var exceptedPowerTransformer = new CimEntity
            {
                Name = "PowerTransformer",
                XmiId = "EAID_3D50627C_324C_4e9d_AB5C_592E0A1E0AFE",
                Namespace = "EAPK_6C99E9CA_2035_4035_B77F_9217E17D86F4",
                Description = "An electrical device consisting of  two or more coupled windings, with or without a magnetic core, for introducing mutual coupling between electric circuits. Transformers can be used to control voltage and phase shift (active power flow).&#xA;A power transformer may be composed of separate transformer tanks that need not be identical.&#xA;A power transformer can be modelled with or without tanks and is intended for use in both balanced and unbalanced representations. A power transformer typically has two terminals, but may have one (grounding), three or more terminals. &#xA;The inherited association ConductingEquipment.BaseVoltage should not be used.  The association from TransformerEnd to BaseVoltage should be used instead.",
                SuperType = "EAID_7C5F19FB_5253_430c_8E53_BDCF7EBCE4C1",
                Attributes = new List<Attribute>()
            };

            var xmlTextReaderFactory = A.Fake<IXmlTextReaderFactory>();
            var reader = new StreamReader(xmlFilePath, encoding, true);
            A.CallTo(() => xmlTextReaderFactory.Create(xmlFilePath, encoding)).Returns(new XmlTextReader(reader));

            var xmiExtractor = new XmiExtractor(xmlTextReaderFactory);
            var cimParser = new CimParser(xmiExtractor);

            var cimEntities = cimParser.Parse(xmlFilePath, encoding);

            cimEntities.Should().NotBeNull().Should();
            cimEntities.Should().NotBeEmpty();
            cimEntities.FirstOrDefault(x => x.Name == "PowerTransformer").Should().NotBeNull();
            // cimEntities.FirstOrDefault(x => x.Name == "PowerTransformer").Should().BeEquivalentTo(exceptedPowerTransformer);
        }
    }
}
