using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigration.Test
{
    [TestClass]
    public class AWGTransactionMigratorTest
    {
        [TestMethod]
        public void testTransactionMigration()
        {
            DataMigrator<AWGTransactionDTO> transactionMigrator = new BookSheetTransactionMigrator();
            transactionMigrator.migrate();
        }
    }
}
