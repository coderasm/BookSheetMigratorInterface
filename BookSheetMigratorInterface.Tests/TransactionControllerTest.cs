using BookSheetMigratorInterface.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigratorInterface.Tests
{
    [TestClass]
    public class TransactionControllerTest
    {
        private TransactionController controller;

        [TestInitialize]
        public void setUP()
        {
            controller = new TransactionController();
        }

        [TestMethod]
        public void WhenAskingForUnimportedEvents_ANonZeroAmountIsReturned()
        {
            var unimportedTransactions = controller.findUnimportedTransactions().Result;
            Assert.AreEqual(62, unimportedTransactions.Count);
        }

        [TestMethod]
        public void WhenAskingForUnimportedEvents_IfEventsAreFoundAJsonStringIsCreated()
        {
            var eventsAsJson = controller.GetUnimported();
            Assert.AreNotEqual("[{}]", eventsAsJson);
        }
    }
}
