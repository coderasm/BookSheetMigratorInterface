using System.Collections.Generic;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class TransactionCollectionLoader
    {
        private List<AWGTransactionDTO> transactions;

        public TransactionCollectionLoader(List<AWGTransactionDTO> transactions)
        {
            this.transactions = transactions;
        }

        public List<AWGTransactionDTO> loadDependentCollections()
        {
            findAndAttachSellingDealersForTransactions(transactions);
            findAndAttachBuyingDealersForTransactions(transactions);
            findAndAttachAllBuyingDealersContactsForTransactions(transactions);
            return transactions;
        }

        private void findAndAttachSellingDealersForTransactions(List<AWGTransactionDTO> transactions)
        {
            transactions.ForEach(setPossibleSellerDealers);
        }

        private void findAndAttachBuyingDealersForTransactions(List<AWGTransactionDTO> transactions)
        {
            transactions.ForEach(setPossibleBuyerDealers);
        }

        private void findAndAttachAllBuyingDealersContactsForTransactions(List<AWGTransactionDTO> transactions)
        {
            transactions.ForEach((t) =>
            {
                if(buyersWereFound(t))
                {
                    findAndAttachContacts(t);
                }
            });
        }

        private void setPossibleSellerDealers(AWGTransactionDTO t)
        {
            DealerCollectionInserter sellerDealersInserter = new SellerDealersInserterByDmvNumber(t);
            if (insertedDealersUsingDmvNumber(sellerDealersInserter))
                return;
            sellerDealersInserter = new SellerDealersInserterByPhoneNumber(t);
            if (insertedDealersUsingPhoneNumber(sellerDealersInserter))
                return;
            sellerDealersInserter = new SellerDealersInserterByAddressAndCity(t);
            if (insertedDealersUsingAddressAndCity(sellerDealersInserter))
                return;
            sellerDealersInserter = new SellerDealersInserterByCompanyName(t);
            if (insertedDealersUsingCompanyName(sellerDealersInserter))
                return;
        }

        private void setPossibleBuyerDealers(AWGTransactionDTO t)
        {
            DealerCollectionInserter buyerDealersInserter = new BuyerDealersInserterByDmvNumber(t);
            if (insertedDealersUsingDmvNumber(buyerDealersInserter))
                return;
            buyerDealersInserter = new BuyerDealersInserterByPhoneNumber(t);
            if (insertedDealersUsingPhoneNumber(buyerDealersInserter))
                return;
            buyerDealersInserter = new BuyerDealersInserterByAddressAndCity(t);
            if (insertedDealersUsingAddressAndCity(buyerDealersInserter))
                return;
            buyerDealersInserter = new BuyerDealersInserterByCompanyName(t);
            if (insertedDealersUsingCompanyName(buyerDealersInserter))
                return;
        }

        private bool insertedDealersUsingDmvNumber(DealerCollectionInserter dealersInserter)
        {
            return dealersInserter.insertCollectionIfFound();
        }

        private bool insertedDealersUsingPhoneNumber(DealerCollectionInserter dealersInserter)
        {
            return dealersInserter.insertCollectionIfFound();
        }

        private bool insertedDealersUsingAddressAndCity(DealerCollectionInserter dealersInserter)
        {
            return dealersInserter.insertCollectionIfFound();
        }

        private bool insertedDealersUsingCompanyName(DealerCollectionInserter dealersInserter)
        {
            return dealersInserter.insertCollectionIfFound();
        }

        private bool buyersWereFound(AWGTransactionDTO transaction)
        {
            return transaction.buyers != null;
        }

        private void findAndAttachContacts(AWGTransactionDTO t)
        {
            t.buyers.ForEach(b =>
            {
                var buyerContactsInserter = new BuyerContactsInserter(b);
                buyerContactsInserter.insertCollectionIfFound();
            });
        }
    }
}