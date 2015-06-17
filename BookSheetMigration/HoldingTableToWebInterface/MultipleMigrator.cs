using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class MultipleMigrator
    {
        public async Task<List<AWGTransactionDTO>> migrate()
        {
            await migrateEvents();
            return await migrateTransactions();
        }

        private Task migrateEvents()
        {
            return Task.Run(() =>
            {
                DataMigrator<AWGEventDTO> upcomingEventMigrator = new BookSheetEventMigrator(EventStatus.Upcoming);
                upcomingEventMigrator.migrate();
                DataMigrator<AWGEventDTO> inprogressEventMigrator = new BookSheetEventMigrator(EventStatus.InProgress);
                inprogressEventMigrator.migrate();
            });
        }

        private Task<List<AWGTransactionDTO>> migrateTransactions()
        {
            return Task.Run(() =>
            {
                DataMigrator<AWGTransactionDTO> transactionMigrator = new BookSheetTransactionMigrator();
                return transactionMigrator.migrate();
            });
        }
    }
}
