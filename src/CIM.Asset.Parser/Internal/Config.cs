using Microsoft.Extensions.DependencyInjection;
using CIM.Asset.Parser.Xmi;
using CIM.Asset.Parser.Cim;

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
                .AddSingleton<Startup>()
                .AddTransient<IXmlTextReaderFactory, XmlTextReaderFactory>()
                .AddTransient<IXmiExtractor, XmiExtractor>()
                .AddTransient<ICimParser, CimParser>()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
