using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class SellerDealersInserterByAddressAndCity : CollectionInserter<DealerDTO>
    {
        public SellerDealersInserterByAddressAndCity(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        protected override bool entityArgumentsExist()
        {
            return !string.IsNullOrEmpty(transaction.sellerAddress) && !string.IsNullOrEmpty(transaction.sellerCity);
        }

        protected override object[] getEntityArguments()
        {
            return new object[]
            {
                transaction.sellerAddress,
                transaction.sellerCity
            };
        }

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByAddressAndCity((string)entityArguments[0], (string)entityArguments[1]);
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
