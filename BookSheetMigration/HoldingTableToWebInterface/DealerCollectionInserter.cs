using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class DealerCollectionInserter
    {
        protected AWGTransactionDTO transaction;

        public bool insertCollectionIfFound()
        {
            if (entityArgumentsExist())
            {
                var entityArguments = getEntityArguments();
                return insertCollectionIfAtLeastOneEntryFound(entityArguments);
            }
            return false;
        }

        protected abstract bool entityArgumentsExist();

        protected abstract object[] getEntityArguments();

        private bool insertCollectionIfAtLeastOneEntryFound(params object[] entityArguments)
        {
            var possibleCollectionOfEntities = findEntities(entityArguments).Result;
            if (foundAtLeastOneEntityIn(possibleCollectionOfEntities))
            {
                setPossibleCollection(possibleCollectionOfEntities);
                return true;
            }
            return false;
        }

        protected abstract Task<List<DealerDTO>> findEntities(params object[] entityArguments);

        private bool foundAtLeastOneEntityIn(List<DealerDTO> dealers)
        {
            if (insertingBuyersCollection())
            {
                dealers.RemoveAll(hasNoContacts);
            }
            return dealers.Count > 0;
        }

        protected abstract bool insertingBuyersCollection();

        private bool hasNoContacts(DealerDTO dealer)
        {
            var contactFinder = new DealerContactsFinder(dealer.dealerId);
            return contactFinder.find().Result.Count == 0;
        }

        protected abstract void setPossibleCollection(List<DealerDTO> entity);
    }
}
