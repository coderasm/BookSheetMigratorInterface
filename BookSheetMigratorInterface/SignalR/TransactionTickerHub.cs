using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace BookSheetMigratorInterface.SignalR
{
    [HubName("transactionTicker")]
    public class TransactionTickerHub : Hub
    {
        private readonly TransactionTicker transactionTicker;

        public TransactionTickerHub() : this(TransactionTicker.Instance) { }

        private TransactionTickerHub(TransactionTicker TransactionTicker)
        {
            transactionTicker = TransactionTicker;
        }

        public async Task<object> GetUnimported()
        {
            return await transactionTicker.getUnimported(Context.ConnectionId);
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