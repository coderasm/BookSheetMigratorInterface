using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class DealerIdInserter
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
                return InsertId(possibleEntities);
            }
            return false;
        }

        private bool InsertId(List<DealerDTO> possibleEntities)
        {
            if (foundMorethanOneEntityIn(possibleEntities))
            {
                if (ableToSetPossibleEntityByName(possibleEntities))
                    return true;
            }
            setIdFromFirstFoundEntity(possibleEntities[0]);
            return true;
        }

        private bool ableToSetPossibleEntityByName(List<DealerDTO> dealers)
        {
            var nameIntransaction = removePunctuationAndToLower(getNameInTransaction());
            foreach (var dealer in dealers)
            {
                var foundName = getEntityName(dealer);
                var cleanFoundName = removePunctuationAndToLower(foundName);
                if (cleanFoundName.Equals(nameIntransaction))
                {
                    setIdFromFirstFoundEntity(dealer);
                    return true;
                }
            }
            return false;
        }

        protected abstract string getNameInTransaction();

        protected abstract string getEntityName(DealerDTO entity);

        private static string removePunctuationAndToLower(string name)
        {
            name = Regex.Replace(name, "[^\\w\\s]", "");
            name = name.ToLower().Trim();
            return name;
        }

        protected abstract Task<List<DealerDTO>> findEntities(params object[] entityArguments);

        private bool foundAtLeastOneEntityIn(List<DealerDTO> dealers)
        {
            if (insertingBuyerDealerId())
            {
                dealers.RemoveAll(hasNoContacts);
            }
            return dealers.Count > 0;
        }

        protected abstract bool insertingBuyerDealerId();

        private bool hasNoContacts(DealerDTO dealer)
        {
            var contactFinder = new DealerContactsFinder(dealer.dealerId);
            return contactFinder.find().Result.Count == 0;
        }

        private bool foundMorethanOneEntityIn(List<DealerDTO> possibleEntities)
        {
            return possibleEntities.Count > 1;
        }

        protected abstract void setIdFromFirstFoundEntity(DealerDTO entity);
    }
}
