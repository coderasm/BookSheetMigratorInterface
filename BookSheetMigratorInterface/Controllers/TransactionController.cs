using System.Collections.Generic;
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

        // POST: api/Transaction/import/eventId/transactionId
        [Route("import/{eventId:int}/{transactionId:int}")]
        public async Task<object> PostImport(int eventId, int transactionId)
        {
            var transactionDao = new TransactionDAO();
            var result = await transactionDao.import(eventId, transactionId);
            return new { success = (result != 0) };
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
            await migrate();
            var transactionDao = new TransactionDAO();
            return await transactionDao.getUnimported();
        }

        // GET: api/Transaction/unimported/eventId/transactionId
        [Route("unimported/{eventId:int}/{transactionId:int}")]
        public async Task<IEnumerable<AWGTransactionDTO>> GetUnimported(int eventId, int transactionId)
        {
            await migrate();
            var transactionDao = new TransactionDAO();
            return await transactionDao.getUnimported(eventId, transactionId);
        }

        [NonAction]
        public async Task<List<AWGTransactionDTO>>  migrate()
        {
                await migrateEvents();
                return await migrateTransactions();
        }

        [NonAction]
        private Task migrateEvents()
        {
            return Task.Run(() =>
            {
                DataMigrator<AWGEventDTO> upcomingEventMigrator = new BookSheetEventMigrator(EventStatus.Upcoming);
                upcomingEventMigrator.migrate();
                DataMigrator<AWGEventDTO> inprogressEventMigrator = new BookSheetEventMigrator(EventStatus.InProgress);
                inprogressEventMigrator.migrate();
            });
        }

        [NonAction]
        private Task<List<AWGTransactionDTO>>  migrateTransactions()
        {
            return Task.Run(() =>
            {
                DataMigrator<AWGTransactionDTO> transactionMigrator = new BookSheetTransactionMigrator();
                return transactionMigrator.migrate();
            });
        }

        // POST: api/Transaction/update
        [Route("update")]
        public async Task<object> PostUpdate(JToken json)
        {
            var transactionDao = new TransactionDAO();
            var result = await transactionDao.update(json);
            return new {success=(result != 0)};
        }

        // POST: api/Transaction/update/eventId/transactionId
        [Route("update/{eventId:int}/{transactionId:int}")]
        public async Task<object> PostUpdate(int eventId, int transactionId, [FromBody]JToken json)
        {
            var transactionDao = new TransactionDAO();
            var result = await transactionDao.update(eventId, transactionId, json);
            return new { success = (result != 0) };
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
