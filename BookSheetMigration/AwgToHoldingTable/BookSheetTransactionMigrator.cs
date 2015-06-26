using System;
using System.Collections.Generic;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class BookSheetTransactionMigrator : DataMigrator<AWGTransactionDTO>
    {
        private const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        private DateTime lastDayAnEventIsSearchable = DateTime.Now.AddDays(-Settings.daysBeforeToday);

        protected override List<AWGTransactionDTO> findPossiblyNewRecords()
        {
            var liveEvents = findEventsNotExpiredBy(lastDayAnEventIsSearchable);
            return findSalesInEvents(liveEvents);
        }

        private List<AWGEventDTO> findEventsNotExpiredBy(DateTime day)
        {
            var eventDao = createEventDao();
            var query = "SELECT * FROM " + Settings.ABSBookSheetEventTable + " WHERE EndTime >= '" + day.ToString(dateFormat) + "'";
            return eventDao.@select(query).Result;
        }

        private List<AWGTransactionDTO> findSalesInEvents(List<AWGEventDTO> liveEvents)
        {
            var allTransactions = new List<AWGTransactionDTO>();
            foreach (var awgEvent in liveEvents)
            {
                var transactions = findTransactionsForEvent(awgEvent);
                insertIdsIntoTransactions(transactions, awgEvent.eventId);
                allTransactions.AddRange(transactions);
            }
            return allTransactions;
        }

        private List<AWGTransactionDTO> findTransactionsForEvent(AWGEventDTO awgEvent)
        {
            var serviceClient = new AWGServiceClient();
            var startDate = findStartDate(awgEvent);
            var now = DateTime.Now;
            var endDate = findEndDate(now);
            updateLastMigratedForEvent(awgEvent, now);
            return serviceClient.findTransactionsByStatusDateRangeAndId(TransactionStatus.New, startDate, endDate, awgEvent.eventId);
        }

        private DateTime findStartDate(AWGEventDTO awgEvent)
        {
            return awgEvent.lastMigrated == null ? awgEvent.startTime : awgEvent.lastMigrated.Value;
        }

        private DateTime findEndDate(DateTime now)
        {
            var endHour = Settings.migrationRangeEndHour;
            var hourDifference = endHour - now.Hour;
            return isAfterOrEqualToLastHour(hourDifference) ? now : now.AddHours(hourDifference);
        }

        private bool isAfterOrEqualToLastHour(int hourDifference)
        {
            return hourDifference <= 0;
        }

        private async void updateLastMigratedForEvent(AWGEventDTO awgEvent, DateTime now)
        {
            var eventDao = createEventDao();
            awgEvent.lastMigrated = now;
            await eventDao.update(awgEvent, new List<string>() { "LastMigrated" });
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

        private EntityDAO<AWGEventDTO> createEventDao()
        {
            return new EntityDAO<AWGEventDTO>();
        }
    }
}
