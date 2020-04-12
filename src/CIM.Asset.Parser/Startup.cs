using System.Text;
using CIM.Asset.Parser.Xmi;

namespace CIM.Asset.Parser
{
    public class Startup
    {
        private readonly IXmiParse _xmiParse;

        public Startup(IXmiParse xmiParse)
        {
            _xmiParse = xmiParse;
        }

        public void Start()
        {
            RegisterCodePages();
            _xmiParse.Parse("../cim-model/cim.xml", Encoding.GetEncoding("windows-1252"));
        }

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
