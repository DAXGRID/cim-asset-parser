using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CIM.Asset.Parser.Xmi;
using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Asset;
using CIM.Asset.Parser.FileIO;

namespace CIM.Asset.Parser.Internal
{
    public static class Config
    {
        public static ServiceProvider Configure()
        {
            var serviceProvider = BuildServiceProvider();
            return serviceProvider;
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(x => x.AddConsole())
                .AddSingleton<Startup>()
                .AddTransient<IXmlTextReaderFactory, XmlTextReaderFactory>()
                .AddTransient<IXmiExtractor, XmiExtractor>()
                .AddTransient<ICimParser, CimParser>()
                .AddTransient<IAssetSchemaCreator, AssetSchemaCreator>()
                .AddScoped<IJsonFileWriter, JsonFileWriter>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
