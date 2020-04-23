using System;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CIM.Asset.Parser.Xmi
{
    public class XmiExtractor : IXmiExtractor
    {
        private readonly IXmlTextReaderFactory _xmlTextReaderFactory;
        private readonly ILogger _logger;

        public XmiExtractor(IXmlTextReaderFactory xmlTextReaderFactory, ILogger<XmiExtractor> logger)
        {
            _xmlTextReaderFactory = xmlTextReaderFactory;
            _logger = logger;
        }

        public XElement LoadXElement(string xmlFilePath, Encoding encoding)
        {
            if (string.IsNullOrEmpty(xmlFilePath))
                throw new ArgumentException($"{nameof(xmlFilePath)} is not allowed to be null or empty");

            _logger.LogInformation($"Loads XElements from {xmlFilePath}");

            return XElement.Load(_xmlTextReaderFactory.Create(xmlFilePath, encoding));
        }

        public IEnumerable<XElement> GetXElementClasses(XElement xElement)
        {
            if (xElement is null)
                throw new ArgumentNullException($"{nameof(xElement)} is not allowed to be null");

            _logger.LogInformation($"Loads Classes");

            return GetOnLocalName(xElement, EnterpriseArchitectConfig.Class);
        }

        public IEnumerable<XElement> GetGeneralizations(XElement xElement)
        {
            if (xElement is null)
                throw new ArgumentNullException($"{nameof(xElement)} is not allowed to be null");

            _logger.LogInformation($"Loads Generalizations");

            return GetOnLocalName(xElement, EnterpriseArchitectConfig.Generalization);
        }

        private IEnumerable<XElement> GetOnLocalName(XElement xElement, string localName)
        {
            return xElement.Descendants().OfType<XElement>()
                .Where(x => x.Name.LocalName == localName);
        }
    }
}
