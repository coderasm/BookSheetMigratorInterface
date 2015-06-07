define(['knockout'], function(ko) {
    ko.bindingHandlers['paging-control'] = {
        makeTemplateValueAccessor: function (valueAccessor) {
            var config = valueAccessor(),
                pageIndex = config.pageIndex,
                itemCount = ko.isObservable(config.itemCount) ? config.itemCount : ko.observable(config.itemCount),
                perPage = ko.isObservable(config.perPage) ? config.perPage : ko.observable(config.perPage),
                viewModel = ko.isObservable(config.viewModel) ? config.viewModel : ko.observable(config.viewModel);
            if (!ko.isWriteableObservable(pageIndex)) {
                throw new Error('pageIndex must be a writable observable');
            }
            return function() {
                return {
                    name: 'paging-control',
                    data: {
                        atFirstPage: ko.computed(function() {
                            return pageIndex() === 0;
                        }),
                        atLastPage: ko.computed(function() {
                            return pageIndex() === Math.ceil(itemCount()/perPage()) - 1;
                        }),
                        previousPage: function() {
                            if (!this.atFirstPage()) {
                                pageIndex(pageIndex() - 1);
                            }
                        },
                        nextPage: function() {
                            if (!this.atLastPage()) {
                                pageIndex(pageIndex() + 1);
                            }
                        },
                        pages: ko.computed(function() {
                            var indices = [], max = Math.ceil(itemCount()/perPage());
                            for (var i = 0; i < max; i++) {
                                indices.push({
                                    label: i + 1,
                                    isCurrentPage: pageIndex() === i,
                                    handler: pageIndex.bind(viewModel, i)
                                });
                            }
                            return indices;
                        })
                    }
                };
            };
        },
        init: function(element, valueAccessor, allBindings) {
            return ko.bindingHandlers.template.init(element,
                ko.bindingHandlers['paging-control'].makeTemplateValueAccessor(valueAccessor),
                allBindings);
        },
        update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
            return ko.bindingHandlers.template.update(element,
                ko.bindingHandlers['paging-control'].makeTemplateValueAccessor(valueAccessor),
                allBindings, viewModel, bindingContext);
        }
    };
    ko.virtualElements.allowedBindings['paging-control'] = true;
});