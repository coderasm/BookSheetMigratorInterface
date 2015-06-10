using System.Collections.Generic;
using AsyncPoco;

namespace BookSheetMigration
{
    public class DealerDTO
    {
        [Column("ACCOUNTNO")]
        public string dealerId { get; set; }

        [Column("COMPANY")]
        public string companyName { get; set; }

        [Ignore]
        public List<DealerContactDTO> contacts { get; set; }
    }
}