using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AsyncPoco;
using BookSheetMigration;
using Newtonsoft.Json.Linq;

namespace BookSheetMigratorInterface.Controllers
{
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        // POST: api/Transaction/import/{
        [Route("import")]
        public async Task<object> importTransactions(AWGTransactionDTO transaction)
        {
            var sql = buildAndReturnImportQuery(transaction);
            var nonEntityDao = new NonEntityDAO();
            var result = await nonEntityDao.executeScalar(sql);
            if (result != 0)
                await markTransactionAsImported(transaction);
            return new {success = (result != 0)};
        }

        [NonAction]
        private Sql buildAndReturnImportQuery(AWGTransactionDTO transaction)
        {
            var sql = Sql.Builder.Append("EXEC Booksheet_Insert_Vehicle @@pVIN = @0", transaction.vin);
            sql.Append(", @@pSID = @0", transaction.sellerDealerId);
            sql.Append(", @@pBIID = @0", transaction.buyerDealerId);
            sql.Append(", @@pCID = @0", transaction.buyerContactId);
            sql.Append(", @@Year = @0", transaction.year);
            sql.Append(", @@Make = @0", transaction.make);
            sql.Append(", @@Model = @0", transaction.model);
            sql.Append(", @@Miles = @0", transaction.mileage);
            sql.Append(", @@pBid = @0", transaction.bidAmount);
            sql.Append(", @@SoldDT = @0", transaction.soldDate);
            sql.Append(", @@Trans = @0", transaction.transportFee);
            return sql;
        }

        [NonAction]
        private async Task<int> markTransactionAsImported(AWGTransactionDTO transaction)
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            transaction.imported = DateTime.Now;
            return await entityDao.update(transaction, new List<string>() {"Imported"});
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
            var transactions = await entityDao.@select("SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NULL");
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

        // POST: api/Transaction/update
        [Route("update")]
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
