using AsyncPoco;

namespace BookSheetMigration
{
    public class DatabaseFactory
    {
        public static Database makeDatabase()
        {
            var database = new Database(Settings.ABSDbConnectionString, Settings.ABSDatabaseProviderName);
            database.CommandTimeout = 120;
            return database;
        }
    }
}
