define([
        'knockout', 'app/viewModels/transactionViewModel',
        'app/viewModels/bulkUpdateAjaxOperation', 'app/viewModels/bulkImportAjaxOperation',
        'app/bindings/isLoadingWhen', 'app/bindings/pagination',
        'app/utilities/utilities', 'signalr.hubs'
],
        function (ko, Transaction, BulkUpdateAjaxOperation, BulkImportAjaxOperation) {
            return function transactionsViewModel() {
                var self = this;
                self.transactions = ko.observableArray([]);
                self.error = ko.observable();
                self.hasTransactions = ko.computed(function () {
                    return self.transactions().length > 0;
                });
                self.transactionUri = '/api/Transaction/';
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
                    var bulkImporter = new BulkImportAjaxOperation(selectedTransactions, self);
                    bulkImporter.execute();
                }

                function findSelectedTransactions() {
                    return ko.utils.arrayFilter(self.transactions(), function (transaction) {
                        return transaction.isSelected();
                    });
                }

                self.showImportMessagesAndRemove = function(results) {
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
                    var bulkUpdater = new BulkUpdateAjaxOperation(selectedTransactions, self);
                    bulkUpdater.execute();
                }

                self.showUpdateMessages = function(results) {
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

                self.migratorBeginSoldDate = ko.observable(new Date());

                self.migratorEndSoldDate = ko.observable(new Date());

                self.doMigration = function() {
                    ticker.server.migrateAndBroadcastToAll(self.migratorBeginSoldDate(), self.migratorEndSoldDate())
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            self.error(errorThrown);
                        });
                }

                var ticker = $.connection.transactionTicker; // the generated client-side hub proxy

                function init() {
                    ticker.server.getUnimported()
                        .done(function (data) {
                            var mappedTransactions = $.map(data.unimported, function (item) {
                                return new Transaction(item, self.transactionUri);
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
                        var transaction = new Transaction(item, self.transactionUri);
                        transaction.newlyAdded(true);
                        self.transactions.push(transaction);
                    });
                }

                $.connection.hub.logging = true;
                $.connection.hub.start().done(init);
            }
});