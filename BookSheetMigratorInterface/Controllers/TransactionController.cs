using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BookSheetMigration;
using Newtonsoft.Json.Linq;

namespace BookSheetMigratorInterface.Controllers
{
    [System.Web.Http.RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        // POST: api/Transaction/import/{
        [System.Web.Http.Route("import/{eventId:int}/{transactionId:int}")]
        public IEnumerable<string> importTransactions(int eventId, int transactionId)
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Transaction/imported
        [System.Web.Http.Route("imported")]
        public IEnumerable<string> GetImported()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Transaction/unimported
        [System.Web.Http.Route("unimported")]
        public async Task<IEnumerable<AWGTransactionDTO>> GetUnimported()
        {
            var transactions = await findUnimportedTransactions();
            await attachDealersAndContactsTo(transactions);
            return transactions;
        }

        [System.Web.Http.NonAction]
        public async Task<List<AWGTransactionDTO>> findUnimportedTransactions()
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var transactions = await entityDao.@select("SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE ImportedIntoGsv IS NULL");
            return transactions;
        }

        [System.Web.Http.NonAction]
        private Task attachDealersAndContactsTo(List<AWGTransactionDTO> transactions)
        {
            return Task.Run(() => {
                var transactionCollectionLoader = new TransactionCollectionLoader(transactions);
                transactionCollectionLoader.loadDependentCollections();
            });
        }

        // POST: api/Transaction/update
        [System.Web.Http.Route("update")]
        public async Task<object> PostUpdate(JToken json)
        {
            var jsonkeys = extractKeys(json.Value<JObject>());
            var transaction = json.ToObject<AWGTransactionDTO>();
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var result = await entityDao.update(transaction, jsonkeys);
            return new {success=(result != 0)};
        }

        private List<string> extractKeys(JObject jObject)
        {
            return jObject.Properties().Select(p => p.Name).ToList();
        }

        // GET: api/Transaction/5
        [System.Web.Http.Route("")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Transaction
        [System.Web.Http.Route("")]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Transaction/5
        [System.Web.Http.Route("")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Transaction/5
        [System.Web.Http.Route("")]
        public void Delete(int id)
        {
        }
    }
}
