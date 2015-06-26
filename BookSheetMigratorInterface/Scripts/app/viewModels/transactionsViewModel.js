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
                self.feeExceptions = [];
                self.filter = ko.observable("");
                self.showOnlyNew = ko.observable(false);
                self.isNotImporting = ko.observable(true);
                self.filteredTransactions = ko.computed(function() {
                    var filter = self.filter().toLowerCase();
                    if (!filter && !self.showOnlyNew()) {
                        return self.transactions();
                    } else {
                        var filteredTransactions = self.transactions();
                        if (self.showOnlyNew()) {
                            filteredTransactions = ko.utils.arrayFilter(filteredTransactions, function(item) {
                                                        return item.newlyAdded();
                                                    });
                        }
                        if (filter) {
                            filteredTransactions = ko.utils.arrayFilter(filteredTransactions, function(item) {
                                                        return item.vin.toLowerCase().contains(filter);
                                                    });
                        }
                        return filteredTransactions;
                    }
                });
                self.toggleShowOnlyNew = function() {
                    self.showOnlyNew(!self.showOnlyNew());
                }
                self.showOnlyNew = ko.observable(false);
                self.newCount = ko.computed(function() {
                    var newTransactions = ko.utils.arrayFilter(self.transactions(), function (transaction) {
                        return transaction.newlyAdded();
                    });
                    return newTransactions.length;
                });
                self.filterTypeCount = ko.computed(function () {
                    if (!self.showOnlyNew()) {
                        return self.newCount();
                    } else
                        return self.transactions().length;
                });
                self.filterTypeText = ko.computed(function() {
                    if (!self.showOnlyNew())
                        return "New";
                    else
                        return "All";
                });
                self.hasNew = ko.computed(function() {
                    return self.newCount() > 0;
                });
                self.isLoadingTransactions = ko.observable(true);
                self.itemCount = ko.computed(function() { 
                    return self.filteredTransactions().length;
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
                    $(element).fadeIn(250);
                }

                var transactionUri = '/api/Transaction/';

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
                    var selectedTransactions = findSelectedTransactions();
                    if (selectedTransactions.length === 0) {
                        return;
                    }
                    self.isNotImporting(false);
                    var importableTransactions = findImportableInSelected(selectedTransactions);
                    importTransactions(importableTransactions);
                }

                function findImportableInSelected(selectedTransactions) {
                    var transactionsToImport = [];
                    ko.utils.arrayForEach(selectedTransactions, function (transaction) {
                        if (transaction.importable() && transaction.isValidImport()) {
                            transactionsToImport.push(
                                {
                                    eventId: transaction.eventId,
                                    transactionId: transaction.transactionId
                                }
                            );
                        }
                    });
                    return transactionsToImport;
                }

                function importTransactions(transactions) {
                    if (transactions.length === 0)
                        return;
                    $.ajax({
                        url: transactionUri + "import",
                        type: "POST",
                        data: JSON.stringify(transactions),
                        dataType: 'json',
                        contentType: 'application/json',
                        success: function (data) {
                            self.isNotImporting(true);
                            showImportMessagesAndRemove(data);
                        }
                    });
                }

                function showImportMessagesAndRemove(results) {
                    results.forEach(function(data) {
                        self.transactions().some(function (transaction) {
                            if (transaction.equals(data)) {
                                transaction.showImportResultAndRemove(data.result, self.transactions);
                                return true;
                            }
                            return false;
                        });
                    });
                }

                self.updateSelected = function () {
                    var selectedTransactions = findSelectedTransactions();
                    var updateableTransactions = findUpdateableInSelected(selectedTransactions);
                    updateTransactions(updateableTransactions);

                }

                function findSelectedTransactions() {
                    return ko.utils.arrayFilter(self.transactions(), function (transaction) {
                        return transaction.isSelected();
                    });
                }

                function findUpdateableInSelected(selectedTransactions) {
                    var transactionsToUpdate = [];
                    ko.utils.arrayForEach(selectedTransactions, function (transaction) {
                        if (transaction.updateable() && transaction.isValidateUpdate()) {
                            transactionsToUpdate.push(
                                {
                                    eventId: transaction.eventId,
                                    transactionId: transaction.transactionId,
                                    sellerDealerId: transaction.sellerDealerId(),
                                    buyerDealerId: transaction.buyerDealerId(),
                                    buyerContactId: transaction.buyerContactId(),
                                    bidAmount: transaction.bidAmount(),
                                    transportFee: transaction.transportFee(),
                                    soldDate: transaction.soldDate(),
                                    feeException: transaction.feeException()
                                }
                            );
                        }
                    });
                    return transactionsToUpdate;
                }

                function updateTransactions(transactions) {
                    if (transactions.length === 0)
                        return;
                    $.ajax({
                        url: transactionUri + "bulk-update",
                        type: "PUT",
                        data: JSON.stringify(transactions),
                        dataType: 'json',
                        contentType: 'application/json',
                        success: function (data) {
                            showUpdateMessages(data);
                        }
                    });
                }

                function showUpdateMessages(results) {
                    results.forEach(function (data) {
                        self.transactions().some(function (transaction) {
                            if (transaction.equals(data)) {
                                transaction.showUpdateResult(data.result);
                                return true;
                            }
                            return false;
                        });
                    });
                }

                var ticker = $.connection.transactionTicker; // the generated client-side hub proxy

                function init() {
                    ticker.server.getUnimported()
                        .done(function (data) {
                            var mappedTransactions = $.map(data.unimported, function (item) {
                                return new Transaction(item, transactionUri);
                            });
                            self.isLoadingTransactions(false);
                            self.feeExceptions = data.feeExceptions;
                            self.transactions(mappedTransactions);
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            self.error(errorThrown);
                        });
                }
                //Wait for any new data.
                ticker.client.consumeNewTransactions = function (transactions) {
                    $.each(transactions, function (index, item) {
                        var transaction = new Transaction(item, transactionUri);
                        transaction.newlyAdded(true);
                        self.transactions.push(transaction);
                    });
                }

                $.connection.hub.logging = true;
                $.connection.hub.start().done(init);
            }
});