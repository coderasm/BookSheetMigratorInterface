using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BookSheetMigration
{
    class ListEventOperation : SoapOperation<AWGEventDirectory>
    {
        private const string listEventPathToDataNodeFromRoot = "//AWGDataSet";

        public ListEventOperation(EventStatus eventStatus)
        {
            action = "ListEvent";
            actionArguments.Add("eventStatus", eventStatus.ToString());
        }

        protected override void setPathToDataNodeFromRoot()
        {
            pathToDataNodeFromRoot = listEventPathToDataNodeFromRoot;
        }
    }
}
