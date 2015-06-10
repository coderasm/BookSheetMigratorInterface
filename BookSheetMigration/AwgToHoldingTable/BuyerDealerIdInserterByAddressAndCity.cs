using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration.AwgToHoldingTable
{
    class BuyerDealerIdInserterByAddressAndCity : IdInserter<DealerDTO>
    {
        public BuyerDealerIdInserterByAddressAndCity(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        protected override bool entityArgumentsExist()
        {
            return !string.IsNullOrEmpty(transaction.buyerAddress) && !string.IsNullOrEmpty(transaction.buyerCity);
        }

        protected override object[] getEntityArguments()
        {
            return new object[]
            {
                transaction.buyerAddress,
                transaction.buyerCity
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
            var entitiesFinder = new DealersFinderByAddressAndCity((string)entityArguments[0], (string)entityArguments[1]);
            return await entitiesFinder.find();
        }

        protected override void setIdFromFirstFoundEntity(DealerDTO entity)
        {
            transaction.buyerDealerId = entity.dealerId;
        }
    }
}
