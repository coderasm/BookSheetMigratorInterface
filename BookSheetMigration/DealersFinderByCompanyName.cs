namespace BookSheetMigration
{
    public class DealersFinderByCompanyName : DealersFinder
    {
        private const string queryPart = " AND c1.COMPANY LIKE '%{0}%'";

        public DealersFinderByCompanyName(string companyName) : base(returnFilledQueryPart(queryPart, createParsedName(companyName)))
        {
        }

        private static string createParsedName(string companyName)
        {
            var nameParts = companyName.Split(' ');
            var firstWord = nameParts[0];
            var firstLetterOfSecondWord = nameParts[1].Substring(0, 1);
            return firstWord + " " + firstLetterOfSecondWord;
        }
    }
}
