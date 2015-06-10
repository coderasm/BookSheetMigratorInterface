using AsyncPoco;

namespace BookSheetMigration
{
    public class DatabaseFactory
    {

        internal static Database makeDatabase()
        {
            var database = new Database(Settings.ABSProductionDbConnectionString, Settings.ABSDatabaseProviderName);
            database.CommandTimeout = 120;
            return database;
        }
    }
}
