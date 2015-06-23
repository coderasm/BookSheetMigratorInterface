namespace BookSheetMigration
{
    public class ArticleConjunctionPrepositionRemover : CharacterRemover
    {
        public ArticleConjunctionPrepositionRemover(string stringToClean) : base(stringToClean)
        {
        }

        protected override string getRegexPattern()
        {
            return "\\s+(a|an|and|the|or|of|in|from)(\\s+)";
        }

        protected override string getReplacementString()
        {
            return "\\2";
        }
    }
}
