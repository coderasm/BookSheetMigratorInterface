using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration.AwgToHoldingTable
{
    public class SellerDealerIdInserterByCompanyName : IdInserter<DealerDTO>
    {
        public SellerDealerIdInserterByCompanyName(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        protected override bool entityArgumentsExist()
        {
            return !string.IsNullOrEmpty(transaction.sellerCompanyName);
        }

        protected override object[] getEntityArguments()
        {
            return new object[]
            {
                transaction.sellerCompanyName
            };
        }

        protected override string getNameInTransaction()
        {
            return transaction.sellerCompanyName;
        }

        protected override string getEntityName(DealerDTO dealer)
        {
            return dealer.companyName;
        }

        protected override bool insertingBuyerDealerId()
        {
            return false;
        }

        protected override bool hasAtLeastOneContact(List<DealerDTO> possibleEntities)
        {
            return false;
        }

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByCompanyName((string)entityArguments[0]);
            return await entitiesFinder.find();
        }

        protected override void setIdFromFirstFoundEntity(DealerDTO entity)
        {
            transaction.sellerDealerId = entity.dealerId;
        }
    }
}
