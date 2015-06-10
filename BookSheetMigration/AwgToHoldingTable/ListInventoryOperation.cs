
namespace BookSheetMigration
{
    class ListInventoryOperation : SoapOperation<AWGInventoryDirectory>
    {
        private const string listInventoryPathToDataNodeFromRoot = "//AWGDataSet";

        public ListInventoryOperation(InventoryStatus inventoryStatus, int eventId = 0, string dealerNumber = "")
        {
            action = "ListInventory";
            actionArguments.Add("inventoryStatus", InventoryStatus.Sold.ToString());
            actionArguments.Add("eventId", eventId.ToString());
            actionArguments.Add("dealerNumber", dealerNumber);
        }

        protected override void setPathToDataNodeFromRoot()
        {
            pathToDataNodeFromRoot = listInventoryPathToDataNodeFromRoot;
        }
    }
}
