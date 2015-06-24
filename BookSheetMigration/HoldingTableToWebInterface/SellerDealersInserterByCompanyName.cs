namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class SellerDealersInserterByCompanyName : SellerDealerCollectionInserter
    {
        public SellerDealersInserterByCompanyName(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.sellerCompanyName);
        }

        protected override DealersFinder findPotentialSellerDealers()
        {
            return new DealersFinderByCompanyName(transaction.sellerCompanyName);
        }
    }
}
