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
                   min: 1000
               });
               self.feeException = ko.observable(data.feeException).extend({ trackChange: true });
               self.soldDate = ko.observable(data.soldDate).extend({ trackChange: true });
               self.failedImport = data.failedImport;
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
                   min: 0
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
               self.newlyAdded = ko.observable(false);
               self.isImported = data.imported != null;
               self.isNotImporting = ko.observable(true);
               self.isSelected = ko.observable(false);
               self.error = ko.observable("");
               self.hasError = ko.computed(function () {
                   return self.error() !== "";
               });
               self.success = ko.observable("");
               self.hasSuccess = ko.computed(function () {
                   return self.success() !== "";
               });

               function clearAlerts() {
                   self.success("");
                   self.error("");
               }

               self.isDirty = ko.computed(function () {
                   for (key in self) {
                       var member = self[key];
                       if (self.hasOwnProperty(key) && ko.isObservable(member) && typeof member.isDirty === 'function' && member.isDirty()) {
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
                       var member = self[key];
                       if (self.hasOwnProperty(key) && ko.isObservable(member) && typeof member.isDirty === "function" && member.isDirty()) {
                           member(member.originalValue);
                           member.isDirty(false);
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

               self.update = function (formElement) {
                   if (!self.isValidUpdate())
                       return;
                   var postData = $(formElement).formToJSON();
                   $.ajax({
                       url: transactionUri + self.eventId + "/" + self.transactionId,
                       type: "PUT",
                       data: postData,
                       dataType: 'json',
                       success: function (result) {
                           if (result.success) {
                               updateAllDirtyToNewValues();
                               self.success("Update Successful.");
                           } else
                               self.error("Update Failed. See I.T.");
                       }
                   });
               }

               self.isValidUpdate = function() {
                   clearAlerts();
                   if (!self.isDirty()) {
                       self.error("Nothing to update.");
                       return false;
                   } else if (!self.updateable()) {
                       self.error("Fix your errors.");
                       return false;
                   }
                   return true;
               }

               self.showUpdateResult = function (result) {
                   if (result.success) {
                       updateAllDirtyToNewValues();
                       self.success(result.message);
                   } else
                       self.error(result.message);
               }

               self.importAndRemove = function (transactions) {
                   if (!self.isValidImport())
                       return;
                   self.isNotImporting(false);
                   $.ajax({
                       url: transactionUri + "import/" + self.eventId + "/" + self.transactionId,
                       type: "POST",
                       data: {},
                       dataType: 'json',
                       success: function (result) {
                           self.isNotImporting(true);
                           self.showImportResultAndRemove(result, transactions);
                       }
                   });
               }

               self.isValidImport = function() {
                   clearAlerts();
                   if (self.isImported) {
                       self.error("Already imported.");
                       return false;
                   }
                   if (self.isDirty()) {
                       self.error("Update or clear your changes first.");
                       return false;
                   }
                   if (!self.importable()) {
                       self.error("Fix your errors.");
                       return false;
                   }
                   return true;
               }

               self.showImportResultAndRemove = function(result, transactions)
               {
                   if (result.success) {
                       self.isImported = true;
                       self.success(result.message);
                       self.removeAfterTime(transactions, 5000);
                   } else
                       self.error(result.message);
               }

               self.remove = function(transactions) {
                   var remainingTransactions = ko.utils.arrayFilter(transactions(), function (transaction) {
                       return transaction !== self;
                   });
                   transactions(remainingTransactions);
               }

               self.removeAfterTime = function (transactions, time) {
                   setTimeout(function() {
                       self.remove(transactions);
                   }, time);
               }

               self.isDeleting = false;

               self.doDelete = function (transactions) {
                   if (self.isDeleting)
                       return;
                   self.isDeleting = true;
                   $.ajax({
                       url: transactionUri + self.eventId + "/" + self.transactionId,
                       type: "DELETE",
                       data: {},
                       dataType: 'json',
                       success: function (result) {
                           self.isDeleting = false;
                           if (result.success)
                               self.remove(transactions);
                           else
                               self.error("Unable to delete.");
                       }
                   });
               }

               self.isHiding = false;

               self.hide = function(transactions) {
                   if (self.isHiding)
                       return;
                   self.isHiding = true;
                   $.ajax({
                       url: transactionUri + "hide/" + self.eventId + "/" + self.transactionId,
                       type: "PUT",
                       data: {},
                       dataType: 'json',
                       success: function (result) {
                           self.isHiding = false;
                           if (result.success)
                               self.remove(transactions);
                           else
                               self.error("Unable to hide.");
                       }
                   });
               }

               function updateAllDirtyToNewValues() {
                   for (key in self) {
                       var member = self[key];
                       if (self.hasOwnProperty(key) && ko.isObservable(member) && typeof member.isDirty === 'function' && member.isDirty()) {
                           member.originalValue = member();
                           member.isDirty(false);
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

               self.equals = function(transaction)
               {
                   return self.eventId === transaction.eventId && self.transactionId === transaction.transactionId;
               }

               initializeBuyerContacts();
           }
       });