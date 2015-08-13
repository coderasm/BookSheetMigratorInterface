using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class MultipleMigrator
    {
        private DateTime beginSoldDate;
        private DateTime endSoldDate;

        public async Task<List<AWGTransactionDTO>> migrate()
        {
            await migrateEvents();
            return await migrateTransactions();
        }

        public async Task<List<AWGTransactionDTO>> migrate(DateTime beginSoldDate, DateTime endSoldDate)
        {
            await migrateEvents();
            return await migrateTransactions(beginSoldDate, endSoldDate);
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

        private Task<List<AWGTransactionDTO>> migrateTransactions(DateTime beginSoldDate, DateTime endSoldDate)
        {
            return Task.Run(async () =>
            {
                DataMigrator<AWGTransactionDTO> transactionMigrator = new BookSheetTransactionMigratorByDateRange(beginSoldDate, endSoldDate, new TransactionIdsInserter());
                return await transactionMigrator.migrate();
            });
        }

        private Task<List<AWGTransactionDTO>> migrateTransactions()
        {
            return Task.Run(async () =>
            {
                DataMigrator<AWGTransactionDTO> transactionMigrator = new BookSheetTransactionMigrator(new TransactionIdsInserter());
                return await transactionMigrator.migrate();
            });
        }
    }
}
