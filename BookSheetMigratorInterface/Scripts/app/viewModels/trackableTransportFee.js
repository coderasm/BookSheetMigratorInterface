define(['app/viewModels/trackableObservable'], function (TrackableObservable) {
    function TrackableTransportFee(transportFee, tracker) {
        var self = this;
        TrackableObservable.call(self, "transportFee", transportFee, tracker);
    }
    TrackableTransportFee.prototype = Object.create(TrackableObservable.prototype);
    TrackableTransportFee.prototype.customValidation = function (transportFee) {
        return parseInt(transportFee) >= 0;
    }
    return TrackableTransportFee;
});