using AsyncPoco;

namespace BookSheetMigration
{
    public class DealerContactDTO
    {
        [Column("RECID")]
        public string contactId { get; set; }

        [Column("CONTACT")]
        public string name { get; set; }

        [Column("TITLE")]
        public string title { get; set; }
    }
}