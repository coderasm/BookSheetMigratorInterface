using BookSheetMigration.AwgToHoldingTable;
using BookSheetMigration.HoldingTableToWebInterface;
using BookSheetMigration.StringManipulation;

namespace BookSheetMigration
{
    public class BuyerDealersInserterByDmvNumber : BuyerDealerCollectionInserter
    {
        public BuyerDealersInserterByDmvNumber(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.buyerDmvNumber) && isGreaterThanTwoCharacters(transaction.buyerDmvNumber);
        }

        private bool isGreaterThanTwoCharacters(string dmvNumber)
        {
            var leadingZeroRemover = new LeadingZeroRemover(dmvNumber);
            var dmvNumberWithoutZeros = leadingZeroRemover.remove();
            return dmvNumberWithoutZeros.Length > 2;
        }

        protected override DealersFinder findPotentialBuyerDealers()
        {
            return new DealersFinderByDmvNumber(transaction.buyerDmvNumber);
        }
    }
}
