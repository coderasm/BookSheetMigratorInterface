using System.Text.RegularExpressions;

namespace BookSheetMigration
{
    class DealersFinderByAddressAndCity : DealersFinder
    {
        private const string queryPart = " AND c1.ADDRESS1 LIKE '%{0}%' AND c1.CITY LIKE '%{1}%'";

        public DealersFinderByAddressAndCity(string streetAddress, string city) : base(returnFilledQueryPart(queryPart, cleanAndReturnStreetNumber(streetAddress), city.Trim()))
        {
        }

        private static string cleanAndReturnStreetNumber(string address)
        {
            var addressOnlyWordCharacters = Regex.Replace(address, "[^\\w\\s]", "");
            var addressParts = addressOnlyWordCharacters.Split(' ');
            var streetNumber = addressParts[0].Trim();
            return streetNumber;
        }
    }
}
