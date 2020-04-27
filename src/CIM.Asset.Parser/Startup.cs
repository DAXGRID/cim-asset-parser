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
        private readonly ILogger _logger;
        private readonly IJsonFileWriter _jsonFileWriter;
        private const string _outputPath = "schema.json";
        private const string _inputPath = "../cim-model/cim.xml";

        public Startup(ICimParser cimParser,
                       IAssetSchemaCreator assetSchemaCreator,
                       ILogger<Startup> logger,
                       IJsonFileWriter jsonFileWriter)
        {
            _cimParser = cimParser;
            _assetSchemaCreator = assetSchemaCreator;
            _logger = logger;
            _jsonFileWriter = jsonFileWriter;
        }

        public void Start()
        {
            RegisterCodePages();

            _logger.LogInformation($"Starting parsing");
            var cimEntities = _cimParser.Parse(_inputPath, Encoding.GetEncoding(_encoding));
            var schema = _assetSchemaCreator.Create(cimEntities);

            _logger.LogInformation($"Writing Schema to JSON {_outputPath}");
            _jsonFileWriter.Write(_outputPath, schema, Formatting.Indented);
        }

        private static void RegisterCodePages()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}
