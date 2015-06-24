
using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration
{
    public class SellerDealerIdInserterByDmvNumber : DealerIdInserter
    {
        public SellerDealerIdInserterByDmvNumber(AWGTransactionDTO transaction)
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

        public override string getNameInTransaction()
        {
            return transaction.sellerCompanyName;
        }

        public override string getEntityName(DealerDTO dealer)
        {
            return dealer.companyName;
        }

        protected override bool insertingBuyerDealerId()
        {
            return false;
        }

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByDmvNumber((string)entityArguments[0]);
            return await entitiesFinder.find();
        }

        public override void setIdFromFirstFoundEntity(DealerDTO dealer)
        {
            transaction.sellerDealerId = dealer.dealerId;
        }
    }
}