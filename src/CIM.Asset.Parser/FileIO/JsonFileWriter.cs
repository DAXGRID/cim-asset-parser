using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CIM.Asset.Parser.FileIO
{
    public class JsonFileWriter : IJsonFileWriter
    {
        public void Write(string path, object content, Formatting jsonFormatting)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = jsonFormatting
            };

            var json = JsonConvert.SerializeObject(content, serializerSettings);

            File.WriteAllText(path, json);
        }
    }
}
