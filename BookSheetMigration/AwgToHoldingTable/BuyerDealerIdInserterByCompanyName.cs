﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration.AwgToHoldingTable
{
    public class BuyerDealerIdInserterByCompanyName : DealerIdInserter
    {
        public BuyerDealerIdInserterByCompanyName(AWGTransactionDTO transaction)
        {
            this.transaction = transaction;
        }

        protected override bool entityArgumentsExist()
        {
            return !string.IsNullOrEmpty(transaction.buyerCompanyName);
        }

        protected override object[] getEntityArguments()
        {
            return new object[]
            {
                transaction.buyerCompanyName
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

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByCompanyName((string)entityArguments[0]);
            return await entitiesFinder.find();
        }

        protected override void setIdFromFirstFoundEntity(DealerDTO entity)
        {
            transaction.buyerDealerId = entity.dealerId;
        }
    }
}
