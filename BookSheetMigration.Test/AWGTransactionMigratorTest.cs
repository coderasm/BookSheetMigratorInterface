using BookSheetMigration.AwgToHoldingTable;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigration.Test
{
    [TestClass]
    public class AWGTransactionMigratorTest
    {
        [TestMethod]
        public void testTransactionMigration()
        {
            DataMigrator<AWGTransactionDTO> transactionMigrator = new BookSheetTransactionMigrator(new TransactionIdsInserter());
            transactionMigrator.migrate();
        }
    }
}
