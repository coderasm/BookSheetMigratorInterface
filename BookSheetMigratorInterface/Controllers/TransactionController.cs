using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Http;
using BookSheetMigration;
using BookSheetMigration.HoldingTableToWebInterface;
using Newtonsoft.Json.Linq;

namespace BookSheetMigratorInterface.Controllers
{
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        // GET: api/Transaction
        [Route("")]
        public async Task<IEnumerable<AWGTransactionDTO>> Get()
        {
            var transactionDao = new TransactionDAO();
            return await transactionDao.getAll();
        }

        // GET: api/Transaction/eventId/transactionId
        [Route("{eventId:int}/{transactionId:int}")]
        public async Task<IEnumerable<AWGTransactionDTO>> Get(int eventId, int transactionId)
        {
            var transactionDao = new TransactionDAO();
            return await transactionDao.get(eventId, transactionId);
        }

        // POST: api/Transaction/import
        [Route("import")]
        public async Task<IEnumerable<object>> PostMulipleImports(IEnumerable<AWGTransactionDTO> transactions)
        {
            var results = new List<object>();
            await importAndReturnResults(transactions, results);
            return results;
        }

        private Task importAndReturnResults(IEnumerable<AWGTransactionDTO> transactions, List<object> results)
        {
            return Task.Run(async () =>
            {
                await processImports(transactions, results);
            });
        }

        private async Task processImports(IEnumerable<AWGTransactionDTO> transactions, List<object> results)
        {
            foreach (var transaction in transactions)
            {
                await importTransactionAndReturnResult(results, transaction);
            }
        }

        private async Task importTransactionAndReturnResult(List<object> results, AWGTransactionDTO transaction)
        {
            var importResult = await PostImport(transaction.eventId, transaction.transactionId);
            var finalResult = new {transaction.eventId, transaction.transactionId, result = importResult};
            results.Add(finalResult);
        }

        // POST: api/Transaction/import/eventId/transactionId
        [Route("import/{eventId:int}/{transactionId:int}")]
        public async Task<object> PostImport(int eventId, int transactionId)
        {
            var transactionDao = new TransactionDAO();
            return await transactionDao.import(eventId, transactionId);
        }

        // GET: api/Transaction/imported
        [Route("imported")]
        public async Task<IEnumerable<AWGTransactionDTO>> Getimported()
        {
            var transactionDao = new TransactionDAO();
            return await transactionDao.getImported();
        }

        // GET: api/Transaction/imported/eventId/transactionId
        [Route("imported/{eventId:int}/{transactionId:int}")]
        public async Task<IEnumerable<AWGTransactionDTO>> Getimported(int eventId, int transactionId)
        {
            var transactionDao = new TransactionDAO();
            return await transactionDao.getImported(eventId, transactionId);
        }

        // GET: api/Transaction/unimported
        [Route("unimported")]
        public async Task<IEnumerable<AWGTransactionDTO>> GetUnimported()
        {
            var transactionDao = new TransactionDAO();
            return await transactionDao.getUnimported();
        }

        // GET: api/Transaction/unimported/eventId/transactionId
        [Route("unimported/{eventId:int}/{transactionId:int}")]
        public async Task<IEnumerable<AWGTransactionDTO>> GetUnimported(int eventId, int transactionId)
        {
            var transactionDao = new TransactionDAO();
            return await transactionDao.getUnimported(eventId, transactionId);
        }

        // PUT: api/Transaction/bulk-update
        [Route("bulk-update")]
        public async Task<object> Put(JArray transactions)
        {
            var results = new List<object>();
            await updateAndReturnResults(transactions, results);
            return results;
        }

        private Task updateAndReturnResults(JArray transactions, List<object> results)
        {
            return Task.Run(async () =>
            {
                await processUpdates(transactions, results);
            });
        }

        private async Task processUpdates(JArray transactions, List<object> results)
        {
            foreach (var transaction in transactions)
            {
                await updateTransactionAndReturnResult(results, transaction);
            }
        }

        private async Task updateTransactionAndReturnResult(List<object> results, JToken transaction)
        {
            var transactionDao = new TransactionDAO();
            var result = await transactionDao.update(transaction);
            results.Add(result);
        }

        // PUT: api/Transaction/eventId/transactionId
        [Route("{eventId:int}/{transactionId:int}")]
        public async Task<object> PostUpdate(int eventId, int transactionId, [FromBody]JToken json)
        {
            var transactionDao = new TransactionDAO();
            var result = await transactionDao.update(eventId, transactionId, json);
            return new { success = (result != 0) };
        }

        // DELETE: api/Transaction/5
        [Route("")]
        public void Delete(int id)
        {
        }
    }
}
