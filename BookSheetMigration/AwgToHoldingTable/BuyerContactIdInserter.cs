using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class BuyerContactIdInserter : ContactIdInserter
    {
        public BuyerContactIdInserter(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        protected override bool entityArgumentsExist()
        {
            return transaction.buyerDealerId != null;
        }

        protected override object[] getEntityArguments()
        {
            return new object[]
            {
                transaction.buyerDealerId
            };
        }

        protected override string getNameInTransaction()
        {
            return transaction.buyerFirstName + " " + transaction.buyerLastName;
        }

        protected override string getEntityName(DealerContactDTO contact)
        {
            return contact.name;
        }

        protected override async Task<List<DealerContactDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealerContactsFinder((string)entityArguments[0]);
            return await entitiesFinder.find();
        }

        protected override void setIdFromFoundEntity(DealerContactDTO entity)
        {
            transaction.buyerContactId = entity.contactId;
        }
    }
}
