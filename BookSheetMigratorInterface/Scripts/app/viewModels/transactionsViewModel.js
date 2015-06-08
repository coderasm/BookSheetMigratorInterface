define([
        'knockout', 'app/viewModels/transactionViewModel',
        'app/bindings/isLoadingWhen', 'app/bindings/pagination',
        'app/utilities/utilities'
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
                            return item.sellerCompanyName.toLowerCase().startsWith(filter) ||
                                   item.sellerDmvNumber.toLowerCase().startsWith(filter) ||
                                   item.sellerPhone.toLowerCase().startsWith(filter) ||
                                   item.sellerAddress.toLowerCase().startsWith(filter) ||
                                   item.sellerCity.toLowerCase().startsWith(filter) ||
                                   item.buyerCompanyName.toLowerCase().startsWith(filter) ||
                                   item.buyerDmvNumber.toLowerCase().startsWith(filter) ||
                                   item.buyerPhone.toLowerCase().startsWith(filter) ||
                                   item.buyerAddress.toLowerCase().startsWith(filter) ||
                                   item.buyerCity.toLowerCase().startsWith(filter);
                        });
                    }
                });
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