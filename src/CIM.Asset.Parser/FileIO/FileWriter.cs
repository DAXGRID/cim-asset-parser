namespace CIM.Asset.Parser.FileIO
{
    public class FileWriter : IFileWriter
    {
        public void Write(string path, string content)
        {
            System.IO.File.WriteAllText(path, content);    
        }
    }
}
