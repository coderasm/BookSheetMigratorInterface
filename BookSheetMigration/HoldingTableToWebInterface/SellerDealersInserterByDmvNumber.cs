using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class SellerDealersInserterByDmvNumber : CollectionInserter<DealerDTO>
    {
        public SellerDealersInserterByDmvNumber(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        protected override bool entityArgumentsExist()
        {
            return !string.IsNullOrEmpty(transaction.sellerDmvNumber);
        }

        protected override object[] getEntityArguments()
        {
            return new object[]
            {
                transaction.sellerDmvNumber
            };
        }

        protected override async Task<List<DealerDTO>> findEntities(object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByDmvNumber((string)entityArguments[0]);
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
