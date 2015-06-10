using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace BookSheetMigration
{
    [Serializable, XmlRoot("AWGDataSet")]
    public class AWGTransactionDirectory
    {
        [XmlElement("Transaction")]
        public List<AWGTransactionDTO> transactions = new List<AWGTransactionDTO>(); 
    }
}
