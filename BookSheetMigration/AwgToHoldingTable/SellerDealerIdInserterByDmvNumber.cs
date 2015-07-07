using BookSheetMigration.AwgToHoldingTable;
using BookSheetMigration.StringManipulation;

namespace BookSheetMigration
{
    public class SellerDealerIdInserterByDmvNumber : SellerDealerIdInserter
    {
        public SellerDealerIdInserterByDmvNumber(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.sellerDmvNumber) && isGreaterThanTwoCharacters(transaction.sellerDmvNumber);
        }

        private bool isGreaterThanTwoCharacters(string dmvNumber)
        {
            var leadingZeroRemover = new LeadingZeroRemover(dmvNumber);
            var dmvNumberWithoutZeros = leadingZeroRemover.remove();
            return dmvNumberWithoutZeros.Length > 2;
        }

        protected override DealersFinder findPotentialSellerDealers()
        {
            return new DealersFinderByDmvNumber(transaction.sellerDmvNumber);
        }
    }
}