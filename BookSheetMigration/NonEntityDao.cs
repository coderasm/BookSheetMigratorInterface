using System.Threading.Tasks;
using AsyncPoco;

namespace BookSheetMigration
{
    public class NonEntityDAO
    {
        public Database database { get; private set; }

        public NonEntityDAO(Database database)
        {
            this.database = database;
        }

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

        public async Task<int> executeScalarShared(Sql sql)
        {
                return await database.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> executeScalar(string sql, params object[] sqlparams)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.ExecuteScalarAsync<int>(sql, sqlparams);
            }
        }

        public async Task<int> executeScalarShared(string sql, params object[] sqlparams)
        {
            return await database.ExecuteScalarAsync<int>(sql, sqlparams);
        }
    }
}
