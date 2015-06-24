using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class DealerCollectionInserter
    {
        protected AWGTransactionDTO transaction;

        protected DealerCollectionInserter(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        public bool insertCollectionIfFound()
        {
            if (entityArgumentsExist())
            {
                return insertCollectionIfAtLeastOneEntryFound();
            }
            return false;
        }

        protected abstract bool entityArgumentsExist();

        private bool insertCollectionIfAtLeastOneEntryFound()
        {
            var possibleCollectionOfEntities = findEntities().Result;
            if (foundAtLeastOneEntityIn(possibleCollectionOfEntities))
            {
                setPossibleCollection(possibleCollectionOfEntities);
                return true;
            }
            return false;
        }

        private async Task<List<DealerDTO>> findEntities()
        {
            var entitiesFinder = findDealers();
            return await entitiesFinder.find();
        }

        protected abstract DealersFinder findDealers();

        private bool foundAtLeastOneEntityIn(List<DealerDTO> dealers)
        {
            removeDealersWithoutContacts(dealers);
            return dealers.Count > 0;
        }

        private void removeDealersWithoutContacts(List<DealerDTO> dealers)
        {
            dealers.RemoveAll(hasNoContacts);
        }

        private bool hasNoContacts(DealerDTO dealer)
        {
            var contactFinder = new DealerContactsFinder(dealer.dealerId);
            return contactFinder.find().Result.Count == 0;
        }

        protected abstract void setPossibleCollection(List<DealerDTO> entity);
    }
}
