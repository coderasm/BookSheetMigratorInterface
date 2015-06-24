namespace BookSheetMigration.AwgToHoldingTable
{
    public class SellerDealerIdInserterByAddressAndCity : SellerDealerIdInserter
    {
        public SellerDealerIdInserterByAddressAndCity(AWGTransactionDTO transaction) : base(transaction)
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
