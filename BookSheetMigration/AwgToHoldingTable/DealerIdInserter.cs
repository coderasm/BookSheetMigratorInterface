using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

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
                if (ableToSetPossibleEntityByFullName(possibleEntities))
                    return true;
                if (ableToSetPossibleEntityByPartialName(possibleEntities))
                    return true;
            }
            setIdFromFirstFoundEntity(possibleEntities[0]);
            return true;
        }

        private bool ableToSetPossibleEntityByFullName(List<DealerDTO> dealers)
        {
            DealerMatcher dealerMatcher = new DealerMatcherByFullName(dealers, this);
            return dealerMatcher.foundAndSetMatch();
        }

        private bool ableToSetPossibleEntityByPartialName(List<DealerDTO> dealers)
        {
            DealerMatcher dealerMatcher = new DealerMatcherByPartialName(dealers, this);
            return dealerMatcher.foundAndSetMatch();
        }

        public abstract string getNameInTransaction();

        public abstract string getEntityName(DealerDTO entity);

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

        public abstract void setIdFromFirstFoundEntity(DealerDTO entity);
    }
}
