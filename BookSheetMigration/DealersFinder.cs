namespace BookSheetMigration
{
    public class DealersFinder : EntitiesFinder<DealerDTO>
    {
        protected const string dealerQuery = @"SELECT DISTINCT c1.ACCOUNTNO, COMPANY
                                             FROM ABSContact..CONTACT2 c2
                                             JOIN ABSContact..CONTACT1 c1 ON c1.ACCOUNTNO = c2.ACCOUNTNO
                                             WHERE dbo.whoami(c2.ACCOUNTNO) NOT LIKE '%- old%' AND
                                                    dbo.whoami(c2.ACCOUNTNO) NOT LIKE '%- dup%' AND
                                                    dbo.whoami(c2.ACCOUNTNO) NOT LIKE '%(dup%)%' AND
                                                    dbo.whoami(c2.ACCOUNTNO) NOT LIKE '%- __' AND
                                                    dbo.whoami(c2.ACCOUNTNO) NOT LIKE '%-__'";

        public DealersFinder(string queryPart)
        {
            query = dealerQuery + queryPart;
        }
    }
}
