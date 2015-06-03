define(['app/viewModels/trackableObservable'], function (TrackableObservable) {
    function TrackableSoldDate(soldDate, tracker) {
        var self = this;
        TrackableObservable.call(self, "soldDate", soldDate, tracker);
    }

    TrackableSoldDate.prototype = Object.create(TrackableObservable.prototype);
    TrackableSoldDate.prototype.customValidation = function (soldDate) {
        var oneWeekAgo = returnDateOneWeekAgo();
        return new Date(soldDate) - oneWeekAgo > 0;
    }

    function returnDateOneWeekAgo() {
        var today = Date.now();
        var millisecondsInADay = 24 * 60 * 60 * 1000;
        var millisecondsInAWeek = 7 * millisecondsInADay;
        return new Date(today.getTime() - millisecondsInAWeek);
    }

    return TrackableSoldDate;
});