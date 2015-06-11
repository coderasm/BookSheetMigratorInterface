using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class BuyerDealersInserterByAddressAndCity : DealerCollectionInserter
    {
        public BuyerDealersInserterByAddressAndCity(AWGTransactionDTO transaction)
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

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByAddressAndCity((string)entityArguments[0], (string)entityArguments[1]);
            return await entitiesFinder.find();
        }

        protected override void setPossibleCollection(List<DealerDTO> entity)
        {
            transaction.buyers = entity;
        }

        protected override bool insertingBuyersCollection()
        {
            return true;
        }
    }
}
