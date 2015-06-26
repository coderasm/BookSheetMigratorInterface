using System;
using System.Collections.Generic;

namespace BookSheetMigration.AwgToHoldingTable
{
    public class DealerMatcherByPartialName : DealerMatcher
    {
        public DealerMatcherByPartialName(IEnumerable<DealerDTO> dealers, DealerIdInserter dealerIdInserter) : base(dealers, dealerIdInserter)
        {
        }

        protected override bool doCompare()
        {
            if (!bothNamesHaveAtLeastTwoWords())
                return false;
            nameInTransaction = firstWordFirstLetterOfSecondWord(nameInTransaction);
            foundName = firstWordFirstLetterOfSecondWord(foundName);
            return nameInTransaction.Equals(foundName, StringComparison.OrdinalIgnoreCase);
        }

        private bool bothNamesHaveAtLeastTwoWords()
        {
            return nameInTransaction.Split(' ').Length > 1 && foundName.Split(' ').Length > 1;
        }

        private string firstWordFirstLetterOfSecondWord(string fullString)
        {
            var stringParts = fullString.Split(' ');
            return stringParts[0] + " " + stringParts[1].Substring(0, 1);
        }
    }
}
