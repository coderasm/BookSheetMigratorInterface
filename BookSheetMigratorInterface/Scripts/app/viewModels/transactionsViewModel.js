define(['knockout', 'app/viewModels/transactionViewModel', 'app/bindings/isLoadingWhen'], function (ko, Transaction) {
    return function transactionsViewModel() {
        var self = this;
        self.transactions = ko.observableArray();
        self.error = ko.observable();
        self.hasTransactions = ko.computed(function () {
            return self.transactions().length > 0;
        });

        self.fadeIn = function (element) {
            $(element).hide();
            $(element).fadeIn(1000);
        }

        var transactionUri = '/api/Transaction/';

        function ajaxHelper(uri, method, data) {
            self.error('');
            return $.ajax({
                type: method,
                url: uri,
                dataType: 'json',
                contentType: 'application/json',
                data: data ? JSON.stringify(data) : null
            }).fail(function (jqXHR, textStatus, errorThrown) {
                self.error(errorThrown);
            });
        }

        self.importAll = function () {
            ko.utils.arrayForEach(self.transactions, function (transaction) {
                if (transaction.importable())
                    transaction.import();
            });
        }

        self.pullNewAndUpdateExistingTransactions = function () {

        }

        function getAllTransactions() {
            ajaxHelper(transactionUri + "unimported", 'GET').done(function (data) {
                var mappedTransactions = $.map(data, function (item) {
                    return new Transaction(item, transactionUri);
                });
                self.transactions(mappedTransactions);
            });
        }

        // Fetch the initial data.
        getAllTransactions();
    }
});