using System.Collections.Generic;
using BookSheetMigration;
using Microsoft.AspNet.SignalR.Hubs;

namespace BookSheetMigratorInterface.SignalR
{
    public class ToAllBroadcaster : BroadCaster
    {
        public ToAllBroadcaster(IHubConnectionContext<dynamic> connectionContext, List<AWGTransactionDTO> transactions) : base(connectionContext, transactions)
        {
        }

        protected override bool clientsConnected()
        {
            return true;
        }

        protected override void doBroadcast()
        {
            connectionContext.All.consumeNewTransactions(transactions);
        }
    }
}