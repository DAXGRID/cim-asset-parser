using Newtonsoft.Json;

namespace CIM.Asset.Parser.FileIO
{
    public interface IJsonFileWriter
    {
        void Write(string path, object content, Formatting jsonFormatting);
    }
}
