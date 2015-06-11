namespace BookSheetMigration
{
    public class DealersFinder : EntitiesFinder<DealerDTO>
    {
        protected const string dealerQuery = @"SELECT DISTINCT c1.ACCOUNTNO, COMPANY
                                                FROM ABSContact..CONTACT2 c2
                                                JOIN ABSContact..CONTACT1 c1 ON c1.ACCOUNTNO = c2.ACCOUNTNO
                                                WHERE COMPANY IS NOT NULL AND
                                                    COMPANY <> '' AND
                                                    COMPANY NOT LIKE '%- old%' AND
                                                    COMPANY NOT LIKE '%- dup%' AND
                                                    COMPANY NOT LIKE '%-old%' AND
                                                    COMPANY NOT LIKE '%-dup%' AND
                                                    COMPANY NOT LIKE '%old record%' AND
                                                    COMPANY NOT LIKE '%duplicate%' AND
                                                    COMPANY NOT LIKE '%(dup%)%' AND
                                                    COMPANY NOT LIKE '%(old%)%' AND
                                                    COMPANY NOT LIKE '%- __' AND
                                                    COMPANY NOT LIKE '%-__' AND
                                                    UPPER(COMPANY) <> COMPANY COLLATE SQL_Latin1_General_CP1_CS_AS";

        public DealersFinder(string queryPart)
        {
            query = dealerQuery + queryPart;
        }
    }
}
