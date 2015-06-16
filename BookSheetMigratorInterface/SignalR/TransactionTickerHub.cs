using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BookSheetMigratorInterface.SignalR
{
    [HubName("transactionTicker")]
    public class TransactionTickerHub : Hub
    {
        private readonly TransactionTicker _transactionTicker;

        public TransactionTickerHub() : this(TransactionTicker.Instance) { }

        private TransactionTickerHub(TransactionTicker TransactionTicker)
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