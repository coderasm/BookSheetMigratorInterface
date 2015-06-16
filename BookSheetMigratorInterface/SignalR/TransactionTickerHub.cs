using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration;
using Microsoft.AspNet.SignalR;

namespace BookSheetMigratorInterface.SignalR
{
    public class TransactionTickerHub : Hub
    {
        private readonly TransactionTicker _transactionTicker;

        public TransactionTickerHub() : this(TransactionTicker.Instance) { }

        public TransactionTickerHub(TransactionTicker TransactionTicker)
        {
            _transactionTicker = TransactionTicker;
        }

        public override Task OnConnected()
        {
            UserHandler.ConnectedIds.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}