
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration
{
    public class BuyerContactsInserter : ContactCollectionInserter
    {
        public BuyerContactsInserter(DealerDTO buyer)
        {
            dealer = buyer;
        }

        protected override bool entityArgumentsExist()
        {
            return dealer.dealerId != null;
        }

        protected override string getEntityNumber()
        {
            return dealer.dealerId;
        }

        protected override async Task<List<DealerContactDTO>> findEntities(string entityNumber)
        {
            var entitiesFinder = new DealerContactsFinder(entityNumber);
            return await entitiesFinder.find();
        }

        protected override void setPossibleCollection(List<DealerContactDTO> entity)
        {
            dealer.contacts = entity;
        }
    }
}
