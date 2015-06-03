define(['knockout'], function (ko) {
    function TrackableObservable(name, dataItem, tracker) {
        var self = this;
        self.name = name;
        self.dataItem = ko.observable(dataItem);
        self.initialValue = dataItem;
        self.dataItem.subscribe(function (value) {
            var indexInTracker = attributeIndexInTracker();
            if (doesValidate(value))
                addOrUpdateTracker(value, indexInTracker);
            else
                removeFromTracker(indexInTracker);
        });

        function attributeIndexInTracker() {
            var currentIndex = 0;
            var attributeIndex = -1;
            tracker().every(function (attribute) {
                if (attribute.name === self.name) {
                    attributeIndex = currentIndex;
                    return false;
                }
                currentIndex++;
                return true;
            });
            return attributeIndex;
        }

        function doesValidate(value) {
            var result = !isNullOrEmpty(value) && !isInitialValue(value) && self.customValidation(value);
            return result;
        }

        function isNullOrEmpty(value) {
            return value === null || value == "";
        }

        function isInitialValue(value) {
            return value == self.initialValue;
        }

        function addOrUpdateTracker(value, indexInTracker) {
            if (foundInTracker(indexInTracker))
                updateInTracker(indexInTracker, value);
            else
                addToTracker(value);
        }


        function foundInTracker(indexInTracker) {
            return indexInTracker > -1;
        }

        function updateInTracker(indexInTracker, value) {
            tracker()[indexInTracker] = {
                name: self.name,
                value: value
            };
        }

        function addToTracker(value) {
            tracker.push({
                name: self.name,
                value: value
            });
        }

        function removeFromTracker(indexInTracker) {
            if (foundInTracker(indexInTracker))
                tracker.splice(indexInTracker, 1);
        }
    }

    TrackableObservable.prototype.name = "";
    TrackableObservable.prototype.dataItem = ko.observable();
    TrackableObservable.prototype.initialValue = "";
    TrackableObservable.prototype.customValidation = function (value) {
        return true;
    }

    return TrackableObservable;
});