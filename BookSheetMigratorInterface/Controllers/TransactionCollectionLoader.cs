using System.Collections.Generic;
using System.Diagnostics;
using BookSheetMigration;
using BookSheetMigration.HoldingTableToWebInterface;

namespace BookSheetMigratorInterface.Controllers
{
    public class TransactionCollectionLoader
    {
        private List<AWGTransactionDTO> transactions;

        public TransactionCollectionLoader(List<AWGTransactionDTO> transactions)
        {
            this.transactions = transactions;
        }

        public void loadDependentCollections()
        {
            findAndAttachSellingDealersForTransactions(transactions);
            findAndAttachBuyingDealersForTransactions(transactions);
            findAndAttachAllBuyingDealersContactsForTransactions(transactions);
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
            CollectionInserter<DealerDTO> sellerDealersInserter = new SellerDealersInserterByDmvNumber(t);
            if (insertedDealersUsingDmvNumber(sellerDealersInserter))
                return;
            sellerDealersInserter = new SellerDealersInserterByPhoneNumber(t);
            if (insertedDealersUsingPhoneNumber(sellerDealersInserter))
                return;
            sellerDealersInserter = new SellerDealersInserterByAddressAndCity(t);
            if (insertedDealersUsingAddressAndCity(sellerDealersInserter))
                return;
        }

        private void setPossibleBuyerDealers(AWGTransactionDTO t)
        {
            CollectionInserter<DealerDTO> buyerDealersInserter = new BuyerDealersInserterByDmvNumber(t);
            if (insertedDealersUsingDmvNumber(buyerDealersInserter))
                return;
            buyerDealersInserter = new BuyerDealersInserterByPhoneNumber(t);
            if (insertedDealersUsingPhoneNumber(buyerDealersInserter))
                return;
            buyerDealersInserter = new BuyerDealersInserterByAddressAndCity(t);
            if (insertedDealersUsingAddressAndCity(buyerDealersInserter))
                return;
        }

        private bool insertedDealersUsingDmvNumber(CollectionInserter<DealerDTO> sellerDealersInserter)
        {
            return sellerDealersInserter.insertCollectionIfFound();
        }

        private bool insertedDealersUsingPhoneNumber(CollectionInserter<DealerDTO> sellerDealersInserter)
        {
            return sellerDealersInserter.insertCollectionIfFound();
        }

        private bool insertedDealersUsingAddressAndCity(CollectionInserter<DealerDTO> sellerDealersInserter)
        {
            return sellerDealersInserter.insertCollectionIfFound();
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