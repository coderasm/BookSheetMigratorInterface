using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncPoco;
using Newtonsoft.Json.Linq;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class TransactionDAO
    {
        private EntityDAO<AWGTransactionDTO> entityDao = new EntityDAO<AWGTransactionDTO>(DatabaseFactory.makeDatabase());

        public async Task<List<AWGTransactionDTO>> getAll()
        {
            const string query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable;
            var transactions = await entityDao.@select(query);
            return await attachDealersAndContactsTo(transactions);
        }

        public async Task<List<AWGTransactionDTO>> get(int eventId, int transactionId)
        {
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE EventId=" + eventId + " AND TransactionId=" + transactionId;
            var transactions = await entityDao.@select(query);
            return await attachDealersAndContactsTo(transactions);
        }

        public async Task<List<AWGTransactionDTO>> getUnimportedWithReferences()
        {
            const string query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NULL";
            var transactions = await entityDao.@select(query);
            return await attachDealersAndContactsTo(transactions);
        }

        public async Task<List<AWGTransactionDTO>> getImported()
        {
            const string query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NOT NULL";
            var transactions = await entityDao.@select(query);
            return await attachDealersAndContactsTo(transactions);
        }

        public async Task<List<AWGTransactionDTO>> getImported(int eventId, int transactionId)
        {
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NOT NULL AND EventId=" + eventId + " AND TransactionId=" + transactionId;
            var transactions = await entityDao.@select(query);
            return await attachDealersAndContactsTo(transactions);
        }

        public Task<List<AWGTransactionDTO>> attachDealersAndContactsTo(List<AWGTransactionDTO> transactions)
        {
            return Task.Run(() =>
            {
                var transactionCollectionLoader = new TransactionCollectionLoader(transactions);
                return transactionCollectionLoader.loadDependentCollections();
            });
        }

        public async Task<object> import(int eventId, int transactionId)
        {
            var transactionsFound = await getUnimported(eventId, transactionId);
            if (isAlreadyImported(transactionsFound))
                return new
                {
                    success = false,
                    message = "Already Imported."
                };
            return await import(transactionsFound[0]);
        }

        private bool isAlreadyImported(List<AWGTransactionDTO> transactionsFound)
        {
            return transactionsFound.Count == 0;
        }

        public async Task<List<AWGTransactionDTO>> getUnimported(int eventId, int transactionId)
        {
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NULL AND EventId=" + eventId + " AND TransactionId=" + transactionId;
            return await entityDao.@select(query);
        }

        public async Task<List<AWGTransactionDTO>> getUnimportedWithReferences(int eventId, int transactionId)
        {
            var transactions = await getUnimported(eventId, transactionId);
            return await attachDealersAndContactsTo(transactions);
        }

        private async Task<object> import(AWGTransactionDTO transaction)
        {
            var sql = buildAndReturnImportQuery(transaction);
            var nonEntityDao = new NonEntityDAO();
            var result = await nonEntityDao.executeScalar(sql);
            if (result != 0)
            {
                await markTransactionAsImported(transaction);
                return new
                {
                    success = true,
                    message = "Imported Successfully. I will disappear in 1 minute."
                };
            }
            await markTransactionAsFailedImport(transaction);
            return new
            {
                success = false,
                message = "Import Failed. See I.T."
            };
        }

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
            //sql.Append(", @@FeeException = @0", transaction.feeException);
            return sql;
        }

        private async Task markTransactionAsImported(AWGTransactionDTO transaction)
        {
            transaction.imported = DateTime.Now;
            await entityDao.update(transaction, new List<string>() { "Imported" });
        }

        private async Task markTransactionAsFailedImport(AWGTransactionDTO transaction)
        {
            transaction.failedImport = true;
            await entityDao.update(transaction, new List<string>() { "FailedImport" });
        }

        public async Task<object> update(JToken json)
        {
            var columns = extractUpdateColumns(json.Value<JObject>());
            var transaction = json.ToObject<AWGTransactionDTO>();
            var result = await entityDao.update(transaction, columns);
            if (result != 0)
            {
                return new
                {
                    transaction.eventId,
                    transaction.transactionId,
                    result = new {
                        success = true,
                        message = "Updated Successfully"
                        }
                };
            }
            return new
            {
                transaction.eventId,
                transaction.transactionId,
                result = new {
                    success = false,
                    message = "Update Failed. See I.T."
                }
            };
        }

        public async Task<int> update(int eventId, int transactionId, JToken json)
        {
            var columns = extractUpdateColumns(json.Value<JObject>());
            var transaction = json.ToObject<AWGTransactionDTO>();
            transaction.eventId = eventId;
            transaction.transactionId = transactionId;
            return await entityDao.update(transaction, columns);
        }

        private IEnumerable<string> extractUpdateColumns(JObject jObject)
        {
            var idList = new List<string>() { "eventid", "transactionid" };
            return jObject.Properties().Where(p => !idList.Contains(p.Name.ToLower()))
                                       .Select(p => p.Name).ToList();
        }
    }
}
