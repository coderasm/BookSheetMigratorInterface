define(['knockout', 'transactionViewModel'], function (ko) {
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

        self.pauseAllUpdateListeners = function () {
            ko.utils.arrayForEach(self.transactions, function (transaction) {
                transaction.update.pause();
            });
        }

        self.resumeAllUpdateListeners = function () {
            ko.utils.arrayForEach(self.transactions, function (transaction) {
                transaction.update.resume();
            });
        }

        self.pullNewAndUpdateExistingTransactions = function () {

        }

        function getAllTransactions() {
            ajaxHelper(transactionUri + "unimported", 'GET').done(function (data) {
                var mappedTransactions = $.map(data, function (item) {
                    var transaction = new Transaction(item, transactionUri);
                    self.attachUpdater(transaction);
                    return transaction;
                });
                self.transactions(mappedTransactions);
            });
        }

        self.attachUpdater = function (transaction) {
            transaction.update = ko.pauseableComputed(function () {
                if (!transaction.firstTimeLoading) {
                    $.ajax({
                        url: transaction.transactionUri + "update",
                        type: 'PUT',
                        data: {
                            eventId: transaction.eventId,
                            transactionId: transaction.transactionId,
                            bidAmount: transaction.bidAmount(),
                            soldDate: transaction.soldDate(),
                            sellerDealerId: transaction.sellerDealerId(),
                            buyerDealerId: transaction.buyerDealerId(),
                            buyerContactId: transaction.buyerContactId
                        }
                    });
                }
                transaction.firstTimeLoading = false;
            }).extend({ rateLimit: 0 });
        }

        // Fetch the initial data.
        getAllTransactions();
    }
});