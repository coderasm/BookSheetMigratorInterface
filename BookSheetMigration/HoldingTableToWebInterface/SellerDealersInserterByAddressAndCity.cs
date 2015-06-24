namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class SellerDealersInserterByAddressAndCity : SellerDealerCollectionInserter
    {
        public SellerDealersInserterByAddressAndCity(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.sellerAddress) && !string.IsNullOrEmpty(transaction.sellerCity);
        }

        protected override DealersFinder findPotentialSellerDealers()
        {
            return new DealersFinderByAddressAndCity(transaction.sellerAddress, transaction.sellerCity);
        }
    }
}
