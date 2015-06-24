namespace BookSheetMigration.AwgToHoldingTable
{
    public class BuyerDealerIdInserterByCompanyName : BuyerDealerIdInserter
    {
        public BuyerDealerIdInserterByCompanyName(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.buyerCompanyName);
        }

        protected override DealersFinder findPotentialBuyerDealers()
        {
            return new DealersFinderByCompanyName(transaction.buyerCompanyName);
        }
    }
}
