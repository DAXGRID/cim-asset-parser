using CIM.Asset.Parser.Cim;
using CIM.Asset.Parser.Asset;
using CIM.Asset.Parser.FileIO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace CIM.Asset.Parser
{
    public class Startup
    {
        private readonly ICimParser _cimParser;
        private readonly string _encoding = "windows-1252";
        private readonly IAssetSchemaCreator _assetSchemaCreator;
        private readonly IFileWriter _fileWriter;
        private readonly ILogger _logger;

        public Startup(ICimParser cimParser, IAssetSchemaCreator assetSchemaCreator, IFileWriter fileWriter, ILogger<Startup> logger)
        {
            _cimParser = cimParser;
            _assetSchemaCreator = assetSchemaCreator;
            _fileWriter = fileWriter;
            _logger = logger;
        }

        public void Start()
        {
            RegisterCodePages();

            _logger.LogInformation($"Starting parsing");
            var cimEntities = _cimParser.Parse("../cim-model/cim.xml", Encoding.GetEncoding(_encoding));
            var schema = _assetSchemaCreator.Create(cimEntities);

            _logger.LogInformation($"Writing Schema to JSON");
            WriteSchemaToDiskAsJson("schema.json", schema);
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
