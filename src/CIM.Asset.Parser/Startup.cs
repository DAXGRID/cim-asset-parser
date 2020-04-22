using CIM.Asset.Parser.Cim;
using System.Text;

namespace CIM.Asset.Parser
{
    public class Startup
    {
        private readonly ICimParser _cimParser;
        private readonly Encoding _encoding = Encoding.GetEncoding("windows-1252");

        public Startup(ICimParser cimParser)
        {
            _cimParser = cimParser;
        }

        public void Start()
        {
            RegisterCodePages();
            _cimParser.Parse("../cim-model/cim.xml", _encoding);
        }

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
