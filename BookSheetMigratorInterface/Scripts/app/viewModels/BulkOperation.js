define(['knockout'], function(ko) {
    function BulkOperation() {
    }

    BulkOperation.prototype.selectedItems = [];

    BulkOperation.prototype.execute = function () {
        if (this.selectedItems.length === 0)
            return;
        var processableItems = this.findProcessable();
        this.processItems(processableItems);
    }

    BulkOperation.prototype.findProcessable = function () {
        var self = this;
        var itemsToProcess = [];
        ko.utils.arrayForEach(this.selectedItems, function (item) {
            if (self.isProcessable(item)) {
                itemsToProcess.push(self.returnItem(item));
            }
        });
        return itemsToProcess;
    }

    BulkOperation.prototype.isProcessable = function(item) {
        return false;
    }

    BulkOperation.prototype.returnItem = function (item) {
        return {};
    }

    BulkOperation.prototype.processItems = function (items) {
        if (items.length === 0)
            return;
        this.beforeProcessing();
        this.doProcessing(items);
        this.afterProcessing();
    }

    BulkOperation.prototype.beforeProcessing = function () {
    }

    BulkOperation.prototype.doProcessing = function (items) {
    }

    BulkOperation.prototype.afterProcessing = function () {
    }

    return BulkOperation;
});