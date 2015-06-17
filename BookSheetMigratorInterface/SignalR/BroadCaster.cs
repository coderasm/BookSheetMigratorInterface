using System.Collections.Generic;
using BookSheetMigration;
using Microsoft.AspNet.SignalR.Hubs;

namespace BookSheetMigratorInterface.SignalR
{
    public abstract class BroadCaster
    {
        protected List<AWGTransactionDTO> transactions;
        protected IHubConnectionContext<dynamic> connectionContext;

        public BroadCaster(IHubConnectionContext<dynamic> connectionContext,
            List<AWGTransactionDTO> transactions)
        {
            this.connectionContext = connectionContext;
            this.transactions = transactions;
        }

        public void broadcast()
        {
            if(clientsConnected())
                doBroadcast();
        }

        protected abstract bool clientsConnected();

        protected abstract void doBroadcast();
    }
}
