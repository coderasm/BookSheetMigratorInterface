using System.Text.RegularExpressions;

namespace BookSheetMigration
{
    public abstract class CharacterRemover
    {
        private string stringToClean;
        private RegexOptions regexOptions = RegexOptions.IgnoreCase;

        protected CharacterRemover(string stringToClean)
        {
            this.stringToClean = stringToClean;
        }

        public string remove()
        {
            var regexPattern = getRegexPattern();
            var replacementString = getReplacementString();
            return Regex.Replace(stringToClean, regexPattern, replacementString, regexOptions);
        }

        protected abstract string getRegexPattern();

        protected abstract string getReplacementString();
    }
}
