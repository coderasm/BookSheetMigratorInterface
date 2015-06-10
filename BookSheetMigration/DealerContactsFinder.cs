
namespace BookSheetMigration
{
    public class DealerContactsFinder : EntitiesFinder<DealerContactDTO>
    {
        private const string contactQuery = @"SELECT DISTINCT cs.RECID, CONTACT, TITLE
                                              FROM ABSContact..CONTACT2 c2
	                                          LEFT JOIN ABSContact..CONTSUPP cs ON cs.ACCOUNTNO = c2.ACCOUNTNO AND cs.RECTYPE = 'C'
                                              WHERE cs.ACCOUNTNO = '{0}'";

        public DealerContactsFinder(string key)
        {
            query = returnFilledQueryPart(contactQuery, key);
        }
    }
}
