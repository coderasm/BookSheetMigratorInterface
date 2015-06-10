using System;

namespace BookSheetMigration
{
    public class ListTransactionOperation : SoapOperation<AWGTransactionDirectory>
    {
        private const string listTransactionPathToDataNodeFromRoot = "//AWGDataSet";

        public ListTransactionOperation(TransactionStatus transactionStatus, DateTime dateFrom , DateTime dateTo, int eventId = 0, string sellerDealerNumber = "", string buyerDealerNumber = "")
        {
            initializeActionAndAddArguments(transactionStatus, eventId, sellerDealerNumber, buyerDealerNumber);
            actionArguments.Add("dateFrom", dateFrom.ToString("O"));
            actionArguments.Add("dateTo", dateTo.ToString("O"));
        }

        public ListTransactionOperation(TransactionStatus transactionStatus, int eventId = 0, string sellerDealerNumber = "", string buyerDealerNumber = "")
        {
            initializeActionAndAddArguments(transactionStatus, eventId, sellerDealerNumber, buyerDealerNumber);
            actionArguments.Add("dateFrom", "");
            actionArguments.Add("dateTo", "");
        }

        public ListTransactionOperation(TransactionStatus transactionStatus, DateTime dateFrom, int eventId = 0, string sellerDealerNumber = "", string buyerDealerNumber = "")
        {
            initializeActionAndAddArguments(transactionStatus, eventId, sellerDealerNumber, buyerDealerNumber);
            actionArguments.Add("dateFrom", dateFrom.ToString("O"));
            actionArguments.Add("dateTo", "");
        }

        private void initializeActionAndAddArguments(TransactionStatus transactionStatus, int eventId, string sellerDealerNumber, string buyerDealerNumber)
        {
            action = "ListTransaction";
            actionArguments.Add("transactionStatus", transactionStatus.ToString());
            actionArguments.Add("eventId", eventId.ToString());
            actionArguments.Add("sellerDealerNumber", sellerDealerNumber);
            actionArguments.Add("buyerDealerNumber", buyerDealerNumber);
        }

        protected override void setPathToDataNodeFromRoot()
        {
            pathToDataNodeFromRoot = listTransactionPathToDataNodeFromRoot;
        }
    }
}
