﻿using BookSheetMigration.AwgToHoldingTable;
using BookSheetMigration.HoldingTableToWebInterface;
using BookSheetMigration.StringManipulation;

namespace BookSheetMigration
{
    public class SellerDealersInserterByDmvNumber : SellerDealerCollectionInserter
    {
        public SellerDealersInserterByDmvNumber(AWGTransactionDTO transaction) : base(transaction)
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
            return dmvNumberWithoutZeros.Length > Settings.minimumDmvNumberLength;
        }

        protected override DealersFinder findPotentialSellerDealers()
        {
            return new DealersFinderByDmvNumber(transaction.sellerDmvNumber);
        }
    }
}
