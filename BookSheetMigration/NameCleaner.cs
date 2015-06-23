namespace BookSheetMigration
{
    public class NameCleaner
    {
        private string name;
        protected NameCleaner(string name)
        {
            this.name = name;
        }

        public string clean()
        {
            applyAllCleaners();
            return name;
        }

        protected void applyAllCleaners()
        {
            removePunctuation();
            removeArticleConjunctionPreposition();
            removeRepeatedWhitespace();
        }

        protected void removePunctuation()
        {
            var punctuationRemover = new PunctuationRemover(name);
            name = punctuationRemover.remove();
        }

        protected void removeArticleConjunctionPreposition()
        {
            var articleConjunctionPrepositionRemover = new ArticleConjunctionPrepositionRemover(name);
            name = articleConjunctionPrepositionRemover.remove();
        }

        protected void removeRepeatedWhitespace()
        {
            var repeatedWhitespaceRemover = new RepeatedWhitespaceRemover(name);
            name = repeatedWhitespaceRemover.remove();
        }
    }
}
