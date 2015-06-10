using System;
using System.Xml.Serialization;
using AsyncPoco;

namespace BookSheetMigration
{
    [TableName(Settings.ABSBookSheetEventTable)]
    [PrimaryKey("EventId", autoIncrement = false)]
    public class AWGEventDTO
    {
        [XmlElement("EventId")]
        [Column("EventId")]
        public int eventId { get; set; }

        [XmlElement("EndTime")]
        [Column("EndTime")]
        public DateTime endTime { get; set; }

        public override bool Equals(object obj)
        {
            AWGEventDTO eventDto = obj as AWGEventDTO;
            if (eventDto == null)
                return false;
            return eventId == eventDto.eventId;
        }

        public bool Equals(AWGEventDTO eventDto)
        {
            if (eventDto == null)
                return false;
            return eventId == eventDto.eventId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
