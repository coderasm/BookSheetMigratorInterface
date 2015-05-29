using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BookSheetMigration;

namespace BookSheetMigratorInterface.Controllers
{
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        // POST: api/Transaction/import/{
        [Route("import/{eventId:int}/{transactionId:int}")]
        public IEnumerable<string> importTransactions(int eventId, int transactionId)
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Transaction/imported
        [Route("imported")]
        public IEnumerable<string> GetImported()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Transaction/unimported
        [Route("unimported")]
        public async Task<IEnumerable<AWGTransactionDTO>> GetUnimported()
        {
            var transactions = await findUnimportedTransactions();
            await attachDealersAndContactsTo(transactions);
            return transactions;
        }

        [NonAction]
        public async Task<List<AWGTransactionDTO>> findUnimportedTransactions()
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var transactions = await entityDao.@select("SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE ImportedIntoGsv IS NULL");
            return transactions;
        }

        [NonAction]
        private Task attachDealersAndContactsTo(List<AWGTransactionDTO> transactions)
        {
            return Task.Run(() => {
                var transactionCollectionLoader = new TransactionCollectionLoader(transactions);
                transactionCollectionLoader.loadDependentCollections();
            });
        }

        // GET: api/Transaction/5
        [Route("")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Transaction
        [Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Transaction/5
        [Route("")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Transaction/5
        [Route("")]
        public void Delete(int id)
        {
        }
    }
}
