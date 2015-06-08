define([
        'knockout', 'app/bindings/dateTimePicker'
],
       function (ko) {
           return function Transaction(data, transactionUri) {
               var self = this;
               self.eventId = data.eventId;
               self.transactionId = data.transactionId;
               self.bidAmount = ko.observable(data.bidAmount);
               self.soldDate = ko.observable(data.soldDate);
               self.sellerDmvNumber = data.sellerDmvNumber;
               self.sellerDealerId = ko.observable(data.sellerDealerId);
               self.sellerCompanyName = data.sellerCompanyName;
               self.sellerPhone = data.sellerPhone;
               self.sellerAddress = data.sellerAddress;
               self.sellerCity = data.sellerCity;
               self.buyerDmvNumber = data.buyerDmvNumber;
               self.buyerDealerId = ko.observable(data.buyerDealerId);
               self.buyerContactId = ko.observable(data.buyerContactId);
               self.buyerCompanyName = data.buyerCompanyName;
               self.buyerPhone = data.buyerPhone;
               self.buyerAddress = data.buyerAddress;
               self.buyerCity = data.buyerCity;
               self.transportFee = ko.observable(data.transportFee);
               self.mileage = data.mileage;
               self.make = data.make;
               self.model = data.model;
               self.vin = data.vin;
               self.year = data.year;
               self.sellers = ko.observableArray(data.sellers);
               self.buyers = ko.observableArray(data.buyers);
               self.buyerContacts = ko.observableArray([]);
               self.firstTimeLoading = true;
               self.buyerDealerId.subscribe(function () {
                   setBuyerContacts();
                   clearBuyerContactId();
               });
               self.error = ko.observable("");
               self.hasError = ko.computed(function () {
                   return self.error() !== "";
               });
               function setError(error) {
                   self.error(error);
               }
               self.success = ko.observable("");
               self.hasSuccess = ko.computed(function () {
                   return self.success() !== "";
               });

               self.hasChanges = ko.computed(function () {
                   self.sellerDealerId();
                   self.buyerDealerId();
                   self.buyerContactId();
                   self.bidAmount();
                   self.transportFee();
                   self.soldDate();
                   if (self.firstTimeLoading) {
                       self.firstTimeLoading = false;
                       return false;
                   }
                   setError("Changes have been made. Changes must be submitted before importing.");
                   return true;
               });

               self.importable = ko.computed(function () {
                   return self.sellerDealerId() != null && self.buyerDealerId() != null && self.buyerContactId() != null
                       && self.bidAmount() > 1000 && self.transportFee() >= 0 && !self.hasChanges();
               });

               self.updateSale = function () {
                   var postData = {
                       transactions: [{}]
                   }
                   postData = addTransactionIdsTo(postData);
                   $.ajax({
                       url: transactionUri + "update",
                       type: "POST",
                       data: postData,
                       contenType: 'json',
                       success: function (results) {
                           if (results.success) {
                               self.success("Update Successful");
                               self.error("");
                               self.hasChanges(false);
                           } else
                               self.error("Update Not Successful");
                       }
                   });
               }

               self.importSale = function () {
                   var postData = {
                       transactions: [{}]
                   }
                   postData = addTransactionIdsTo(postData);
                   $.ajax({
                       url: transactionUri + "import",
                       type: "POST",
                       data: postData,
                       contentType: 'json',
                       success: function (results) {
                           if (results.success)
                               self.success("Import Successful");
                           else
                               self.success("Import Not Successful");
                       }
                   });
               }

               function addTransactionIdsTo(postData) {
                   postData.transactions[0].eventId = self.eventId;
                   postData.transactions[0].transactionId = self.transactionId;
                   return postData;
               }

               function initializeBuyerContacts() {
                   setBuyerContacts();
               }

               function clearBuyerContactId() {
                   self.buyerContactId(null);
               }

               function setBuyerContacts() {
                   self.buyers().every(function (buyer) {
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