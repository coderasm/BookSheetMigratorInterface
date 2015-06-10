using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class IdInserter<T>
    {
        protected AWGTransactionDTO transaction;

        public bool insertIdIfFound()
        {
            if (entityArgumentsExist())
            {
                var entityArguments = getEntityArguments();
                return insertIdIfAtLeastOneFound(entityArguments);
            }
            return false;
        }

        protected abstract bool entityArgumentsExist();

        protected abstract object[] getEntityArguments();

        private bool insertIdIfAtLeastOneFound(params object[] entityArguments)
        {
            var possibleEntities = findEntities(entityArguments).Result;
            if (foundAtLeastOneEntityIn(possibleEntities))
            {
                if (foundMorethanOneEntityIn(possibleEntities))
                {
                    if (ableToSetPossibleEntityByName(possibleEntities))
                        return true;
                }
                else
                {
                    if (insertingBuyerDealerId())
                    {
                        if (!hasAtLeastOneContact(possibleEntities))
                            return false;
                    }
                }
                setIdFromFirstFoundEntity(possibleEntities[0]);
                return true;
            }
            return false;
        }

        private bool ableToSetPossibleEntityByName(List<T> possibleEntities)
        {
            var nameIntransaction = removePunctuationAndToLower(getNameInTransaction());
            foreach (var possibleEntity in possibleEntities)
            {
                var foundName = getEntityName(possibleEntity);
                if (foundName != null)
                {
                    var cleanFoundName = removePunctuationAndToLower(foundName);
                    if (cleanFoundName.Equals(nameIntransaction))
                    {
                        setIdFromFirstFoundEntity(possibleEntity);
                        return true;
                    }
                }
            }
            return false;
        }

        protected abstract string getNameInTransaction();

        protected abstract string getEntityName(T entity);

        private static string removePunctuationAndToLower(string name)
        {
            name = Regex.Replace(name, "[^\\w\\s]", "");
            name = name.ToLower().Trim();
            return name;
        }

        protected abstract bool insertingBuyerDealerId();

        protected abstract bool hasAtLeastOneContact(List<T> possibleEntities);

        protected abstract Task<List<T>> findEntities(params object[] entityArguments);

        private bool foundAtLeastOneEntityIn(List<T> items)
        {
            return items.Count > 0;
        }

        private bool foundMorethanOneEntityIn(List<T> possibleEntities)
        {
            return possibleEntities.Count > 1;
        }

        protected abstract void setIdFromFirstFoundEntity(T entity);
    }
}
