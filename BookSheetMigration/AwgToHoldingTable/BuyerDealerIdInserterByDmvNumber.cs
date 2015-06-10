using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class BuyerDealerIdInserterByDmvNumber : IdInserter<DealerDTO>
    {

        public BuyerDealerIdInserterByDmvNumber(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        protected override bool entityArgumentsExist()
        {
            return !string.IsNullOrEmpty(transaction.buyerDmvNumber);
        }

        protected override object[] getEntityArguments()
        {
            return new object[]
            {
                transaction.buyerDmvNumber
            };
        }

        protected override string getNameInTransaction()
        {
            return transaction.buyerCompanyName;
        }

        protected override string getEntityName(DealerDTO dealer)
        {
            return dealer.companyName;
        }

        protected override bool insertingBuyerDealerId()
        {
            return true;
        }

        protected override bool hasAtLeastOneContact(List<DealerDTO> possibleEntities)
        {
            var contactFinder = new DealerContactsFinder(possibleEntities[0].dealerId);
            return contactFinder.find().Result.Count > 0;
        }

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByDmvNumber((string)entityArguments[0]);
            return await entitiesFinder.find();
        }

        protected override void setIdFromFirstFoundEntity(DealerDTO entity)
        {
            transaction.buyerDealerId = entity.dealerId;
        }
    }
}