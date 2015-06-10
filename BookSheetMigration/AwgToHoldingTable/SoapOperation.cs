using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BookSheetMigration
{
    public abstract class SoapOperation<T> where T : new()
    {
        protected readonly Dictionary<string, string> actionArguments = new Dictionary<string, string>();
        protected string action = "";
        protected string pathToDataNodeFromRoot = "/*";

        public T execute()
        {
            var response = buildMessageAndReturnResponse(action, actionArguments);
            var extractedData = extractDataNode(response);
            var deserializer = new Deserializer<T>(extractedData);
            return deserializer.deserializeResponse();
        }

        private XElement buildMessageAndReturnResponse(string action, Dictionary<string, string> actionArguments)
        {
            var messageBuilder = new AWGSoapRequestMessageBuilder(action, actionArguments);
            var message = messageBuilder.buildSoapRequestMessage();
            return message.sendMessage().Result;
        }

        protected abstract void setPathToDataNodeFromRoot();

        private XElement extractDataNode(XElement response)
        {
            setPathToDataNodeFromRoot();
            return response.XPathSelectElement(pathToDataNodeFromRoot);
        }
    }
}
