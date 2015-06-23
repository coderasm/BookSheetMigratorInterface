using BookSheetMigration.StringManipulation;

namespace BookSheetMigration
{
    public class StringCleaner
    {
        private string name;
        public StringCleaner(string name)
        {
            this.name = name;
        }

        public string clean()
        {
            applyAllCleaners();
            return name.Trim();
        }

        protected void applyAllCleaners()
        {
            removePunctuation();
            removeLeadingArticleConjunctionPreposition();
            removeArticleConjunctionPreposition();
            removeRepeatedWhitespace();
        }

        protected void removePunctuation()
        {
            var punctuationRemover = new PunctuationRemover(name);
            name = punctuationRemover.remove();
        }

        protected void removeLeadingArticleConjunctionPreposition()
        {
            var leadingArticleConjunctionPrepositionRemover = new LeadingArticleConjunctionPrepositionRemover(name);
            name = leadingArticleConjunctionPrepositionRemover.remove();
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
