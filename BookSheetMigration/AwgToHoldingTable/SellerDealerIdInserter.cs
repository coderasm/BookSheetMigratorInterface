namespace BookSheetMigration.AwgToHoldingTable
{
    public abstract class SellerDealerIdInserter : DealerIdInserter
    {
        public SellerDealerIdInserter(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool entityArgumentsExist()
        {
            return dealerSearchCriteriaExists();
        }

        protected abstract bool dealerSearchCriteriaExists();

        protected override string getEntityNumber()
        {
            return transaction.sellerDmvNumber;
        }

        public override string getNameInTransaction()
        {
            return transaction.sellerCompanyName;
        }

        protected override DealersFinder findDealers()
        {
            return findPotentialSellerDealers();
        }

        protected abstract DealersFinder findPotentialSellerDealers();

        public override void setDealerId(DealerDTO dealer)
        {
            transaction.sellerDealerId = dealer.dealerId;
        }

        protected override bool dmvNumberIsABS()
        {
            return transaction.sellerDmvNumber.Equals(Settings.ABSDmvNumber);
        }
    }
}
