define(['knockout', 'kovalidation'],
    function(ko) {
        ko.validation.init({
            errorElementClass: 'has-error',
            errorMessageClass: 'help-block',
            decorateInputElement: true
        });
    }
);