﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncPoco;
using Newtonsoft.Json.Linq;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class TransactionDAO
    {

        public async Task<List<AWGTransactionDTO>> getAll()
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable;
            var transactions = await entityDao.@select(query);
            await attachDealersAndContactsTo(transactions);
            return transactions;
        }

        public async Task<List<AWGTransactionDTO>> get(int eventId, int transactionId)
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE EventId=" + eventId + " AND TransactionId=" + transactionId;
            var transactions = await entityDao.@select(query);
            await attachDealersAndContactsTo(transactions);
            return transactions;
        }

        public async Task<List<AWGTransactionDTO>> getUnimported()
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NULL";
            var transactions = await entityDao.@select(query);
            await attachDealersAndContactsTo(transactions);
            return transactions;
        }

        public async Task<List<AWGTransactionDTO>> getImported()
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NOT NULL";
            var transactions = await entityDao.@select(query);
            await attachDealersAndContactsTo(transactions);
            return transactions;
        }

        public async Task<List<AWGTransactionDTO>> getImported(int eventId, int transactionId)
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NOT NULL AND EventId=" + eventId + " AND TransactionId=" + transactionId;
            var transactions = await entityDao.@select(query);
            await attachDealersAndContactsTo(transactions);
            return transactions;
        }

        public Task attachDealersAndContactsTo(List<AWGTransactionDTO> transactions)
        {
            return Task.Run(() =>
            {
                var transactionCollectionLoader = new TransactionCollectionLoader(transactions);
                transactionCollectionLoader.loadDependentCollections();
            });
        }

        public async Task<int> import(int eventId, int transactionId)
        {
            var transactionsFound = await getUnimported(eventId, transactionId);
            if (isAlreadyImported(transactionsFound))
                return 0;
            return await import(transactionsFound[0]);
        }

        private bool isAlreadyImported(List<AWGTransactionDTO> transactionsFound)
        {
            return transactionsFound.Count == 0;
        }

        public async Task<List<AWGTransactionDTO>> getUnimported(int eventId, int transactionId)
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            var query = "SELECT * FROM " + Settings.ABSBookSheetTransactionTable + " WHERE Imported IS NULL AND EventId=" + eventId + " AND TransactionId=" + transactionId;
            var transactions = await entityDao.@select(query);
            await attachDealersAndContactsTo(transactions);
            return transactions;
        }

        private async Task<int> import(AWGTransactionDTO transaction)
        {
            var sql = buildAndReturnImportQuery(transaction);
            var nonEntityDao = new NonEntityDAO();
            var result = await nonEntityDao.executeScalar(sql);
            if (result != 0)
                markTransactionAsImported(transaction);
            else
                markTransactionAsFailedImport(transaction);
            return result;
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
            return sql;
        }

        private void markTransactionAsImported(AWGTransactionDTO transaction)
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            transaction.imported = DateTime.Now;
            entityDao.update(transaction, new List<string>() { "Imported" });
        }

        private void markTransactionAsFailedImport(AWGTransactionDTO transaction)
        {
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            transaction.failedImport = true;
            entityDao.update(transaction, new List<string>() { "FailedImport" });
        }

        public async Task<int> update(JToken json)
        {
            var columns = extractUpdateColumns(json.Value<JObject>());
            var transaction = json.ToObject<AWGTransactionDTO>();
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            return await entityDao.update(transaction, columns);
        }

        public async Task<int> update(int eventId, int transactionId, JToken json)
        {
            var columns = extractUpdateColumns(json.Value<JObject>());
            var transaction = json.ToObject<AWGTransactionDTO>();
            transaction.eventId = eventId;
            transaction.transactionId = transactionId;
            var entityDao = new EntityDAO<AWGTransactionDTO>();
            return await entityDao.update(transaction, columns);
        }

        private List<string> extractUpdateColumns(JObject jObject)
        {
            var idList = new List<string>() { "eventid", "transactionid" };
            return jObject.Properties().Where(p => !idList.Contains(p.Name.ToLower()))
                                       .Select(p => p.Name).ToList();
        }
    }
}
