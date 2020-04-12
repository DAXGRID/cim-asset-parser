using System;
using System.Xml;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
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
