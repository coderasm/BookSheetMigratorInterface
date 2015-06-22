using System.Threading.Tasks;
using BookSheetMigration;
using BookSheetMigration.HoldingTableToWebInterface;
using System.Collections.Generic;

namespace BookSheetMigratorInterface.Controllers
{
    public class BulkImportAction : BulkAction<AWGTransactionDTO>
    {
        public BulkImportAction(IEnumerable<AWGTransactionDTO> transactions):base(transactions)
        {
        }

        protected override async Task<object> process(AWGTransactionDTO transaction)
        {
            var transactionDao = new TransactionDAO();
            var importResult = await transactionDao.import(transaction.eventId, transaction.transactionId);
            return new {transaction.eventId, transaction.transactionId, result = importResult};
        }
    }
}