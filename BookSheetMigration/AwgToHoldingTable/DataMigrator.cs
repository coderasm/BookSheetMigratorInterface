using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public abstract class DataMigrator<T>
    {
        protected List<T> possiblyNewRecords;
        protected EntityDAO<T> entityDao;
        private Task[] runningTasks;

        protected abstract List<T> findPossiblyNewRecords();

        public DataMigrator()
        {
            entityDao = new EntityDAO<T>();
        } 

        private bool possibleRecordsToMigrateExist(List<T> possiblyNewRecords)
        {
 	        return possiblyNewRecords.Count > 0;
        }

        protected void migrateRecords()
        {
            possiblyNewRecords = findPossiblyNewRecords();
            if (possibleRecordsToMigrateExist(possiblyNewRecords))
            {
                initializeRunningTasks(possiblyNewRecords.Count);
                createAndSaveMigrationTasks();
                Task.WaitAll(runningTasks);
            }
        }

        private void initializeRunningTasks(int recordCount)
        {
            runningTasks = new Task[recordCount];
        }

        private void createAndSaveMigrationTasks()
        {
            for (int i = 0; i < possiblyNewRecords.Count; i++)
            {
                var runningTask = migrateRecord(possiblyNewRecords[i]);
                runningTasks[i] = runningTask;
            }
        }

        public Task<object> migrateRecord(T possiblyNewRecord)
        {
            return entityDao.insert(possiblyNewRecord);
        }

        public void migrate()
        {
            migrateRecords();
        }
    }
}
