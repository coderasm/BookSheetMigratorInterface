using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BookSheetMigration
{
    public class SoapRequestMessage
    {
        private const string firstChildOfBodyPath = "//soap:Body/*[1]";
        private const string soapNamespace = "http://schemas.xmlsoap.org/soap/envelope/";

        private HttpRequestMessage soapRequestMessage;

        public SoapRequestMessage(HttpRequestMessage soapRequestMessage)
        {
            this.soapRequestMessage = soapRequestMessage;
        }

        public async Task<XElement> sendMessage()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.SendAsync(soapRequestMessage))
                {
                    var soapResponse = await response.Content.ReadAsStringAsync();
                    return parseSoapResponse(soapResponse);
                }
            }
        }

        private XElement parseSoapResponse(string response)
        {
            var parsedResponse = XElement.Parse(response);
            return stripEnvelopeAndBodyTags(parsedResponse);
        }

        private XElement stripEnvelopeAndBodyTags(XElement parsedResponse)
        {
            var namespacemanager = new XmlNamespaceManager(new NameTable());
            namespacemanager.AddNamespace("soap", soapNamespace);
            return parsedResponse.XPathSelectElement(firstChildOfBodyPath, namespacemanager);
        }
    }
}
