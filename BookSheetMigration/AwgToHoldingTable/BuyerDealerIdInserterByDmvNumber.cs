using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class BuyerDealerIdInserterByDmvNumber : BuyerDealerIdInserter
    {
        public BuyerDealerIdInserterByDmvNumber(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.buyerDmvNumber);
        }

        protected override DealersFinder findPotentialBuyerDealers()
        {
            return new DealersFinderByDmvNumber(transaction.buyerDmvNumber);
        }
    }
}