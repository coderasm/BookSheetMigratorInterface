define(['app/viewModels/bulkAjaxOperation'], function(BulkAjaxOperation) {
    BulkImportAjaxOperation.prototype = new BulkAjaxOperation();

    function BulkImportAjaxOperation(selectedItems, viewModel) {
        var self = this;
        self.operation = "import";
        self.method = "POST";
        self.initialize(selectedItems, viewModel);
        self.isProcessable = function (item) {
            return item.importable() && item.isValidImport();
        }
        self.returnItem = function (item) {
            return {
                eventId: item.eventId,
                transactionId: item.transactionId
            }
        }
        self.onAjaxSuccess = function (data) {
            BulkImportAjaxOperation.prototype.onAjaxSuccess.call(this);
            self.viewModel.isNotImporting(true);
            self.viewModel.showImportMessagesAndRemove(data);
        }
        self.beforeProcessing = function () {
            BulkImportAjaxOperation.prototype.beforeProcessing.call(this);
            self.viewModel.isNotImporting(false);
        }
    }

    return BulkImportAjaxOperation;
});