define(['app/viewModels/bulkAjaxOperation'], function (BulkAjaxOperation) {
    BulkUpdateAjaxOperation.prototype = new BulkAjaxOperation();

    function BulkUpdateAjaxOperation(selectedItems, viewModel) {
        var self = this;
        self.operation = "bulk-update";
        self.method = "PUT";
        self.selectedItems = selectedItems;
        self.viewModel = viewModel;
        self.isProcessable = function(item) {
            return item.updateable() && item.isValidUpdate();
        }
        self.returnItem = function(item) {
            return {
                eventId: item.eventId,
                transactionId: item.transactionId,
                sellerDealerId: item.sellerDealerId(),
                buyerDealerId: item.buyerDealerId(),
                buyerContactId: item.buyerContactId(),
                bidAmount: item.bidAmount(),
                transportFee: item.transportFee(),
                soldDate: item.soldDate(),
                feeException: item.feeException()
            }
        }
        self.onAjaxSuccess = function (data) {
            BulkUpdateAjaxOperation.prototype.onAjaxSuccess.call(this);
            self.viewModel.showUpdateMessages(data);
        }
    }

    return BulkUpdateAjaxOperation;
});