namespace BookSheetMigration
{
    public class RepeatedWhitespaceRemover : CharacterRemover
    {
        public RepeatedWhitespaceRemover(string stringToClean) : base(stringToClean)
        {
        }

        protected override string getRegexPattern()
        {
            return "\\s\\s+";
        }

        protected override string getReplacementString()
        {
            return " ";
        }
    }
}
