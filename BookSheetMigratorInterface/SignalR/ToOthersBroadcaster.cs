using System.Collections.Generic;
using BookSheetMigration;
using Microsoft.AspNet.SignalR.Hubs;

namespace BookSheetMigratorInterface.SignalR
{
    public class ToOthersBroadcaster : BroadCaster
    {
        private string connectionId;

        public ToOthersBroadcaster(IHubConnectionContext<dynamic> connectionContext, List<AWGTransactionDTO> transactions, string connectionId) : base(connectionContext, transactions)
        {
            this.connectionId = connectionId;
        }

        protected override bool clientsConnected()
        {
            return UserHandler.ConnectedIds.Count > 1;
        }

        protected override void doBroadcast()
        {
            connectionContext.AllExcept(connectionId).consumeNewTransactions(transactions);
        }
    }
}
