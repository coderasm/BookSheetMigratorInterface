using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigration.Test
{
    [TestClass]
    public class SoapActionTest
    {

        private SoapAction soapAction;

        [TestInitialize]
        public void setUp()
        {
            String action = "operation";
            String xmlnamespace = "actionNamespace";
            soapAction = new SoapAction(action, xmlnamespace);
        }

        [TestMethod]
        public void WhenAddingOnePairToOperation_ThenPairCountIsOne()
        {
            var key = "akey";
            var value = "avalue";
            soapAction.addParameterPairToAction(key, value);
            Assert.AreEqual(1, soapAction.getPairCount());
        }
    }
}
