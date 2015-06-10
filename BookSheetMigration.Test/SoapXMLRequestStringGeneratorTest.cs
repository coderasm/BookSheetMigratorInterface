using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace BookSheetMigration.Test
{
    [TestClass]
    public class SoapXMLRequestStringGeneratorTest
    {
        private string soapPayload;
        private const string testPayload= @"<?xml version=""1.0"" encoding=""utf-8""?>
                                            <Envelope xmlns=""http://schemas.xmlsoap.org/soap/envelope/"">
                                                <Body>
                                                    <myaction xmlns=""http://mynamespace.com"">
                                                        <clientPassword>mypassword</clientPassword>
                                                        <clientUsername>myusername</clientUsername>
                                                        <securityToken>mysecuritytoken</securityToken>
                                                        <eventStatus>mystatus</eventStatus>
                                                    </myaction>
                                                 </Body>
                                            </Envelope>";

        private string cleanedPayload = Regex.Replace(testPayload, @"([>])(\s+)([<])", "$1$3");

        [TestInitialize]
        public void setUp()
        {
            var soapAction = new SoapAction("myaction", "http://mynamespace.com");
            soapAction.addParameterPairToAction("clientPassword", "mypassword");
            soapAction.addParameterPairToAction("clientUsername", "myusername");
            soapAction.addParameterPairToAction("securityToken", "mysecuritytoken");
            soapAction.addParameterPairToAction("eventStatus", "mystatus");
            var soapXmlGenerator = new SoapXMLGenerator(soapAction);
            soapPayload = soapXmlGenerator.generateSoapXmlDocument().OuterXml;
        }

        [TestMethod]
        public void WhenGivenASoapAction_AnXMLPayloadIsBuilt()
        {
            Assert.AreEqual(cleanedPayload, soapPayload);
        }
    }
}
