using System.Collections.Generic;

namespace BookSheetMigration.AwgToHoldingTable
{
    public class TransactionIdsInserter
    {
        public List<AWGTransactionDTO> insertIdsIntoTransactions(List<AWGTransactionDTO> transactions, int id)
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
    }
}