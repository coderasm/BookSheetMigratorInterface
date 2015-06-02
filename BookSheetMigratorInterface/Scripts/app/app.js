define(['jquery', 'underscore', 'knockout', 'app/viewModels/transactionsViewModel', 'domReady!'], function ($, _, ko, transactionsViewModel) {
    var App = function () { };

    _.extend(App.prototype, {
        init: function () {
            console.log("Init...");
            ko.applyBindings(new transactionsViewModel());
        }
    });

    return new App();
});