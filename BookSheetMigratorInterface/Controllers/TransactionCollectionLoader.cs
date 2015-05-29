using System.Collections.Generic;
using BookSheetMigration;

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

        private void findAndAttachBuyingDealersForTransactions(List<AWGTransactionDTO> transactions)
        {
            transactions.ForEach((t) =>
            {
                var buyerDealersInserter = new BuyerDealersInserter(t);
                buyerDealersInserter.insertCollectionIfFound();
            });
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

        private bool buyersWereFound(AWGTransactionDTO transaction)
        {
            return transaction.buyers != null;
        }

        private void findAndAttachContacts(AWGTransactionDTO t)
        {
            t.buyers.ForEach((b) =>
            {
                var buyerContactsInserter = new BuyerContactsInserter(b);
                buyerContactsInserter.insertCollectionIfFound();
            });
        }

        private void findAndAttachSellingDealersForTransactions(List<AWGTransactionDTO> transactions)
        {
            transactions.ForEach((t) =>
            {
                var sellerDealersInserter = new SellerDealersInserter(t);
                sellerDealersInserter.insertCollectionIfFound();
            });
        }
    }
}