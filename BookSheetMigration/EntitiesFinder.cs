using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class EntitiesFinder<T>
    {
        protected string query = "";

        public async Task<List<T>> find()
        {
            var entityDao = new EntityDAO<T>();
            return await entityDao.@select(query);
        }

        protected static string returnEscapedArgument(string parameter)
        {
            return parameter.Replace("@", "@@").Replace("'", "''");
        }

        protected static string returnFilledQueryPart(string queryPart, params object[] arguments)
        {
            arguments = returnEscapedArguments(arguments);
            return string.Format(queryPart, arguments);
        }

        private static object[] returnEscapedArguments(object[] arguments)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                escapeArgumentIfString(arguments, i);
            }
            return arguments;
        }

        private static void escapeArgumentIfString(object[] arguments, int index)
        {
            if (arguments[index] is string)
                arguments[index] = returnEscapedArgument(arguments[index].ToString());
        }
    }
}
