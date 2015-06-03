define(['app/viewModels/trackableObservable'], function (TrackableObservable) {
    function TrackableId(name, id, tracker) {
        var self = this;
        TrackableObservable.call(self, name, id, tracker);
    }
    TrackableId.prototype = Object.create(TrackableObservable.prototype);
    return TrackableId;
});