using System;

namespace CIM.Asset.Parser.Xmi
{
    public class XmiParse : IXmiParse
    {
        public void Parse(string xmlFilePath)
        {
            Console.WriteLine(xmlFilePath);
        }
    }
}
