using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Asset;
using System.Text;

namespace CIM.Asset.Parser
{
    public class Startup
    {
        private readonly ICimParser _cimParser;
        private readonly Encoding _encoding = Encoding.GetEncoding("windows-1252");
        private readonly IAssetSchemaCreator _assetSchemaCreator;

        public Startup(ICimParser cimParser, IAssetSchemaCreator assetSchemaCreator)
        {
            _cimParser = cimParser;
            _assetSchemaCreator = assetSchemaCreator;
        }

        public void Start()
        {
            RegisterCodePages();
            var cimEntities = _cimParser.Parse("../cim-model/cim.xml", _encoding);
            var schema = _assetSchemaCreator.Create(cimEntities);
        }

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
