using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookSheetMigration;
using BookSheetMigratorInterface.Controllers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BookSheetMigratorInterface.SignalR
{
    public class TransactionTicker
    {
        // Singleton instance
        private readonly static Lazy<TransactionTicker> _instance = new Lazy<TransactionTicker>(() => new TransactionTicker(GlobalHost.ConnectionManager.GetHubContext<TransactionTickerHub>().Clients));
        private readonly object migrateTransactionsLock = new object();
        private readonly TimeSpan migrateInterval = TimeSpan.FromMinutes(1);
        private readonly Timer timer;
        private volatile bool migratingNewTransactions = false;

        private TransactionTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            timer = new Timer(findAndReturnNewTransactions, null, migrateInterval, migrateInterval);

        }

        public static TransactionTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        private async void findAndReturnNewTransactions(object state)
        {
            if (atLeastOneClientConnected() && !migratingNewTransactions)
            {
                migratingNewTransactions = true;
                var transactions = await migrateAndReturnNewTransactions();
                broadcastNewTransactions(transactions);
                migratingNewTransactions = false;
            }
        }

        private bool atLeastOneClientConnected()
        {
            return UserHandler.ConnectedIds.Count > 0;
        }

        private async Task<IEnumerable<AWGTransactionDTO>>  migrateAndReturnNewTransactions()
        {
            var controller = new TransactionController();
            return await controller.migrate();
        }

        private void broadcastNewTransactions(IEnumerable<AWGTransactionDTO> transactions)
        {
            Clients.All.consumeNewTransactions(transactions);
        }
    }
}