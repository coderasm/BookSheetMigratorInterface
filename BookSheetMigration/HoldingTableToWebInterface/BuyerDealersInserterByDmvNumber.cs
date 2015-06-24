using BookSheetMigration.AwgToHoldingTable;
using BookSheetMigration.HoldingTableToWebInterface;

namespace BookSheetMigration
{
    public class BuyerDealersInserterByDmvNumber : BuyerDealerCollectionInserter
    {
        public BuyerDealersInserterByDmvNumber(AWGTransactionDTO transaction) : base(transaction)
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
