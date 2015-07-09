using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class BookSheetTransactionMigrator : DataMigrator<AWGTransactionDTO>
    {
        private const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        private DateTime lastDayAnEventIsSearchable = DateTime.Today.AddDays(-Settings.daysBeforeToday);
        private List<AWGEventDTO> liveEvents;
        private DateTime rightNow = DateTime.Now;
        private EntityDAO<AWGEventDTO> eventDao;
        private List<AWGTransactionDTO> allTransactions = new List<AWGTransactionDTO>();
        private DateTime todaysOpeningDate = DateTime.Today.AddHours(Settings.openingHourOfWorkDay);
        private DateTime todaysEndingDate = DateTime.Today.AddHours(Settings.endingHourOfWorkDay);

        public BookSheetTransactionMigrator()
        {
            eventDao = new EntityDAO<AWGEventDTO>(entityDao.database);
        }

        protected override List<AWGTransactionDTO> findPossiblyNewRecords()
        {
            liveEvents = findEventsNotExpiredBy(lastDayAnEventIsSearchable);
            return findSalesInEvents(liveEvents);
        }

        private List<AWGEventDTO> findEventsNotExpiredBy(DateTime day)
        {
            var query = "SELECT * FROM " + Settings.ABSBookSheetEventTable + " WHERE EndTime >= '" + day.ToString(dateFormat) + "'";
            return eventDao.@select(query).Result;
        }

        private List<AWGTransactionDTO> findSalesInEvents(List<AWGEventDTO> liveEvents)
        {
            var endDate = findEndDate();
            foreach (var awgEvent in liveEvents)
            {
                findTransactionsAndInsertIds(awgEvent, endDate);
            }
            return allTransactions;
        }

        private void findTransactionsAndInsertIds(AWGEventDTO awgEvent, DateTime endDate)
        {
            var transactions = findTransactionsForEvent(awgEvent, endDate);
            insertIdsIntoTransactions(transactions, awgEvent.eventId);
            allTransactions.AddRange(transactions);
        }

        private List<AWGTransactionDTO> findTransactionsForEvent(AWGEventDTO awgEvent, DateTime endDate)
        {
            var serviceClient = new AWGServiceClient();
            var startDate = findStartDate(awgEvent);
            return serviceClient.findTransactionsByStatusDateRangeAndId(TransactionStatus.New, startDate, endDate, awgEvent.eventId);
        }

        private DateTime findStartDate(AWGEventDTO awgEvent)
        {
            if (neverMigratedSalesFrom(awgEvent))
                return awgEvent.startTime;
            return determineStartDate(awgEvent);
        }

        private bool neverMigratedSalesFrom(AWGEventDTO awgEvent)
        {
            return awgEvent.lastMigrated == null;
        }

        private DateTime determineStartDate(AWGEventDTO awgEvent)
        {
            if (lastMigratedBeforeToday(awgEvent))
                return awgEvent.lastMigrated.Value;
            return todaysOpeningDate;
        }

        private bool lastMigratedBeforeToday(AWGEventDTO awgEvent)
        {
            return awgEvent.lastMigrated < todaysOpeningDate;
        }

        private DateTime findEndDate()
        {
            return todaysEndingDate > rightNow ? todaysEndingDate : rightNow;
        }

        private List<AWGTransactionDTO> insertIdsIntoTransactions(List<AWGTransactionDTO> transactions, int id)
        {
            transactions.ForEach(t =>
            {
                setEventId(id, t);
                setDealerIds(t);
                setContactIds(t);
            });
            return transactions;
        }

        private void setEventId(int id, AWGTransactionDTO t)
        {
            t.eventId = id;
        }

        private void setDealerIds(AWGTransactionDTO t)
        {
            setSellerId(t);
            setBuyerId(t);
        }



        private void setSellerId(AWGTransactionDTO t)
        {
            DealerIdInserter sellerDealerIdInserter = new SellerDealerIdInserterByDmvNumber(t);
            if (insertedDealerUsingDmvNumber(sellerDealerIdInserter))
                return;
            sellerDealerIdInserter = new SellerDealerIdInserterByPhoneNumber(t);
            if (insertedDealerUsingPhoneNumber(sellerDealerIdInserter))
                return;
            sellerDealerIdInserter = new SellerDealerIdInserterByAddressAndCity(t);
            if (insertedDealerUsingAddressAndCity(sellerDealerIdInserter))
                return;
            sellerDealerIdInserter = new SellerDealerIdInserterByCompanyName(t);
            if (insertedDealerUsingCompanyName(sellerDealerIdInserter))
                return;
        }

        private void setBuyerId(AWGTransactionDTO t)
        {
            DealerIdInserter buyerDealerIdInserter = new BuyerDealerIdInserterByDmvNumber(t);
            if (insertedDealerUsingDmvNumber(buyerDealerIdInserter))
                return;
            buyerDealerIdInserter = new BuyerDealerIdInserterByPhoneNumber(t);
            if (insertedDealerUsingPhoneNumber(buyerDealerIdInserter))
                return;
            buyerDealerIdInserter = new BuyerDealerIdInserterByAddressAndCity(t);
            if (insertedDealerUsingAddressAndCity(buyerDealerIdInserter))
                return;
            buyerDealerIdInserter = new BuyerDealerIdInserterByCompanyName(t);
            if (insertedDealerUsingCompanyName(buyerDealerIdInserter))
                return;
        }

        private bool insertedDealerUsingDmvNumber(DealerIdInserter dealerIdInserter)
        {
            return dealerIdInserter.insertIdIfFound();
        }

        private bool insertedDealerUsingPhoneNumber(DealerIdInserter dealerIdInserter)
        {
            return dealerIdInserter.insertIdIfFound();
        }

        private bool insertedDealerUsingAddressAndCity(DealerIdInserter dealerIdInserter)
        {
            return dealerIdInserter.insertIdIfFound();
        }

        private bool insertedDealerUsingCompanyName(DealerIdInserter dealerIdInserter)
        {
            return dealerIdInserter.insertIdIfFound();
        }

        private void setContactIds(AWGTransactionDTO t)
        {
            var buyerContactIdInserter = new BuyerContactIdInserter(t);
            buyerContactIdInserter.insertIdIfFound();
        }

        protected override Task doAfterMigration()
        {
            base.doAfterMigration();
            return updateLastMigratedForEvents();
        }

        private Task updateLastMigratedForEvents()
        {
            return Task.Run(async() =>
            {
                foreach (var liveEvent in liveEvents)
                {
                    await updateLastMigratedForEvent(liveEvent);
                }
            });
        }

        private Task<int> updateLastMigratedForEvent(AWGEventDTO awgEvent)
        {
            awgEvent.lastMigrated = rightNow;
            return eventDao.updateShared(awgEvent, new List<string>() { "LastMigrated" });
        }
    }
}
