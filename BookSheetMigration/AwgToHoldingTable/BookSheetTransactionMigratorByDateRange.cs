using System;
using System.Collections.Generic;

namespace BookSheetMigration.AwgToHoldingTable
{
    public class BookSheetTransactionMigratorByDateRange : DataMigrator<AWGTransactionDTO>
    {
        private DateTime beginSoldDate;
        private DateTime endSoldDate;
        private List<AWGEventDTO> liveEvents;
        private const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        private EntityDAO<AWGEventDTO> eventDao;
        private List<AWGTransactionDTO> allTransactions = new List<AWGTransactionDTO>();
        private readonly TransactionIdsInserter transactionIdsInserter;

        public BookSheetTransactionMigratorByDateRange(DateTime beginSoldDate, DateTime endSoldDate, TransactionIdsInserter transactionIdsInserter)
        {
            this.beginSoldDate = beginSoldDate;
            this.endSoldDate = endSoldDate;
            eventDao = new EntityDAO<AWGEventDTO>(entityDao.database);
            this.transactionIdsInserter = transactionIdsInserter;
        }
        protected override List<AWGTransactionDTO> findPossiblyNewRecords()
        {
            liveEvents = findEventsInDateRange();
            return findSalesInEvents(liveEvents);
        }

        private List<AWGEventDTO> findEventsInDateRange()
        {
            var query = "SELECT * FROM " + Settings.ABSBookSheetEventTable + " WHERE NOT (EndTime < '" + beginSoldDate.ToString(dateFormat) + "' OR StartTime > '" + endSoldDate.ToString(dateFormat);
            return eventDao.@select(query).Result;
        }

        private List<AWGTransactionDTO> findSalesInEvents(IEnumerable<AWGEventDTO> liveEvents)
        {
            foreach (var awgEvent in liveEvents)
            {
                findTransactionsAndInsertIds(awgEvent);
            }
            return allTransactions;
        }

        private void findTransactionsAndInsertIds(AWGEventDTO awgEvent)
        {
            var transactions = findTransactionsForEvent(awgEvent);
            transactionIdsInserter.insertIdsIntoTransactions(transactions, awgEvent.eventId);
            allTransactions.AddRange(transactions);
        }

        private List<AWGTransactionDTO> findTransactionsForEvent(AWGEventDTO awgEvent)
        {
            var serviceClient = new AWGServiceClient();
            return serviceClient.findTransactionsByStatusDateRangeAndId(TransactionStatus.New, beginSoldDate, endSoldDate, awgEvent.eventId);
        }
    }
}
