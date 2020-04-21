using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Xunit.Sdk;
using System.Linq;
using System.Collections;

namespace CIM.Asset.Parser.Tests
{
    public enum JsonDataType
    {
        JObject,
        JArray
    }

    public class JsonFileDataAttribute : DataAttribute
    {
        private readonly string[] _filePaths;

        /// <summary>
        /// Load data from a JSON files as the data source for a theory
        /// </summary>
        /// <param name="filePaths">The absolute or relative paths to the JSON files to load</param>
        public JsonFileDataAttribute(params string[] filePaths)
        {
            _filePaths = filePaths;
        }

        /// <inheritdoc/>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
                throw new ArgumentNullException(nameof(testMethod));

            var parametersTypes = testMethod.GetParameters().Select(p => p.ParameterType);

            var result = new List<object[]>();
            _filePaths.ToList().ForEach(p => result.AddRange(GetFileData(p, parametersTypes)));

            return result;
        }

        private IEnumerable<object[]> GetFileData(string filePath, IEnumerable<Type> parametersTypes)
        {
            var path = Path.IsPathRooted(filePath)
            ? filePath
            : Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);

            if (!File.Exists(path))
                throw new ArgumentException($"Could not find file at path: {path}");

            var fileData = File.ReadAllText(filePath);

            var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

            var rawData = JsonConvert.DeserializeObject<object[][]>(fileData);

            foreach (var parameter in parametersTypes)
                Console.WriteLine(parameter.Name);


            var result = rawData.Select(x =>
            {
                return x.Select((y, index) =>
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(parametersTypes.ElementAt(index)))
                            return ((JArray)y).ToObject(parametersTypes.ElementAt(index), JsonSerializer.Create(jsonSerializerSettings));
                        else
                            return ((JObject)y).ToObject(parametersTypes.ElementAt(index), JsonSerializer.Create(jsonSerializerSettings));
                    }).ToArray();
            });

            return result;
        }
    }
}
