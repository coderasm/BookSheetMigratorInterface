using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class ContactCollectionInserter
    {
        protected DealerDTO dealer;

        public void insertCollectionIfFound()
        {
            if (entityArgumentsExist())
            {
                var entityNumber = getEntityNumber();
                insertCollectionIfAtLeastOneEntryFound(entityNumber);
            }
        }

        protected abstract bool entityArgumentsExist();

        protected abstract string getEntityNumber();

        private void insertCollectionIfAtLeastOneEntryFound(string entityNumber)
        {
            var possibleCollectionOfEntities = findEntities(entityNumber).Result;
            if (foundAtLeastOneEntityIn(possibleCollectionOfEntities))
                setPossibleCollection(possibleCollectionOfEntities);

        }

        protected abstract Task<List<DealerContactDTO>> findEntities(string entityNumber);

        private bool foundAtLeastOneEntityIn(List<DealerContactDTO> items)
        {
            return items.Count > 0;
        }

        protected abstract void setPossibleCollection(List<DealerContactDTO> entity);
    }
}
