using CIM.Asset.Parser.Xmi;
using CIM.Asset.Parser.Cim;
using System.Text;

namespace CIM.Asset.Parser
{
    public class Startup
    {
        private readonly ICimParser _cimParser;

        public Startup(ICimParser cimParser)
        {
            _cimParser = cimParser;
        }

        public void Start()
        {
            RegisterCodePages();
            _cimParser.Parse("../cim-model/cim.xml", Encoding.GetEncoding("windows-1252"));
        }

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
