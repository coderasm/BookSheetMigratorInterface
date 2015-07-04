define(['app/viewModels/bulkOperation'], function (BulkOperation) {
    BulkAjaxOperation.prototype = new BulkOperation();

    function BulkAjaxOperation() {
        var self = this;
        self.doProcessing = function (items) {
            BulkAjaxOperation.prototype.doProcessing.call(this);
            this.makeAjaxCall(items);
        }
    }

    BulkAjaxOperation.prototype.makeAjaxCall = function (items) {
        var self = this;
        $.ajax({
            url: self.viewModel.transactionUri + self.operation,
            type: self.method,
            data: JSON.stringify(items),
            dataType: 'json',
            contentType: 'application/json',
            success: function (data) {
                self.onAjaxSuccess(data);
            }
        });
    }
    BulkAjaxOperation.prototype.operation = "";
    BulkAjaxOperation.prototype.method = "";
    BulkAjaxOperation.prototype.onAjaxSuccess = function(data) {
    }

    return BulkAjaxOperation;
});