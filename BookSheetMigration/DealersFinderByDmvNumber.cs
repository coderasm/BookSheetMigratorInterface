namespace BookSheetMigration.AwgToHoldingTable
{
    public class DealersFinderByDmvNumber : DealersFinder
    {
        private const string queryPart = " AND c2.UDMVNUM LIKE '%{0}%'";

        public DealersFinderByDmvNumber(string dmvNumber) : base(returnFilledQueryPart(queryPart, dmvNumber))
        {
        }
    }
}
