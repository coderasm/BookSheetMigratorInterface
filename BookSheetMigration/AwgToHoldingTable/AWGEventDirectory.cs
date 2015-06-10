using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookSheetMigration
{
    [Serializable, XmlRoot("AWGDataSet")]
    public class AWGEventDirectory
    {
        [XmlElement("Event")]
        public List<AWGEventDTO> awgEvents = new List<AWGEventDTO>();
    }
}
