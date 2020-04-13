using CIM.Asset.Parser.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace CIM.Asset.Parser
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = Config.Configure();
            var startup = serviceProvider.GetService<Startup>();
            startup.Start();
        }
    }
}
