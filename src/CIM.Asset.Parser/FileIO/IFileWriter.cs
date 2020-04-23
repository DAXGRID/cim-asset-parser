namespace CIM.Asset.Parser.FileIO
{
    public interface IFileWriter
    {
        void Write(string path, string content);
    }
}
