define(['knockout'], function(ko) {
    return function TransactionObservable(name, dataItem, tracker) {
        var self = this;
        self.dataItem = ko.observable(dataItem);
        self.dataItem.subscribe(function(value) {
            if (doesValidate(value))
                addToTracker(value);
            else
                removeFromTracker();
        });
        self.initialValue = dataItem;
        self.tracker = tracker;

        function doesValidate(value) {
            return !isNullOrEmpty(value) && !isInitialValue(value) && customValidation(value);
        }

        function isNullOrEmpty(value) {
            return value === null || value === "";
        }

        function isInitialValue(value) {
            return value === self.initialValue;
        }

        self.prototype.customValidation = function(value) {
            return true;
        }

        function addToTracker(value) {
            tracker[name] = value;
        }

        function removeFromTracker() {
            delete tracker[name];
        }
    }
});