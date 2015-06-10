using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class BuyerDealersInserterByDmvNumber : CollectionInserter<DealerDTO>
    {
        public BuyerDealersInserterByDmvNumber(AWGTransactionDTO transaction)
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

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByDmvNumber((string)entityArguments[0]);
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

        protected override bool hasAtLeastOneContact(List<DealerDTO> possibleCollectionOfEntities)
        {
            var contactFinder = new DealerContactsFinder(possibleCollectionOfEntities[0].dealerId);
            return contactFinder.find().Result.Count > 0;
        }
    }
}
