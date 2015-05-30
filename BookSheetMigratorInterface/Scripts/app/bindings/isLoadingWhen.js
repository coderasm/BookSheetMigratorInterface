define(['knockout'], function (ko) {
    ko.bindingHandlers.loadingWhen = {
        init: function (element) {
            var
                //cache a reference to the element as we use it multiple times below
                $element = $(element),
                //create the new div with the 'loader' class and hide it
                $loader = $('<div class="modal fade" id="loadingDialog" data-backdrop="static" data-keyboard="false">\
                            <div class="modal-dialog">\
                                <div class="modal-content">\
                                    <div class="modal-header">\
                                        <h1>Loading Transactions...</h1>\
                                    </div>\
                                    <div class="modal-body">\
                                        <div class="container-fluid">\
                                            <div class="row">\
                                                <div class="col-md-12">\
                                                    <div class="progress">\
                                                        <div class="progress-bar progress-bar-info progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">\
                                                        </div>\
                                                    </div>\
                                                </div>\
                                            </div>\
                                        </div>\
                                    </div>\
                                    <div class="modal-footer">\
                                    </div>\
                                </div>\
                            </div>\
                        </div>');

            //add the loader div to the original element
            $element.append($loader);
        },
        update: function (element, valueAccessor) {
            var
                //unwrap the value of the flag using knockout utilities
                isLoading = ko.utils.unwrapObservable(valueAccessor()),

                //get a reference to the parent element
                $element = $(element),

                //get a reference to the loader
                $loader = $element.find("div#loadingDialog");

            //if we are currently loading...
            if (isLoading) {
                //...and show the loader
                $loader.modal('toggle');
            } else {
                //otherwise, fade out the loader
                $loader.modal('toggle');
            }
        }
    };
});