using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class SellerDealersInserterByPhoneNumber : SellerDealerCollectionInserter
    {
        public SellerDealersInserterByPhoneNumber(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.sellerPhone);
        }

        protected override DealersFinder findPotentialSellerDealers()
        {
            return new DealersFinderByPhoneNumber(transaction.sellerPhone);
        }
    }
}
