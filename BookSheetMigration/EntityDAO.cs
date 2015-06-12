using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncPoco;

namespace BookSheetMigration
{
    public class EntityDAO<T>
    {
        private Database createConnection()
        {
            return DatabaseFactory.makeDatabase();
        }

        public async Task<List<T>> select(Sql sql)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.FetchAsync<T>(sql);
            }
            
        }

        public async Task<List<T>> select(string sql)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.FetchAsync<T>(sql);
            }
        }

        public async Task<List<T>> select(string sql, params object[] sqlparams)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.FetchAsync<T>(sql, sqlparams);
            }
        }

        public async Task<int> update(T entity)
        {
            using (var databaseConnection = createConnection())
            {
                if (await exists(entity))
                    return await databaseConnection.UpdateAsync(entity);
                return 0;
            }
        }

        public async Task<int> update(T entity, IEnumerable<string> columns)
        {
            using (var databaseConnection = createConnection())
            {
                if (await exists(entity))
                    return await databaseConnection.UpdateAsync(entity, columns);
                return 0;
            }
        }

        public async Task<object> insert(T entity)
        {
            using (var databaseConnection = createConnection())
            {
                if (!await exists(entity))
                    return await databaseConnection.InsertAsync(entity);
                return 0;
            }
        }

        public async Task<bool> exists(T entity)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.ExistsAsync(entity);
            }
        }

        public async Task<object> save(T entity)
        {
            if(await exists(entity))
                return await update(entity);
            return await insert(entity);
        }
    }
}
