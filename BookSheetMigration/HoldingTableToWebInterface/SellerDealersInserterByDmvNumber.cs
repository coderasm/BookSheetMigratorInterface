using BookSheetMigration.AwgToHoldingTable;
using BookSheetMigration.HoldingTableToWebInterface;

namespace BookSheetMigration
{
    public class SellerDealersInserterByDmvNumber : SellerDealerCollectionInserter
    {
        public SellerDealersInserterByDmvNumber(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.sellerDmvNumber);
        }

        protected override DealersFinder findPotentialSellerDealers()
        {
            return new DealersFinderByDmvNumber(transaction.sellerDmvNumber);
        }
    }
}
