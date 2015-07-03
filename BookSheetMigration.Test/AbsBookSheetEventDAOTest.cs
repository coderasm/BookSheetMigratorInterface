using System;
using System.Collections.Generic;
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
            var migratedEvents = bookSheetUpcomingEventMigrator.migrate(new List<AWGEventDTO>(){myEvent}).Result;
            Assert.AreEqual(1, migratedEvents.Count);
        }
    }
}
