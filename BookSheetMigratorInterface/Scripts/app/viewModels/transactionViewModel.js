define([
        'knockout', 'app/viewModels/TrackableBidAmount',
        'app/viewModels/TrackableSoldDate', 'app/viewModels/TrackableId',
        'app/viewModels/TrackableTransportFee', 'app/bindings/datePicker'
],
       function(ko, TrackableBidAmount, TrackableSoldDate, TrackableId, TrackableTransportFee) {
           return function Transaction(data, transactionUri) {
               var self = this;
               self.transactionUri = transactionUri;
               self.eventId = data.eventId;
               self.transactionId = data.transactionId;
               self.changes = ko.observableArray([]);
               self.bidAmount = new TrackableBidAmount(data.bidAmount, self.changes).dataItem;
               self.soldDate = new TrackableSoldDate(data.soldDate, self.changes).dataItem;
               self.sellerDmvNumber = data.sellerDmvNumber;
               self.sellerDealerId = new TrackableId("sellerDealerId", data.sellerDealerId, self.changes).dataItem;
               self.sellerCompanyName = data.sellerCompanyName;
               self.sellerLastName = data.sellerLastName;
               self.sellerFirstName = data.sellerFirstName;
               self.sellerAddress = data.sellerAddress;
               self.sellerCity = data.sellerCity;
               self.sellerState = data.sellerState;
               self.sellerZip = data.sellerZip;
               self.sellerPhone = data.sellerPhone;
               self.buyerDmvNumber = data.buyerDmvNumber;
               self.buyerDealerId = new TrackableId("buyerDealerId", data.buyerDealerId, self.changes).dataItem;
               self.buyerContactId = new TrackableId("buyerContactId", data.buyerContactId, self.changes).dataItem;
               self.buyerCompanyName = data.buyerCompanyName;
               self.buyerLastName = data.buyerLastName;
               self.buyerFirstName = data.buyerFirstName;
               self.buyerAddress = data.buyerAddress;
               self.buyerCity = data.buyerCity;
               self.buyerState = data.buyerState;
               self.buyerZip = data.buyerZip;
               self.buyerPhone = data.buyerPhone;
               self.transportFee = new TrackableTransportFee(data.transportFee, self.changes).dataItem;
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
               self.error = ko.observable("");
               self.hasError = ko.computed(function () {
                   return self.error() !== "";
               });
               self.success = ko.observable("");
               self.hasSuccess = ko.computed(function() {
                   return self.success() !== "";
               });

               self.hasChanges = ko.computed(function () {
                   return self.changes().length > 0;
               });

               self.importable = ko.computed(function() {
                   return self.sellerDealerId() != null && self.buyerDealerId() != null && self.buyerContactId() != null
                       && self.bidAmount() > 1000 && self.transportFee() >= 0 && !self.hasChanges();
               });
        
               self.updateSale = function() {
                   if (self.hasChanges()) {
                       var postData = {
                           transactions: [{
                               changes: self.changes()
                           }]
                       }
                       postData = addTransactionIdsTo(postData);
                       $.ajax({
                           url: self.transactionUri + "update",
                           type: "POST",
                           data: postData,
                           contenType: 'json',
                           success: function(results) {
                               if (results.success)
                                   self.success("Update Successful");
                               else
                                   self.success("Update Not Successful");
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
                       contentType: 'json',
                       success: function(results) {
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