define([
        'knockout', 'app/bindings/dateTimePicker', 'app/utilities/utilities',
        'app/extenders/changeTracking', 'kovalidation', 'app/configurations/validation'
],
       function (ko) {
           return function Transaction(data, transactionUri) {
               var self = this;
               self.eventId = data.eventId;
               self.transactionId = data.transactionId;
               self.bidAmount = ko.observable(data.bidAmount).extend({
                   trackChange: true,
                   required: true,
                   min: 1000,
                   message: "Must be >= 1000"
               });
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
               self.transportFee = ko.observable(data.transportFee).extend({
                   trackChange: true,
                   required: true,
                   min: 0,
                   message: "Must be >= 0"
               });
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
               self.isImported = data.imported != null;
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

               self.contactNameTitle = function contactNameTitle(contact) {
                   return contact.name + " - " + contact.title;
               }

               self.clearChanges = function() {
                   revertAllDirtyBack();
               }

               function revertAllDirtyBack() {
                   for (key in self) {
                       if (self.hasOwnProperty(key) && ko.isObservable(self[key]) && typeof self[key].isDirty === "function" && self[key].isDirty()) {
                           self[key](self[key].originalValue);
                           self[key].isDirty(false);
                       }
                   }
               }

               self.importable = ko.computed(function () {
                   return self.sellerDealerId() != null && self.buyerDealerId() != null && self.buyerContactId() != null
                       && self.bidAmount() >= 1000 && self.transportFee() >= 0;
               });

               self.updateable = ko.computed(function() {
                   return self.bidAmount() >= 1000 && self.transportFee() >= 0;
               });

               self.updateSale = function (formElement) {
                   self.error("");
                   if (!self.isDirty()) {
                       self.error("Nothing to update.");
                       return;
                   }
                   else if (!self.updateable()) {
                       self.error("Fix your errors.");
                       return;
                   }
                   var postData = $(formElement).formToJSON();
                   $.ajax({
                       url: transactionUri + "update",
                       type: "POST",
                       data: postData,
                       dataType: 'json',
                       success: function (result) {
                           self.error("");
                           if (result.success) {
                               updateAllDirtyToNewValues();
                               self.success("Update Successful");
                           } else
                               self.error("Update Not Successful");
                       }
                   });
               }

               self.importSale = function () {
                   self.error("");
                   if (self.isImported) {
                       self.error("Already imported.");
                       return;
                   }
                   if (self.isDirty()) {
                       self.error("Update or clear your changes first.");
                       return;
                   }
                   if (!self.importable()) {
                       self.error("Fix your errors.");
                       return;
                   }
                   $.ajax({
                       url: transactionUri + "import/" + self.eventId + "/" + self.transactionId,
                       type: "POST",
                       data: {},
                       dataType: 'json',
                       success: function (result) {
                           if (result.success) {
                               self.success("Import Successful");
                               self.isImported = true;
                           } else
                               self.success("Import Not Successful");
                       }
                   });
               }

               function updateAllDirtyToNewValues() {
                   for (key in self) {
                       if (self.hasOwnProperty(key) && ko.isObservable(self[key]) && typeof self[key].isDirty === 'function' && self[key].isDirty()) {
                           self[key].originalValue = self[key]();
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