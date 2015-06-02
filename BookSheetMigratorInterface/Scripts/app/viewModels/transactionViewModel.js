define(['knockout'], function(ko) {
    return function Transaction(data, transactionUri) {
        var self = this;
        self.transactionUri = transactionUri;
        self.eventId = data.eventId;
        self.transactionId = data.transactionId;
        self.bidAmount = new BidAmountObservable(data.bidAmount);
        self.soldDate = new DateObserveable(data.soldDate);
        self.sellerDmvNumber = data.sellerDmvNumber;
        self.sellerDealerId = new IdObservable(data.sellerDealerId);
        self.sellerCompanyName = data.sellerCompanyName;
        self.sellerLastName = data.sellerLastName;
        self.sellerFirstName = data.sellerFirstName;
        self.sellerAddress = data.sellerAddress;
        self.sellerCity = data.sellerCity;
        self.sellerState = data.sellerState;
        self.sellerZip = data.sellerZip;
        self.sellerPhone = data.sellerPhone;
        self.buyerDmvNumber = data.buyerDmvNumber;
        self.buyerDealerId = IdObservable(data.buyerDealerId);
        self.buyerContactId = IdObservable(data.buyerContactId);
        self.buyerCompanyName = data.buyerCompanyName;
        self.buyerLastName = data.buyerLastName;
        self.buyerFirstName = data.buyerFirstName;
        self.buyerAddress = data.buyerAddress;
        self.buyerCity = data.buyerCity;
        self.buyerState = data.buyerState;
        self.buyerZip = data.buyerZip;
        self.buyerPhone = data.buyerPhone;
        self.transportFee = TransportFeeObservable(data.transportFee);
        self.mileage = data.mileage;
        self.make = data.make;
        self.model = data.model;
        self.vin = data.vin;
        self.year = data.year;
        self.sellers = ko.observableArray(data.sellers);
        self.buyers = ko.observableArray(data.buyers);
        self.buyerContacts = ko.observableArray();
        self.firstTimeLoading = true;
        self.buyerDealerId.subscribe(function() {
            setBuyerContacts();
            clearBuyerContactId();
        });

        self.changes = {};

        self.doImport = ko.observable(false);

        self.importable = ko.computed(function() {
            return self.sellerDealerId() != null && self.buyerDealerId() != null && self.buyerContactId() != null
                && self.bidAmount() > 1000 && self.transportFee() >= 0 && !hasChanges();
        });

        function hasChanges() {
            return Object.keys(self.changes).length !== 0;
        }
        
        self.update = function() {
            if (hasChanges()) {
                var postData = {
                    transactions: [self.changes]
                }
                postData = addTransactionIdsTo(postData);
                $.ajax({
                    url: self.transactionUri + "update",
                    type: "POST",
                    data: postData,
                    contenType: 'json',
                    success: function(results) {
                        if(results.success)

                    }
                });
            }
        }

        self.importSale = function() {
            var postData = {
                transactions: [{}]
            }
            postData = addTransactionIdsTo(postData);
            $.ajax({
                url: self.transactionUri + "import",
                type: "POST",
                data: postData,
                contentType: 'json'
            });
        }

        function addTransactionIdsTo(transactions) {
            postData.eventId = self.eventId;
            postData.transactionId = self.transactionId;
            return postData;
        }

        function initializeBuyerContacts() {
            setBuyerContacts();
        }

        function clearBuyerContactId() {
            self.buyerContactId(null);
        }

        function setBuyerContacts() {
            self.buyers().every(function(buyer) {
                if (buyer.dealerId === self.buyerDealerId()) {
                    self.buyerContacts(buyer.contacts);
                    return false;
                }
                return true;
            });
        }

        initializeBuyerContacts();
    }
});