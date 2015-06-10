using System.Threading.Tasks;
using AsyncPoco;

namespace BookSheetMigration
{
    public class NonEntityDAO
    {
        private readonly Database databaseConnection;

        public NonEntityDAO()
        {
            databaseConnection = DatabaseFactory.makeDatabase();
        }

        public async Task<int> executeScalar(Sql sql)
        {
            return await databaseConnection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> executeScalar(string sql, params object[] sqlparams)
        {
            return await databaseConnection.ExecuteScalarAsync<int>(sql, sqlparams);
        }
    }
}
