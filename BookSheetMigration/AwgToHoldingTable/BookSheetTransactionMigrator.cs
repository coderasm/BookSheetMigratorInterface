using System;
using System.Collections.Generic;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class BookSheetTransactionMigrator : DataMigrator<AWGTransactionDTO>
    {
        private const string dateFormat = "yyyy-MM-dd HH:mm:ss";
        private DateTime yesterday = DateTime.Now.AddDays(-1);

        protected override List<AWGTransactionDTO> findPossiblyNewRecords()
        {
            var liveEvents = findEventsNotExpiredBy(yesterday);
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
            var serviceClient = new AWGServiceClient();
            var allTransactions = new List<AWGTransactionDTO>();
            foreach (var awgEvent in liveEvents)
            {
                var transactions = serviceClient.findTransactionsByStatusAndId(TransactionStatus.New, awgEvent.eventId);
                insertIdsIntoTransactions(transactions, awgEvent.eventId);
                allTransactions.AddRange(transactions);
            }
            return allTransactions;
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
            IdInserter<DealerDTO> sellerDealerIdInserter = new SellerDealerIdInserterByDmvNumber(t);
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
            IdInserter<DealerDTO> buyerDealerIdInserter = new BuyerDealerIdInserterByDmvNumber(t);
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

        private bool insertedDealerUsingDmvNumber(IdInserter<DealerDTO> dealerIdInserter)
        {
            return dealerIdInserter.insertIdIfFound();
        }

        private bool insertedDealerUsingPhoneNumber(IdInserter<DealerDTO> dealerIdInserter)
        {
            return dealerIdInserter.insertIdIfFound();
        }

        private bool insertedDealerUsingAddressAndCity(IdInserter<DealerDTO> dealerIdInserter)
        {
            return dealerIdInserter.insertIdIfFound();
        }

        private bool insertedDealerUsingCompanyName(IdInserter<DealerDTO> dealerIdInserter)
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
