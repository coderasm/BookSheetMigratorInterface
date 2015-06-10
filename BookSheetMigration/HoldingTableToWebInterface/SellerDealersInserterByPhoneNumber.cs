using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class SellerDealersInserterByPhoneNumber : CollectionInserter<DealerDTO>
    {
        public SellerDealersInserterByPhoneNumber(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        protected override bool entityArgumentsExist()
        {
            return !string.IsNullOrEmpty(transaction.sellerPhone);
        }

        protected override object[] getEntityArguments()
        {
            return new object[]
            {
                transaction.sellerPhone
            };
        }

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityNumber)
        {
            var entitiesFinder = new DealersFinderByPhoneNumber((string)entityNumber[0]);
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
