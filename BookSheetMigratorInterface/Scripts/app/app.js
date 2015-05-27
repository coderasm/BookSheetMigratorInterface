﻿//wrapper for a computed observable that can pause its subscriptions
ko.pauseableComputed = function (evaluatorFunction, evaluatorFunctionTarget) {
    var _cachedValue = "";
    var _isPaused = ko.observable(false);

    //the computed observable that we will return
    var result = ko.computed(function () {
        if (!_isPaused()) {
            //call the actual function that was passed in
            return evaluatorFunction.call(evaluatorFunctionTarget);
        }
        return _cachedValue;
    }, evaluatorFunctionTarget);

    //keep track of our current value and set the pause flag to release our actual subscriptions
    result.pause = function () {
        _cachedValue = this();
        _isPaused(true);
    }.bind(result);

    //clear the cached value and allow our computed observable to be re-evaluated
    result.resume = function () {
        _cachedValue = "";
        _isPaused(false);
    }

    return result;
};

function Transaction(data, transactionUri) {
    var self = this;
    self.transactionUri = transactionUri;
    self.eventId = data.eventId;
    self.transactionId = data.transactionId;
    self.bidAmount = ko.observable(data.bidAmount);
    self.soldDate = ko.observable(data.soldDate);
    self.sellerDmvNumber = data.sellerDmvNumber;
    self.sellerDealerId = ko.observable(data.sellerDealerId);
    self.sellerCompanyName = data.sellerCompanyName;
    self.sellerLastName = data.sellerLastName;
    self.sellerFirstName = data.sellerFirstname;
    self.sellerAddress = data.sellerAddress;
    self.sellerCity = data.sellerCity;
    self.sellerState = data.sellerState;
    self.sellerZip = data.sellerZip;
    self.sellerPhone = data.sellerPhone;
    self.buyerDmvNumber = data.buyerDmvNumber;
    self.buyerDealerId = ko.observable(data.buyerDealerId);
    self.buyerContactId = ko.observable(data.buyerContactId);
    self.buyerCompanyName = data.buyerCompanyName;
    self.buyerLastName = data.buyerLastName;
    self.buyerFirstName = data.buyerFirstname;
    self.buyerAddress = data.buyerAddress;
    self.buyerCity = data.buyerCity;
    self.buyerState = data.buyerState;
    self.buyerZip = data.buyerZip;
    self.buyerPhone = data.buyerPhone;
    self.transportFee = ko.observable(data.transportFee);
    self.mileage = data.mileage;
    self.make = data.make;
    self.model = data.model;
    self.vin = data.vin;
    self.vehicleyear = data.vehicleyear;
    self.sellers = ko.observableArray(data.sellers);
    self.buyers = ko.observableArray(data.buyers);
    self.buyerContacts = ko.observable();
    self.buyersContacts = ko.observableArray(data.buyerContacts);

    self.update = ko.pauseableComputed(function() {
        $.ajax({
            url: self.transactionUri,
            type: 'PUT',
            data: {
                eventId: self.eventId,
                transactionId: self.transactionId,
                bidAmount: self.bidAmount(),
                soldDate: self.soldDate(),
                sellerDealerId: self.sellerDealerId(),
                buyerDealerId: self.buyerDealerId(),
                buyerContactId: self.buyerContactId
            }
        });
    });

    self.importable = ko.computed(function() {
        return self.sellerDealerId() != "" && self.buyerDealerId() != "" && self.buyerContactId() != ""
            && self.bidAmount() > 1000 && self.transportFee();
    });

    self.importSale = function() {
        var postData = {
            transactions: [
                {
                    eventId: self.eventId,
                    transactionId: self.transactionId
                }
            ]
        }
        $.ajax({
            url: self.transactionUri + "import/",
            type: "POST",
            data: postData,
            contentType: 'application/json'
        });
    }
}

var transactionViewModel = function () {
    var self = this;
    self.transactions = ko.observableArray();
    self.error = ko.observable();

    var transactionUri = '/api/Transaction/';

    function ajaxHelper(uri, method, data) {
        self.error('');
        return $.ajax({
            type: method,
            url: transactionUri,
            dataType: 'json',
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    self.importAll = function() {
        ko.utils.arrayForEach(self.transactions, function(transaction) {
            if (transaction.importable())
                transaction.import();
        });
    }

    self.pullNewAndUpdateExistingTransactions = function() {
        
    }

    function getAllTransactions() {
        ajaxHelper(transactionUri, 'GET', {imported: false}).done(function (data) {
            var mappedTransactions = $.map(data, function (item) { return new Transaction(item, transactionUri); });
            self.transactions(mappedTransactions);
        });
    }

    // Fetch the initial data.
    getAllTransactions();
};

ko.applyBindings(new transactionViewModel());