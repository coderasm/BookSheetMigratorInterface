namespace BookSheetMigration.AwgToHoldingTable
{
    public class SellerDealerIdInserterByCompanyName : SellerDealerIdInserter
    {
        public SellerDealerIdInserterByCompanyName(AWGTransactionDTO transaction) : base(transaction)
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
