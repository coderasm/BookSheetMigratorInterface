define(['knockout', 'datetimepicker', 'moment'], function (ko) {
    ko.bindingHandlers.datetimepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datetimepickerOptions || {};
            $(element).datetimepicker(options);

            //handle the field changing
            ko.utils.registerEventHandler(element, "dp.change", function () {
                var observable = valueAccessor();
                observable($(element).data("DateTimePicker").date().format());
            });

            //handle disposal (if KO removes by the template binding)
            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).data("DateTimePicker").destroy();
            });

        },
        //update the control when the view model changes
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor());
            $(element).data("DateTimePicker").date(value);
        }
    };
});