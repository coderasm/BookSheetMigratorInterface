using System.Collections.Generic;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public abstract class BuyerDealerCollectionInserter : DealerCollectionInserter
    {
        protected BuyerDealerCollectionInserter(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool entityArgumentsExist()
        {
            return dealerSearchCriteriaExists();
        }

        protected abstract bool dealerSearchCriteriaExists();

        protected override DealersFinder findDealers()
        {
            return findPotentialBuyerDealers();
        }

        protected abstract DealersFinder findPotentialBuyerDealers();

        protected override void setPossibleCollection(List<DealerDTO> entity)
        {
            transaction.buyers = entity;
        }
    }
}
