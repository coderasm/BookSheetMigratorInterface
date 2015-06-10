using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigration.Test
{
    [TestClass]
    public class SoapRequestMessageBuilderTest
    {
        [TestMethod]
        public void WhenARequestIsBuilt_ThenARequestInstanceIsReturned()
        {
            var action = "myaction";
            var actionArguments = new Dictionary<string, string>()
            {
                {"eventStatus", "Upcoming"}
            };
            var soapRequestMessageBuilder = new AWGSoapRequestMessageBuilder(action, actionArguments);
            SoapRequestMessage soapRequestMessage = soapRequestMessageBuilder.buildSoapRequestMessage();
            Assert.IsNotNull(soapRequestMessage);
        }
    }
}
