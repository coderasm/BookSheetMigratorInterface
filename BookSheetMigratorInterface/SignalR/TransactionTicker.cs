using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookSheetMigration;
using BookSheetMigration.HoldingTableToWebInterface;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BookSheetMigratorInterface.SignalR
{
    public class TransactionTicker
    {
        // Singleton instance
        private readonly static Lazy<TransactionTicker> instance = new Lazy<TransactionTicker>(() => new TransactionTicker(GlobalHost.ConnectionManager.GetHubContext<TransactionTickerHub>().Clients));
        private readonly TimeSpan migrateInterval = Settings.minutesBetweenMigrations;
        private readonly Timer timer;
        private volatile bool migratingNewTransactions = false;
        private BroadCaster broadcaster;

        private TransactionTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            timer = new Timer(tickerDelegate, null, migrateInterval, migrateInterval);

        }

        public static TransactionTicker Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public async Task<object> getUnimported(string clientConnectionId)
        {
            await migrateAndBroadcastToOthers(clientConnectionId);
            var transactionDao = new TransactionDAO();
            var unimported = await transactionDao.getUnimportedWithReferences();
            var entityDao = new EntityDAO<dynamic>(DatabaseFactory.makeDatabase());
            var feeExceptions = await entityDao.@select("SELECT * FROM " + Settings.ABSBookSheetFeeExceptionTable);
            return new
            {
                unimported,
                feeExceptions
            };
        }

        private async void tickerDelegate(object state)
        {
            await migrateAndBroadcastToAll();
        }

        private async Task migrateAndBroadcastToAll()
        {
            if (!migratingNewTransactions && clientsConnected())
            {
                migratingNewTransactions = true;
                var transactions = await migrateAndAttachCollections();
                broadcaster = new ToAllBroadcaster(Clients, transactions);
                broadcaster.broadcast();
                migratingNewTransactions = false;
            }
        }

        public async Task migrateAndBroadcastToAll(DateTime beginSoldDate, DateTime endSoldDate)
        {
            if (!migratingNewTransactions && clientsConnected())
            {
                migratingNewTransactions = true;
                var transactions = await migrateAndAttachCollections(beginSoldDate, endSoldDate);
                broadcaster = new ToAllBroadcaster(Clients, transactions);
                broadcaster.broadcast();
                migratingNewTransactions = false;
            }
        }

        private async Task<List<AWGTransactionDTO>> migrateAndAttachCollections(DateTime beginSoldDate, DateTime endSoldDate)
        {
            var transactions = await migrateAndReturnNewTransactions(beginSoldDate, endSoldDate);
            var transactionDao = new TransactionDAO();
            return await transactionDao.attachDealersAndContactsTo(transactions);
        }

        private async Task<List<AWGTransactionDTO>> migrateAndReturnNewTransactions(DateTime beginSoldDate, DateTime endSoldDate)
        {
            var migrator = new MultipleMigrator();
            return await migrator.migrate(beginSoldDate, endSoldDate);
        }

        private bool clientsConnected()
        {
            return UserHandler.ConnectedIds.Count > 0;
        }

        private async Task migrateAndBroadcastToOthers(string connectionId)
        {
            if (!migratingNewTransactions)
            {
                migratingNewTransactions = true;
                var transactions = await migrateAndAttachCollections();
                broadcaster = new ToOthersBroadcaster(Clients, transactions, connectionId);
                broadcaster.broadcast();
                migratingNewTransactions = false;
            }
        }

        private async Task<List<AWGTransactionDTO>> migrateAndAttachCollections()
        {
            var transactions = await migrateAndReturnNewTransactions();
            var transactionDao = new TransactionDAO();
            return await transactionDao.attachDealersAndContactsTo(transactions);
        }

        private async Task<List<AWGTransactionDTO>>  migrateAndReturnNewTransactions()
        {
            var migrator = new MultipleMigrator();
            return await migrator.migrate();
        }
    }
}