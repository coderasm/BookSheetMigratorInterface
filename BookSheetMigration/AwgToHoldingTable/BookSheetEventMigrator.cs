using System.Collections.Generic;

namespace BookSheetMigration
{
    public class BookSheetEventMigrator : DataMigrator<AWGEventDTO>
    {
        protected EventStatus eventStatus;

        public BookSheetEventMigrator(EventStatus eventStatus)
        {
            this.eventStatus = eventStatus;
        }

        protected override List<AWGEventDTO> findPossiblyNewRecords()
        {
            var awgServiceClient = new AWGServiceClient();
            return awgServiceClient.findEventsByStatus(eventStatus);
        }
    }
}
