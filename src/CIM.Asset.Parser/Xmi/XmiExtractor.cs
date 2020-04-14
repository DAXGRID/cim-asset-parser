using System;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace CIM.Asset.Parser.Xmi
{
    public class XmiExtractor : IXmiExtractor
    {
        private readonly IXmlTextReaderFactory _xmlTextReaderFactory;

        public XmiExtractor(IXmlTextReaderFactory xmlTextReaderFactory)
        {
            _xmlTextReaderFactory = xmlTextReaderFactory;
        }

        public XElement LoadXElement(string xmlFilePath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
                throw new ArgumentException($"{nameof(xmlFilePath)} is not allowed to be null or empty");

            return XElement.Load(_xmlTextReaderFactory.Create(xmlFilePath, encoding));
        }

        public IEnumerable<XElement> GetXElementClasses(XElement xElement)
        {
            if (xElement is null)
                throw new ArgumentNullException($"{nameof(xElement)} is not allowed to be null");

            return GetOnLocalName(xElement, EnterpriseArchitectConfig.Class);
        }

        public IEnumerable<XElement> GetGeneralizations(XElement xElement)
        {
            if (xElement is null)
                throw new ArgumentNullException($"{nameof(xElement)} is not allowed to be null");

            return GetOnLocalName(xElement, EnterpriseArchitectConfig.Generalization);
        }

        private IEnumerable<XElement> GetOnLocalName(XElement xElement, string localName)
        {
            return xElement.Descendants().OfType<XElement>()
                .Where(x => x.Name.LocalName == localName);
        }
    }
}
