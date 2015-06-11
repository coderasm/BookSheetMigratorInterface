﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BookSheetMigration.AwgToHoldingTable;

namespace BookSheetMigration.HoldingTableToWebInterface
{
    public class BuyerDealersInserterByCompanyName : DealerCollectionInserter
    {
        public BuyerDealersInserterByCompanyName(AWGTransactionDTO transaction)
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

        protected override async Task<List<DealerDTO>> findEntities(params object[] entityArguments)
        {
            var entitiesFinder = new DealersFinderByPhoneNumber((string)entityArguments[0]);
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
