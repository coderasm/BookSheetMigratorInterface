using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigration.Test
{
    [TestClass]
    public class AWGEventMigratorTest
    {
        [TestMethod]
        public void testMigrationOfEvents()
        {
            DataMigrator<AWGEventDTO> upcomingEventMigrator = new BookSheetEventMigrator(EventStatus.Upcoming);
            upcomingEventMigrator.migrate();
            DataMigrator<AWGEventDTO> inprogressEventMigrator = new BookSheetEventMigrator(EventStatus.InProgress);
            inprogressEventMigrator.migrate();
        }
    }
}
