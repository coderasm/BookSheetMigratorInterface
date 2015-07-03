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
            return Task.Run(async () =>
            {
                DataMigrator<AWGEventDTO> upcomingEventMigrator = new BookSheetEventMigrator(EventStatus.Upcoming);
                await upcomingEventMigrator.migrate();
                DataMigrator<AWGEventDTO> inprogressEventMigrator = new BookSheetEventMigrator(EventStatus.InProgress);
                await inprogressEventMigrator.migrate();
            });
        }

        private Task<List<AWGTransactionDTO>> migrateTransactions()
        {
            return Task.Run(async () =>
            {
                DataMigrator<AWGTransactionDTO> transactionMigrator = new BookSheetTransactionMigrator();
                return await transactionMigrator.migrate();
            });
        }
    }
}
