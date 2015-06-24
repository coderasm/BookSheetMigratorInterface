using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class SellerDealerIdInserterByDmvNumber : SellerDealerIdInserter
    {
        public SellerDealerIdInserterByDmvNumber(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.sellerDmvNumber);
        }

        protected override DealersFinder findPotentialSellerDealers()
        {
            return new DealersFinderByDmvNumber(transaction.sellerDmvNumber);
        }
    }
}