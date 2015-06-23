namespace BookSheetMigration
{
    public class PunctuationRemover : CharacterRemover
    {
        public PunctuationRemover(string stringToClean) : base(stringToClean)
        {
        }

        protected override string getRegexPattern()
        {
            return "[^\\w\\s]";
        }

        protected override string getReplacementString()
        {
            return "";
        }
    }
}
