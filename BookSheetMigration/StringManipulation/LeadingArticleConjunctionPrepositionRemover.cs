namespace BookSheetMigration.StringManipulation
{
    public class LeadingArticleConjunctionPrepositionRemover : CharacterRemover
    {
        public LeadingArticleConjunctionPrepositionRemover(string stringToClean) : base(stringToClean)
        {
        }

        protected override string getRegexPattern()
        {
            return "^(a|an|the)\\s+";
        }

        protected override string getReplacementString()
        {
            return "";
        }
    }
}
