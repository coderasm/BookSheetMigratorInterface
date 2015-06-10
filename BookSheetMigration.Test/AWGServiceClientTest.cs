using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigration.Test
{
    [TestClass]
    public class AWGServiceClientTest
    {
        [TestMethod]
        public void WhenAskingForEventsByStatus_MatchingEventsAreReturned()
        {
            var client = new AWGServiceClient();
            var events = client.findEventsByStatus(EventStatus.InProgress);
            Assert.AreEqual(1, events.Count);
        }

        [TestMethod]
        public void WhenAskingForInventoryByEventIdAndStatus_MatchingInventoryAreReturned()
        {
            var client = new AWGServiceClient();
            var inventory = client.findVehiclesByStatusAndId(InventoryStatus.Sold, 123191);
            Assert.AreEqual(26, inventory.Count);
        }

        [TestMethod]
        public void WhenAskingForTransactionsByEventIdAndStatus_MatchingTransactionsAreReturned()
        {
            var client = new AWGServiceClient();
            var transactions = client.findTransactionsByStatusAndId(TransactionStatus.New, 122972);
            Assert.AreEqual(46, transactions.Count);
        }
    }
}
