using System;
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

        public List<AWGTransactionDTO> findTransactionsByStatus(TransactionStatus transactionStatus)
        {
            var listTransactionOperation = new ListTransactionOperation(TransactionStatus.New);
            return listTransactionOperation.execute().transactions;
        }

        public List<AWGTransactionDTO> findTransactionsByStatusAndId(TransactionStatus transactionStatus, int eventId)
        {
            var listTransactionOperation = new ListTransactionOperation(TransactionStatus.New, eventId);
            return listTransactionOperation.execute().transactions;
        }

        public List<AWGTransactionDTO> findTransactionsByStatusDateRangeAndId(TransactionStatus transactionStatus, DateTime startDateTime, DateTime endDateTime, int eventId)
        {
            var listTransactionOperation = new ListTransactionOperation(TransactionStatus.New, startDateTime, endDateTime, eventId);
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
