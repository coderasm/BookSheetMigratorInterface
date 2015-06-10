using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class CollectionInserter<T>
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
                if (foundMorethanOneEntityIn(possibleCollectionOfEntities))
                {
                    setPossibleCollection(possibleCollectionOfEntities);
                    return true;
                }
                if (insertingBuyersCollection())
                {
                    if (!hasAtLeastOneContact(possibleCollectionOfEntities))
                        return false;
                }
                setPossibleCollection(possibleCollectionOfEntities);
                return true;
            }
            return false;
        }

        protected abstract Task<List<T>> findEntities(params object[] entityArguments);

        private bool foundAtLeastOneEntityIn(List<T> items)
        {
            return items.Count > 0;
        }

        private bool foundMorethanOneEntityIn(List<T> possibleCollectionOfEntities)
        {
            return possibleCollectionOfEntities.Count > 1;
        }

        protected abstract void setPossibleCollection(List<T> entity);

        protected abstract bool insertingBuyersCollection();

        protected abstract bool hasAtLeastOneContact(List<T> possibleCollectionOfEntities);
    }
}
