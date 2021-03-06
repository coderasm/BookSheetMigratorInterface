﻿namespace BookSheetMigration.AwgToHoldingTable
{
    public class BuyerDealerIdInserterByAddressAndCity : BuyerDealerIdInserter
    {
        public BuyerDealerIdInserterByAddressAndCity(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool dealerSearchCriteriaExists()
        {
            return !string.IsNullOrEmpty(transaction.buyerAddress) && !string.IsNullOrEmpty(transaction.buyerCity);
        }

        protected override DealersFinder findPotentialBuyerDealers()
        {
            return new DealersFinderByAddressAndCity(transaction.buyerAddress, transaction.buyerCity);
        }
    }
}
