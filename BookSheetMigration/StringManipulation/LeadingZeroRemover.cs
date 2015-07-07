namespace BookSheetMigration.StringManipulation
{
    public class LeadingZeroRemover: CharacterRemover
    {
        public LeadingZeroRemover(string stringToClean) : base(stringToClean)
        {
        }

        protected override string getRegexPattern()
        {
            return "^0+";
        }

        protected override string getReplacementString()
        {
            return "";
        }
    }
}
