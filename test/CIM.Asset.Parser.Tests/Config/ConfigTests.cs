using Xunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CIM.Asset.Parser.Asset;
using CIM.Asset.Parser.FileIO;
using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Xmi;

namespace CIM.Asset.Parser.Tests.Config
{
    public class ConfigTests
    {
        [Fact]
        public void Configure_ShouldReturnServiceProviderWithServices_OnBeingCalled()
        {
            var serviceProvider = Internal.Config.Configure();

            serviceProvider.GetService<ILogger<Startup>>().Should().NotBeNull();
            serviceProvider.GetService<Startup>().Should().NotBeNull();
            serviceProvider.GetService<IXmlTextReaderFactory>().Should().NotBeNull();
            serviceProvider.GetService<IXmiExtractor>().Should().NotBeNull();
            serviceProvider.GetService<ICimParser>().Should().NotBeNull();
            serviceProvider.GetService<IAssetSchemaCreator>().Should().NotBeNull();
            serviceProvider.GetService<IFileWriter>().Should().NotBeNull();
            serviceProvider.GetService<IJsonFileWriter>().Should().NotBeNull();
        }
    }
}
