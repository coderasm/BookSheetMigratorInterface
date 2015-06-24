using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class BuyerDealersInserterByPhoneNumber : BuyerDealerCollectionInserter
    {
        public BuyerDealersInserterByPhoneNumber(AWGTransactionDTO transaction) : base(transaction)
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
