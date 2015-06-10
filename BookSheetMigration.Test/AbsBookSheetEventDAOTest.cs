using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookSheetMigration.Test
{
    [TestClass]
    public class AbsBookSheetEventDAOTest
    {
        [TestMethod]
        public void testFindingAlreadyMigratedEvents()
        {
            var myEvent = new AWGEventDTO();
            myEvent.eventId = 21229;
            myEvent.endTime = DateTime.Now;

            DataMigrator<AWGEventDTO> bookSheetUpcomingEventMigrator = new BookSheetEventMigrator(EventStatus.Upcoming);
            var migrationTask = bookSheetUpcomingEventMigrator.migrateRecord(myEvent);
            migrationTask.Wait();
            Assert.IsTrue(migrationTask.IsCompleted);
        }
    }
}
