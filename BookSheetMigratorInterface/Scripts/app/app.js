define(['jquery', 'underscore', 'knockout',
    'app/viewModels/transactionsViewModel',
    'domReady!'],
    function ($, _, ko, transactionsViewModel) {
        var app = function () { };

        _.extend(app.prototype, {
            init: function () {
                console.log("Init...");
                ko.applyBindings(new transactionsViewModel());
            }
        });

        return new app();
    }
);