define(["knockout"],function(e){e.bindingHandlers["paging-control"]={makeTemplateValueAccessor:function(t){var n=t(),r=n.pageIndex,i=e.isObservable(n.itemCount)?n.itemCount:e.observable(n.itemCount),s=e.isObservable(n.perPage)?n.perPage:e.observable(n.perPage),o=e.isObservable(n.viewModel)?n.viewModel:e.observable(n.viewModel);if(!e.isWriteableObservable(r))throw new Error("pageIndex must be a writable observable");return function(){return{name:"paging-control",data:{atFirstPage:e.computed(function(){return r()===0}),atLastPage:e.computed(function(){return r()===Math.ceil(i()/s())-1}),previousPage:function(){this.atFirstPage()||r(r()-1)},nextPage:function(){this.atLastPage()||r(r()+1)},pages:e.computed(function(){var e=[],t=Math.ceil(i()/s());for(var n=0;n<t;n++)e.push({label:n+1,isCurrentPage:r()===n,handler:r.bind(o,n)});return e})}}}},init:function(t,n,r){return e.bindingHandlers.template.init(t,e.bindingHandlers["paging-control"].makeTemplateValueAccessor(n),r)},update:function(t,n,r,i,s){return e.bindingHandlers.template.update(t,e.bindingHandlers["paging-control"].makeTemplateValueAccessor(n),r,i,s)}},e.virtualElements.allowedBindings["paging-control"]=!0});