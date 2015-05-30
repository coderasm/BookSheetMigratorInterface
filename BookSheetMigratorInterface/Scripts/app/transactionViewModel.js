define(['knockout'], function(ko) {
    return function Transaction(data, transactionUri) {
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
        self.sellerFirstName = data.sellerFirstName;
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
        self.buyerFirstName = data.buyerFirstName;
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
        self.year = data.year;
        self.sellers = ko.observableArray(data.sellers);
        self.buyers = ko.observableArray(data.buyers);
        self.buyerContacts = ko.observableArray();
        self.firstTimeLoading = true;
        self.buyerDealerId.subscribe(function() {
            setBuyerContacts();
            clearBuyerContactId();
        });

        self.importable = ko.computed(function() {
            return self.sellerDealerId() != null && self.buyerDealerId() != null && self.buyerContactId() != null
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
                url: self.transactionUri + "import",
                type: "POST",
                data: postData,
                contentType: 'application/json'
            });
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