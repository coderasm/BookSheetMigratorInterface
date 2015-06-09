define([
        'knockout', 'app/bindings/dateTimePicker', 'app/utilities/utilities',
        'app/extenders/changeTracking'
],
       function (ko) {
           return function Transaction(data, transactionUri) {
               var self = this;
               self.eventId = data.eventId;
               self.transactionId = data.transactionId;
               self.bidAmount = ko.observable(data.bidAmount).extend({trackChange: true});
               self.soldDate = ko.observable(data.soldDate).extend({ trackChange: true });
               self.sellerDmvNumber = data.sellerDmvNumber;
               self.sellerDealerId = ko.observable(data.sellerDealerId).extend({ trackChange: true });
               self.sellerCompanyName = data.sellerCompanyName;
               self.sellerPhone = data.sellerPhone;
               self.sellerAddress = data.sellerAddress;
               self.sellerCity = data.sellerCity;
               self.buyerDmvNumber = data.buyerDmvNumber;
               self.buyerDealerId = ko.observable(data.buyerDealerId).extend({ trackChange: true });
               self.buyerContactId = ko.observable(data.buyerContactId).extend({ trackChange: true });
               self.buyerCompanyName = data.buyerCompanyName;
               self.buyerPhone = data.buyerPhone;
               self.buyerAddress = data.buyerAddress;
               self.buyerCity = data.buyerCity;
               self.transportFee = ko.observable(data.transportFee).extend({ trackChange: true });
               self.mileage = data.mileage;
               self.make = data.make;
               self.model = data.model;
               self.vin = data.vin;
               self.year = data.year;
               self.sellers = ko.observableArray(data.sellers);
               self.buyers = ko.observableArray(data.buyers);
               self.buyerContacts = ko.observableArray([]);
               self.buyerDealerId.subscribe(function () {
                   setBuyerContacts();
                   clearBuyerContactId();
               });
               self.error = ko.observable("");
               self.hasError = ko.computed(function () {
                   return self.error() !== "";
               });
               self.success = ko.observable("");
               self.hasSuccess = ko.computed(function () {
                   return self.success() !== "";
               });

               self.isDirty = ko.computed(function () {
                   for (key in self) {
                       if (self.hasOwnProperty(key) && ko.isObservable(self[key]) && typeof self[key].isDirty === 'function' && self[key].isDirty()) {
                           return true;
                       }
                   }
                   return false;
               });

               self.importable = ko.computed(function () {
                   return self.sellerDealerId() != null && self.buyerDealerId() != null && self.buyerContactId() != null
                       && self.bidAmount() > 1000 && self.transportFee() >= 0 && !self.isDirty();
               });

               self.updateSale = function (formElement) {
                   var postData = $(formElement).formToJSON();
                   postData = addTransactionIdsTo(postData);
                   $.ajax({
                       url: transactionUri + "update",
                       type: "POST",
                       data: postData,
                       contenType: 'json',
                       success: function (result) {
                           self.error("");
                           if (result.success) {
                               resetAllDirty();
                               self.success("Update Successful");
                           } else
                               self.error("Update Not Successful");
                       }
                   });
               }

               self.importSale = function () {
                   var postData = {};
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
                   postData.eventId = self.eventId;
                   postData.transactionId = self.transactionId;
                   return postData;
               }

               function resetAllDirty() {
                   for (key in self) {
                       if (self.hasOwnProperty(key) && ko.isObservable(self[key]) && typeof self[key].isDirty === 'function' && self[key].isDirty()) {
                           self[key].orginalValue = self[key]();
                           self[key].isDirty(false);
                       }
                   }
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