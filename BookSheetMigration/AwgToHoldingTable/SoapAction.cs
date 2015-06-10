using System.Collections.Generic;

namespace BookSheetMigration
{
    public class SoapAction
    {
        public string Operation { get; private set; }
        public string actionNamespace { get; private set; }
        public Dictionary<string, string> parameters { get; private set; }

        public SoapAction(string operation, string actionNamespace)
        {
            this.Operation = operation;
            this.actionNamespace = actionNamespace;
            parameters = new Dictionary<string, string>();
        }

        public int getPairCount()
        {
            return parameters.Count;
        }

        public void addParameterPairToAction(string key, string value)
        {
            parameters.Add(key, value);
        }
    }
}
