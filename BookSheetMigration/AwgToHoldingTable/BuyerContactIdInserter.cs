using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public class BuyerContactIdInserter : IdInserter<DealerContactDTO>
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

        protected override bool insertingBuyerDealerId()
        {
            return false;
        }

        protected override bool hasAtLeastOneContact(List<DealerContactDTO> possibleEntities)
        {
            return false;
        }

        protected override async Task<List<DealerContactDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealerContactsFinder((string)entityArguments[0]);
            return await entitiesFinder.find();
        }

        protected override void setIdFromFirstFoundEntity(DealerContactDTO entity)
        {
            transaction.buyerContactId = entity.contactId;
        }
    }
}
