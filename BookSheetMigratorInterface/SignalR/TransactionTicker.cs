using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
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

        private readonly ConcurrentDictionary<string, AWGTransactionDTO> transactions = new ConcurrentDictionary<string, AWGTransactionDTO>();

        private readonly object migrateTransactionsLock = new object();

        private readonly TimeSpan migrateInterval = TimeSpan.FromMilliseconds(250);

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

        private void findAndReturnNewTransactions(object state)
        {
            lock (migrateTransactionsLock)
            {
                if (!migratingNewTransactions && atLeastOneClientConnected())
                {
                    migratingNewTransactions = true;
                    var transactions = migrateAndReturnNewTransactions();
                    BroadcastAWGTransactionDTOPrice(transaction);

                    migratingNewTransactions = false;
                }
            }
        }

        private bool atLeastOneClientConnected()
        {
            return UserHandler.ConnectedIds.Count > 0;
        }

        private async Task<IEnumerable<AWGTransactionDTO>>  migrateAndReturnNewTransactions()
        {
            var controller = new TransactionController();
            return await controller.GetUnimported();
        }

        private void BroadcastAWGTransactionDTOPrice(List<AWGTransactionDTO> transactions)
        {
            Clients.All.updateAWGTransactionDTOPrice(transactions);
        }
    }
}