define(['knockout'], function (ko) {
    ko.extenders.trackChange = function (target, track) {
        if (track) {
            target.isDirty = ko.observable(false);
            target.originalValue = target();
            target.subscribe(function (newValue) {
                // use != not !== so numbers will equate naturally
                target.isDirty(newValue != target.originalValue);
            });
        }
        return target;
    };
});