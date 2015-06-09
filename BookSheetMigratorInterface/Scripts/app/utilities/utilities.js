define(['jquery'], function($) {
    if (typeof String.prototype.startsWith != 'function') {
        String.prototype.startsWith = function (str) {
            return this.slice(0, str.length) == str;
        }
    }

    if (typeof String.prototype.endsWith != 'function') {
        String.prototype.endsWith = function (str) {
            return this.slice(-str.length) == str;
        };
    }

    $.fn.formToJSON = function() {
        var array = this.serializeArray();
        var json = {};
        $.map(array, function (item, index) {
            json[item.name] = item.value;
        });
        return json;
    }
});