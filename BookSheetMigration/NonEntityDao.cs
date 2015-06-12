using System.Threading.Tasks;
using AsyncPoco;

namespace BookSheetMigration
{
    public class NonEntityDAO
    {
        private Database createConnection()
        {
            return DatabaseFactory.makeDatabase();
        }

        public async Task<int> executeScalar(Sql sql)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<int> executeScalar(string sql, params object[] sqlparams)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.ExecuteScalarAsync<int>(sql, sqlparams);
            }
        }
    }
}
