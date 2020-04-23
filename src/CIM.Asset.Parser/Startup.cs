using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Asset;
using CIM.Asset.Parser.FileIO;
using System.Text;
using Newtonsoft.Json;

namespace CIM.Asset.Parser
{
    public class Startup
    {
        private readonly ICimParser _cimParser;
        private readonly string _encoding = "windows-1252";
        private readonly IAssetSchemaCreator _assetSchemaCreator;
        private readonly IFileWriter _fileWriter;

        public Startup(ICimParser cimParser, IAssetSchemaCreator assetSchemaCreator, IFileWriter fileWriter)
        {
            _cimParser = cimParser;
            _assetSchemaCreator = assetSchemaCreator;
            _fileWriter = fileWriter;
        }

        public void Start()
        {
            RegisterCodePages();
            var cimEntities = _cimParser.Parse("../cim-model/cim.xml", Encoding.GetEncoding(_encoding));
            var schema = _assetSchemaCreator.Create(cimEntities);
            WriteSchemaToDiskAsJson("../schema.json", schema);
        }

        private void WriteSchemaToDiskAsJson(string path, Schema schema)
        {
            var schemaAsJson = JsonConvert.SerializeObject(schema, Formatting.Indented);
            _fileWriter.Write(path, schemaAsJson);
        }

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
