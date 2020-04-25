using System;
using System.IO;
using Newtonsoft.Json;

namespace CIM.Asset.Parser.Tests
{
    public static class TestDataHelper
    {
        public static T LoadTestData<T>(string filePath)
        {
            var path = Path.IsPathRooted(filePath)
                ? filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);

            if (!File.Exists(path))
                throw new ArgumentException($"Could not find file at path: {path}");

            var fileContent = File.ReadAllText(path);

            var deserializedObject = JsonConvert.DeserializeObject<T>(fileContent);
           
            return deserializedObject;
        }
    }
}
