using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class DataMigrator<T>
    {
        protected List<T> possiblyNewRecords;
        protected EntityDAO<T> entityDao;

        protected abstract List<T> findPossiblyNewRecords();

        public DataMigrator()
        {
            entityDao = new EntityDAO<T>(DatabaseFactory.makeDatabase());
        } 

        private bool possibleRecordsToMigrateExist(List<T> possiblyNewRecords)
        {
 	        return possiblyNewRecords.Count > 0;
        }

        protected async Task<List<T>> migrateRecords(List<T> foundRecords)
        {
            if (possibleRecordsToMigrateExist(foundRecords))
            {
                try
                {
                    using (var scope = await entityDao.GetTransaction())
                    {
                        await doBeforeMigration();
                        var insertedEntities = await entityDao.mergeInsertOnlyShared(foundRecords, Settings.migrationBatchSize);
                        await doAfterMigration();
                        scope.Complete();
                        return insertedEntities;
                    }
                }
                catch (SqlException exception)
                {
                    Debug.WriteLine("Migration failed: " + exception.Message);
                    return new List<T>();
                }
            }
            return new List<T>();
        }

        protected virtual Task doAfterMigration()
        {
            return Task.Run(() => { });
        }

        protected virtual Task doBeforeMigration()
        {
            return Task.Run(() => { });
        }

        public async Task<List<T>> migrate()
        {
            possiblyNewRecords = findPossiblyNewRecords();
            return await migrateRecords(possiblyNewRecords);
        }

        public async Task<List<T>> migrate(List<T> foundRecords)
        {
            return await migrateRecords(foundRecords);
        }
    }
}
