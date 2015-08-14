using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncPoco;

namespace BookSheetMigration
{
    public class EntityDAO<T>
    {
        public Database database { get; private set; }

        public EntityDAO(Database database)
        {
            this.database = database;
        }

        public async Task<ITransaction> GetTransaction()
        {
            return await database.GetTransactionAsync();
        }

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

        public async Task<List<T>> selectShared(Sql sql)
        {
            return await database.FetchAsync<T>(sql);
        }

        public async Task<List<T>> selectShared(string sql)
        {
            return await database.FetchAsync<T>(sql);
        }

        public async Task<List<T>> selectShared(string sql, params object[] sqlparams)
        {
            return await database.FetchAsync<T>(sql, sqlparams);
        }

        public async Task<int> update(T entity)
        {
            using (var databaseConnection = createConnection())
            {
                 return await databaseConnection.UpdateAsync(entity);
            }
        }

        public async Task<int> update(T entity, IEnumerable<string> columns)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.UpdateAsync(entity, columns);
            }
        }

        public async Task<int> updateShared(T entity)
        {
            return await database.UpdateAsync(entity);
        }

        public async Task<int> updateShared(T entity, IEnumerable<string> columns)
        {
            return await database.UpdateAsync(entity, columns);
        }

        public async Task<object> insert(T entity)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.InsertAsync(entity);
            }
        }

        public async Task<object> insertShared(T entity)
        {
            return await database.InsertAsync(entity);
        }

        public async Task<int> delete(T entity)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.DeleteAsync(entity);
            }
        }

        public async Task<int> deleteShared(T entity)
        {
            return await database.DeleteAsync(entity);
        }

        public async Task<List<T>> mergeInsertOnly(List<T> pocos, int batchSize = 25)
        {
            using (var databaseConnection = createConnection())
            {
                return await databaseConnection.MergeInsertOnly(pocos, batchSize);
            } 
        }

        public async Task<List<T>> mergeInsertOnlyShared(List<T> pocos, int batchSize = 25)
        {
            return await database.MergeInsertOnly(pocos, batchSize);
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
