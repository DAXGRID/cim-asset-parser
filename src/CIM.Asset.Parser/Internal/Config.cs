using Microsoft.Extensions.DependencyInjection;

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
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
