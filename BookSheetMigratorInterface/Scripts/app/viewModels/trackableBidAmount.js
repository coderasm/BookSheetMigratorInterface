define(['app/viewModels/trackableObservable'], function (TrackableObservable) {
    function TrackableBidAmount(bidAmount, tracker) {
        var self = this;
        TrackableObservable.call(self, "bidAmount", bidAmount, tracker);
    }
    TrackableBidAmount.prototype = Object.create(TrackableObservable.prototype);
    TrackableBidAmount.prototype.customValidation = function (bidAmount) {
        return parseInt(bidAmount) >= 1000;
    }
    return TrackableBidAmount;
});