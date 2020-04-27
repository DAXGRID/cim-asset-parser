using System.IO;
using Newtonsoft.Json;

namespace CIM.Asset.Parser.FileIO
{
    public class JsonFileWriter : IJsonFileWriter
    {
        public void Write(string path, object content, Formatting jsonFormatting)
        {
            var json = JsonConvert.SerializeObject(content, jsonFormatting);
            File.WriteAllText(path, json);
        }
    }
}
