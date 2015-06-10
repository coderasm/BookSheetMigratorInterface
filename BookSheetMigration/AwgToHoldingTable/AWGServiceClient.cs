using System.Collections.Generic;

namespace BookSheetMigration
{
    public class AWGServiceClient
    {
        public List<AWGEventDTO> findEventsByStatus(EventStatus eventStatus)
        {
            var listEventOperation = new ListEventOperation(eventStatus);
            return listEventOperation.execute().awgEvents;
        }

        public List<AWGInventoryDTO> findVehiclesByStatusAndId(InventoryStatus inventoryStatus, int eventId = 0, string dealerNumber = "")
        {
            var listInventoryOperation = new ListInventoryOperation(InventoryStatus.Sold, eventId, dealerNumber);
            return listInventoryOperation.execute().inventory;
        }

        public List<AWGTransactionDTO> findTransactionsByStatusAndId(TransactionStatus transactionStatus, int eventId = 0, string sellingDealerNumber = "", string buyingDealerNumber = "")
        {
            var listTransactionOperation = new ListTransactionOperation(TransactionStatus.New, eventId, sellingDealerNumber, buyingDealerNumber);
            return listTransactionOperation.execute().transactions;
        }
    }

    public enum EventStatus
    {
        AllEvents,
        Upcoming,
        InProgress,
        Ended,
        Cancelled
    }

    public enum InventoryStatus
    {
        AllItems,
        ReadyForRelease,
        InAuction,
        NoSale,
        Sold,
        Cancelled
    }

    public enum TransactionStatus
    {
        AllTransactions,
        New,
        InProgress,
        Cancelled,
        Complete,
        IfSale,
        InAbitration
    }
}
