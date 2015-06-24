using System.Collections.Generic;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public abstract class SellerDealerCollectionInserter : DealerCollectionInserter
    {
        protected SellerDealerCollectionInserter(AWGTransactionDTO transaction) : base(transaction)
        {
        }

        protected override bool entityArgumentsExist()
        {
            return dealerSearchCriteriaExists();
        }

        protected abstract bool dealerSearchCriteriaExists();

        protected override DealersFinder findDealers()
        {
            return findPotentialSellerDealers();
        }

        protected abstract DealersFinder findPotentialSellerDealers();

        protected override void setPossibleCollection(List<DealerDTO> entity)
        {
            transaction.sellers = entity;
        }
    }
}
