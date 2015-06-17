define(["knockout","app/viewModels/transactionViewModel","app/bindings/isLoadingWhen","app/bindings/pagination","app/utilities/utilities","signalr.hubs"],function(e,t){return function(){function o(){s.server.getUnimported().done(function(e){var n=$.map(e,function(e){return new t(e,i)});r.isLoadingTransactions(!1),r.transactions(n)}).fail(function(e,t,n){r.error(n)})}var r=this;r.transactions=e.observableArray([]),r.error=e.observable(),r.hasTransactions=e.computed(function(){return r.transactions().length>0}),r.filter=e.observable(""),r.showOnlyNew=e.observable(!1),r.filteredTransactions=e.computed(function(){var t=r.filter().toLowerCase();if(!t&&!r.showOnlyNew())return r.transactions();var n=r.transactions();return r.showOnlyNew()&&(n=e.utils.arrayFilter(n,function(e){return e.newlyAdded()})),t&&(n=e.utils.arrayFilter(n,function(e){return e.vin.toLowerCase().contains(t)})),n}),r.toggleShowOnlyNew=function(){r.showOnlyNew(!r.showOnlyNew())},r.showOnlyNew=e.observable(!1),r.newCount=e.computed(function(){var t=e.utils.arrayFilter(r.transactions(),function(e){return e.newlyAdded()});return t.length}),r.filterTypeCount=e.computed(function(){return r.showOnlyNew()?r.transactions().length:r.newCount()}),r.filterTypeText=e.computed(function(){return r.showOnlyNew()?"All":"New"}),r.hasNew=e.computed(function(){return r.newCount()>0}),r.isLoadingTransactions=e.observable(!0),r.itemCount=e.computed(function(){return r.transactions().length}),r.perPage=e.observable(10),r.pageIndex=e.observable(0),r.page=e.computed(function(){var e=r.pageIndex()*r.perPage();return r.filteredTransactions().slice(e,e+r.perPage())}),r.fadeIn=function(e){$(e).hide(),$(e).fadeIn(500)};var i="/api/Transaction/";r.selectAll=function(){e.utils.arrayForEach(r.page(),function(e){e.isSelected(!0)})},r.selectNone=function(){e.utils.arrayForEach(r.page(),function(e){e.isSelected(!1)})},r.importSelected=function(){e.utils.arrayForEach(r.transactions(),function(e){e.importable()&&e.isSelected()&&e.importSale()})},r.updateSelected=function(){e.utils.arrayForEach(r.transactions(),function(e){e.updateable()&&e.isSelected()&&e.updateSale()})};var s=$.connection.transactionTicker;s.client.consumeNewTransactions=function(e){$.each(e,function(e,n){var s=new t(n,i);s.newlyAdded(!0),r.transactions.push(s)})},$.connection.hub.logging=!0,$.connection.hub.start().done(o)}});