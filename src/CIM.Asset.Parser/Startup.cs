using System.Text;
using CIM.Asset.Parser.Xmi;

namespace CIM.Asset.Parser
{
    public class Startup
    {
        private readonly IXmiExtractor _xmiExtractor;

        public Startup(IXmiExtractor xmiParse)
        {
            _xmiExtractor = xmiParse;
        }

        public void Start()
        {
            RegisterCodePages();
            _xmiExtractor.Parse("../cim-model/cim.xml", Encoding.GetEncoding("windows-1252"));
        }

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
