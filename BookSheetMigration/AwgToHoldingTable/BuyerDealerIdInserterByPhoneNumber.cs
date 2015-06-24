namespace BookSheetMigration.AwgToHoldingTable
{
    public class BuyerDealerIdInserterByPhoneNumber : BuyerDealerIdInserter
    {
        public BuyerDealerIdInserterByPhoneNumber(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.buyerPhone);
        }

        protected override DealersFinder findPotentialBuyerDealers()
        {
            return new DealersFinderByPhoneNumber(transaction.buyerPhone);
        }
    }
}
