namespace BookSheetMigration.AwgToHoldingTable
{
    public abstract class BuyerDealerIdInserter : DealerIdInserter
    {
        protected BuyerDealerIdInserter(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool entityArgumentsExist()
        {
            return dealerSearchCriteriaExists();
        }

        protected abstract bool dealerSearchCriteriaExists();

        protected override string getEntityNumber()
        {
            return transaction.buyerDmvNumber;
        }

        public override string getNameInTransaction()
        {
            return transaction.buyerCompanyName;
        }

        protected override DealersFinder findDealers()
        {
            return findPotentialBuyerDealers();
        }

        protected abstract DealersFinder findPotentialBuyerDealers();

        public override void setDealerId(DealerDTO dealer)
        {
            transaction.buyerDealerId = dealer.dealerId;
        }

        protected override bool dmvNumberIsABS()
        {
            return transaction.buyerDmvNumber.Equals(Settings.ABSDmvNumber);
        }
    }
}
