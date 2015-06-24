using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public abstract class DealerIdInserter
    {
        protected AWGTransactionDTO transaction;

        protected DealerIdInserter(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        public bool insertIdIfFound()
        {
            if (entityArgumentsExist())
            {
                return insertIdIfAtLeastOneFound();
            }
            return false;
        }

        protected abstract bool entityArgumentsExist();

        private bool insertIdIfAtLeastOneFound()
        {
            var possibleEntities = findEntities().Result;
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
                if (ableToSetPossibleEntityAsABS())
                    return true;
                if (ableToSetPossibleEntityByFullName(possibleEntities))
                    return true;
                if (ableToSetPossibleEntityByPartialName(possibleEntities))
                    return true;
            }
            setIdFromFirstFoundEntity(possibleEntities[0]);
            return true;
        }

        private bool ableToSetPossibleEntityAsABS()
        {
            throw new NotImplementedException();
        }

        protected abstract string getEntityNumber();

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

        public string getEntityName(DealerDTO dealer)
        {
            return dealer.companyName;
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

        private bool foundMorethanOneEntityIn(List<DealerDTO> possibleEntities)
        {
            return possibleEntities.Count > 1;
        }

        public abstract void setIdFromFirstFoundEntity(DealerDTO dealer);
    }
}
