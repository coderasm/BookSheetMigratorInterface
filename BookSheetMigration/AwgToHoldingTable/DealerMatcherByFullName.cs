using System;
using System.Collections.Generic;

namespace BookSheetMigration.AwgToHoldingTable
{
    public class DealerMatcherByFullName : DealerMatcher
    {
        public DealerMatcherByFullName(IEnumerable<DealerDTO> dealers, DealerIdInserter dealerIdInserter) : base(dealers, dealerIdInserter)
        {
        }

        protected override bool doCompare()
        {
            return nameInTransaction.Equals(foundName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
