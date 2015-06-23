using System.Collections.Generic;

namespace BookSheetMigration.AwgToHoldingTable
{
    public abstract class DealerMatcher
    {
        private IEnumerable<DealerDTO> dealers;
        private DealerIdInserter dealerIdInserter;
        protected string nameInTransaction;
        protected string foundName;

        protected DealerMatcher(IEnumerable<DealerDTO> dealers, DealerIdInserter dealerIdInserter)
        {
            this.dealers = dealers;
            this.dealerIdInserter = dealerIdInserter;
            nameInTransaction = dealerIdInserter.getNameInTransaction();
        }

        public bool foundAndSetMatch()
        {
            return doMatch();
        }

        protected bool doMatch()
        {
            foreach (var dealer in dealers)
            {
                foundName = dealerIdInserter.getEntityName(dealer);
                if (doesMatch())
                {
                    dealerIdInserter.setIdFromFirstFoundEntity(dealer);
                    return true;
                }
            }
            return false;
        }

        protected bool doesMatch()
        {
            cleanNames();
            return doCompare();
        }

        private void cleanNames()
        {
            nameInTransaction = cleanString(nameInTransaction);
            foundName = cleanString(foundName);
        }

        private string cleanString(string stringToClean)
        {
            var stringCleaner = new StringCleaner(stringToClean);
            var cleanedString = stringCleaner.clean();
            return cleanedString;
        }

        protected abstract bool doCompare();
    }
}
