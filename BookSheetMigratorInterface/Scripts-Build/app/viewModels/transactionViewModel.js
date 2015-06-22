define(["knockout","app/bindings/dateTimePicker","app/utilities/utilities","app/extenders/changeTracking","kovalidation","app/configurations/validation"],function(e){return function(n,r){function s(){i.success(""),i.error("")}function o(){for(key in i){var t=i[key];i.hasOwnProperty(key)&&e.isObservable(t)&&typeof t.isDirty=="function"&&t.isDirty()&&(t(t.originalValue),t.isDirty(!1))}}function u(){for(key in i){var t=i[key];i.hasOwnProperty(key)&&e.isObservable(t)&&typeof t.isDirty=="function"&&t.isDirty()&&(t.originalValue=t(),t.isDirty(!1))}}function a(){l()}function f(){i.buyerContactId(null)}function l(){i.buyers().every(function(e){return e.dealerId===i.buyerDealerId()?(i.buyerContacts(e.contacts),!1):!0})}var i=this;i.eventId=n.eventId,i.transactionId=n.transactionId,i.bidAmount=e.observable(n.bidAmount).extend({trackChange:!0,required:!0,min:1e3}),i.soldDate=e.observable(n.soldDate).extend({trackChange:!0}),i.failedImport=n.failedImport,i.sellerDmvNumber=n.sellerDmvNumber,i.sellerDealerId=e.observable(n.sellerDealerId).extend({trackChange:!0}),i.sellerCompanyName=n.sellerCompanyName,i.sellerPhone=n.sellerPhone,i.sellerAddress=n.sellerAddress,i.sellerCity=n.sellerCity,i.buyerDmvNumber=n.buyerDmvNumber,i.buyerDealerId=e.observable(n.buyerDealerId).extend({trackChange:!0}),i.buyerContactId=e.observable(n.buyerContactId).extend({trackChange:!0}),i.buyerCompanyName=n.buyerCompanyName,i.buyerPhone=n.buyerPhone,i.buyerAddress=n.buyerAddress,i.buyerCity=n.buyerCity,i.transportFee=e.observable(n.transportFee).extend({trackChange:!0,required:!0,min:0}),i.mileage=n.mileage,i.make=n.make,i.model=n.model,i.vin=n.vin,i.year=n.year,i.sellers=e.observableArray(n.sellers),i.buyers=e.observableArray(n.buyers),i.buyerContacts=e.observableArray([]),i.buyerDealerId.subscribe(function(){l(),f()}),i.newlyAdded=e.observable(!1),i.isImported=n.imported!=null,i.isSelected=e.observable(!1),i.error=e.observable(""),i.hasError=e.computed(function(){return i.error()!==""}),i.success=e.observable(""),i.hasSuccess=e.computed(function(){return i.success()!==""}),i.isDirty=e.computed(function(){for(key in i){var t=i[key];if(i.hasOwnProperty(key)&&e.isObservable(t)&&typeof t.isDirty=="function"&&t.isDirty())return!0}return!1}),i.contactNameTitle=function(t){return t.name+" - "+t.title},i.clearChanges=function(){o()},i.importable=e.computed(function(){return i.sellerDealerId()!=null&&i.buyerDealerId()!=null&&i.buyerContactId()!=null&&i.bidAmount()>=1e3&&i.transportFee()>=0}),i.updateable=e.computed(function(){return i.bidAmount()>=1e3&&i.transportFee()>=0}),i.update=function(e){if(!i.isValidateUpdate())return;var t=$(e).formToJSON();$.ajax({url:r+i.eventId+"/"+i.transactionId,type:"PUT",data:t,dataType:"json",success:function(e){e.success?(u(),i.success("Update Successful.")):i.error("Update Failed. See I.T.")}})},i.isValidateUpdate=function(){return s(),i.isDirty()?i.updateable()?!0:(i.error("Fix your errors."),!1):(i.error("Nothing to update."),!1)},i.showUpdateResult=function(e){e.success?(u(),i.success(e.message)):i.error(e.message)},i.importAndRemove=function(e){if(!i.isValidImport())return;$.ajax({url:r+"import/"+i.eventId+"/"+i.transactionId,type:"POST",data:{},dataType:"json",success:function(t){i.showImportResultAndRemove(t,e)}})},i.isValidImport=function(){return s(),i.isImported?(i.error("Already imported."),!1):i.isDirty()?(i.error("Update or clear your changes first."),!1):i.importable()?!0:(i.error("Fix your errors."),!1)},i.showImportResultAndRemove=function(e,t){e.success?(i.success(e.message),i.remove(t)):i.error(e.message)},i.remove=function(t){setTimeout(function(){var n=e.utils.arrayFilter(t(),function(e){return e!==i});t(n)},6e4)},i.equals=function(e){return i.eventId===e.eventId&&i.transactionId===e.transactionId},a()}});