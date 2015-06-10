using System;
using System.Xml;

namespace BookSheetMigration
{
    public class SoapXMLGenerator
    {
        private const string envelopeTag = "Envelope";
        private const string bodyTag = "Body";
        private SoapAction soapAction;
        private XmlDocument xmlDocument = new XmlDocument();
        private const string rootNamespace = "http://schemas.xmlsoap.org/soap/envelope/";

        public SoapXMLGenerator(SoapAction soapAction)
        {
            this.soapAction = soapAction;
            setXmlDocumentDeclaration();
        }

        private void setXmlDocumentDeclaration()
        {
            var declaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
            xmlDocument.AppendChild(declaration);
        }

        public XmlDocument generateSoapXmlDocument()
        {
            return generateXMLDocument();
        }

        private XmlDocument generateXMLDocument()
        {
            generateAndPopulateEnvelopeNode(xmlDocument);
            return xmlDocument;
        }

        private void generateAndPopulateEnvelopeNode(XmlDocument xmlDocument)
        {
            var envelopeNode = xmlDocument.CreateElement(envelopeTag, rootNamespace);
            generateAndPopulateBodyNode(envelopeNode);
            xmlDocument.AppendChild(envelopeNode);
        }

        public void generateAndPopulateBodyNode(XmlNode envelopeNode)
        {

            var bodyNode = xmlDocument.CreateElement(bodyTag, rootNamespace);
            generateAndPopulateActionNode(bodyNode);
            envelopeNode.AppendChild(bodyNode);
        } 

        private void generateAndPopulateActionNode(XmlNode bodyNode)
        {
            var actionNode = xmlDocument.CreateElement(soapAction.Operation, soapAction.actionNamespace);
            generateAndAttachParameterNodes(actionNode);
            bodyNode.AppendChild(actionNode);
        }

        private void generateAndAttachParameterNodes(XmlNode actionNode)
        {
            foreach (var parameter in soapAction.parameters)
            {
                var parameterNode = xmlDocument.CreateElement(parameter.Key, soapAction.actionNamespace);
                parameterNode.InnerText = parameter.Value;
                actionNode.AppendChild(parameterNode);
            }
        }
    }
}
