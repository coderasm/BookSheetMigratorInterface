﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookSheetMigration.AwgToHoldingTable
{
    class SellerDealerIdInserterByAddressAndCity : DealerIdInserter
    {
        public SellerDealerIdInserterByAddressAndCity(AWGTransactionDTO transaction)
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
            var entitiesFinder = new DealersFinderByAddressAndCity((string)entityArguments[0], (string)entityArguments[1]);
            return await entitiesFinder.find();
        }

        public override void setIdFromFirstFoundEntity(DealerDTO entity)
        {
            transaction.sellerDealerId = entity.dealerId;
        }
    }
}
