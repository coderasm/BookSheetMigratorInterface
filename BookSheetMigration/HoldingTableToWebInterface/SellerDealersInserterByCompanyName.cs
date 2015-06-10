using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class SellerDealersInserterByCompanyName : CollectionInserter<DealerDTO>
    {
        public SellerDealersInserterByCompanyName(AWGTransactionDTO transaction)
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

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByCompanyName((string)entityArguments[0]);
            return await entitiesFinder.find();
        }

        protected override void setPossibleCollection(List<DealerDTO> entity)
        {
            transaction.sellers = entity;
        }

        protected override bool insertingBuyersCollection()
        {
            return false;
        }

        protected override bool hasAtLeastOneContact(List<DealerDTO> possibleCollectionOfEntities)
        {
            return false;
        }
    }
}
