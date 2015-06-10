namespace BookSheetMigration.AwgToHoldingTable
{
    class DealersFinderByPhoneNumber : DealersFinder
    {
        private const string queryPart = " AND c1.PHONE1 LIKE '%{0}%'";

        public DealersFinderByPhoneNumber(string phoneNumber) : base(returnFilledQueryPart(queryPart, createParameterizedPhoneNumber(phoneNumber)))
        {
        }

        private static string createParameterizedPhoneNumber(string phoneNumber)
        {
            string placeholder = "%";
            string areaCode = phoneNumber.Substring(0, 3);
            string firstThreeNumbers = phoneNumber.Substring(3, 3);
            string lastFourNumbers = phoneNumber.Substring(6, 4);
            return areaCode + placeholder + firstThreeNumbers + placeholder + lastFourNumbers;
        }
    }
}
