using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.HoldingTableToWebInterface;
using Newtonsoft.Json.Linq;

namespace BookSheetMigratorInterface.Controllers
{
    public class BulkUpdateAction : BulkAction<JToken>
    {
        public BulkUpdateAction(IEnumerable<JToken> transactions):base(transactions)
        {
        }

        protected override async Task<object> process(JToken transaction)
        {
            var transactionDao = new TransactionDAO();
            var result = await transactionDao.update(transaction);
            return result;
        }
    }
}