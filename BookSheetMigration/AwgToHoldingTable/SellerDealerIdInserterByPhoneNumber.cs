namespace BookSheetMigration.AwgToHoldingTable
{
    public class SellerDealerIdInserterByPhoneNumber : SellerDealerIdInserter
    {
        public SellerDealerIdInserterByPhoneNumber(AWGTransactionDTO transaction) : base(transaction)
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
