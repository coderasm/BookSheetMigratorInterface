define([
        'knockout', 'app/viewModels/transactionViewModel',
        'app/bindings/isLoadingWhen', 'app/bindings/pagination',
        'app/utilities/utilities', 'signalr.hubs'
],
        function (ko, Transaction) {
            return function transactionsViewModel() {
                var self = this;
                self.transactions = ko.observableArray([]);
                self.error = ko.observable();
                self.hasTransactions = ko.computed(function () {
                    return self.transactions().length > 0;
                });
                self.filter = ko.observable("");
                self.filteredTransactions = ko.computed(function() {
                    var filter = self.filter().toLowerCase();
                    if (!filter) {
                        return self.transactions();
                    } else {
                        return ko.utils.arrayFilter(self.transactions(), function(item) {
                            return item.vin.toLowerCase().contains(filter);
                        });
                    }
                });
                self.isLoadingTransactions = ko.observable(true);
                self.itemCount = ko.computed(function() { 
                    return self.transactions().length;
                }),
                self.perPage = ko.observable(10),
                self.pageIndex = ko.observable(0),
                self.page = ko.computed(function() {
                    var startingIndex = self.pageIndex() * self.perPage();
                    return self.filteredTransactions().slice(
                      startingIndex,                  
                      startingIndex + self.perPage());
                });

                self.fadeIn = function (element) {
                    $(element).hide();
                    $(element).fadeIn(500);
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

                self.selectAll = function () {
                    ko.utils.arrayForEach(self.page(), function (transaction) {
                        transaction.isSelected(true);
                    });
                }

                self.selectNone = function () {
                    ko.utils.arrayForEach(self.page(), function (transaction) {
                        transaction.isSelected(false);
                    });
                }

                self.importSelected = function () {
                    ko.utils.arrayForEach(self.transactions(), function (transaction) {
                        if (transaction.importable() && transaction.isSelected())
                            transaction.importSale();
                    });
                }

                self.updateSelected = function () {
                    ko.utils.arrayForEach(self.transactions(), function (transaction) {
                        if (transaction.updateable() && transaction.isSelected())
                            transaction.updateSale();
                    });
                }

                function getAllTransactions(payload) {
                    self.error('');
                    return $.ajax({
                        type: 'GET',
                        url: transactionUri + "unimported",
                        dataType: 'json',
                        contentType: 'application/json',
                        data: payload ? JSON.stringify(payload) : null,
                        success: function(data) {
                            var mappedTransactions = $.map(data, function (item) {
                                return new Transaction(item, transactionUri);
                            });
                            self.isLoadingTransactions(false);
                            self.transactions(mappedTransactions);
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown) {
                        self.error(errorThrown);
                    });
                }

                // Fetch the initial data.
                getAllTransactions();

                self.newlyAddedTransactions = [];
                //Wait for any new data.
                var ticker = $.connection.transactionTicker; // the generated client-side hub proxy
                ticker.client.consumeNewTransactions = function (transactions) {
                    var mappedTransactions = $.map(transactions, function (item) {
                        return new Transaction(item, transactionUri);
                    });
                    self.newlyAddedTransactions.push(mappedTransactions);
                }
                $.connection.hub.logging = true;
                $.connection.hub.start();
            }
});