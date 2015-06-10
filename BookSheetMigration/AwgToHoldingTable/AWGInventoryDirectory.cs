using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookSheetMigration
{
    [Serializable, XmlRoot("AWGDataSet")]
    public class AWGInventoryDirectory
    {
        [XmlElement("Inventory")]
        public List<AWGInventoryDTO> inventory = new List<AWGInventoryDTO>(); 
    }
}
