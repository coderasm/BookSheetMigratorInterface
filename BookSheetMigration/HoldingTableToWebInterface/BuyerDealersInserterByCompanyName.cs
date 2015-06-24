namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class BuyerDealersInserterByCompanyName : BuyerDealerCollectionInserter
    {
        public BuyerDealersInserterByCompanyName(AWGTransactionDTO transaction) : base(transaction)
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
