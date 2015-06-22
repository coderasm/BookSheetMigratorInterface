using System.Threading.Tasks;
using BookSheetMigration.HoldingTableToWebInterface;
using BookSheetMigratorInterface.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

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
            var transactionDao = new TransactionDAO();
            var unimportedTransactions = transactionDao.getUnimported().Result;
            Assert.AreEqual(50, unimportedTransactions.Count);
        }

        [TestMethod]
        public void WhenAskingForUnimportedEvents_IfEventsAreFoundAJsonStringIsCreated()
        {
            var eventsAsJson = controller.GetUnimported().Result;
            Assert.AreNotEqual("[{}]", eventsAsJson);
        }

        [TestMethod]
        public async Task testingUpdateWhenPassingIdsInUrl()
        {
            var json = "{eventId: 12345, transactionId: 25687, bidAmount: 345}";
            var jsonObject = JToken.Parse(json);
            var controller = new TransactionController();
            await controller.Put(12345, 25687, jsonObject);
        }
    }
}
