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

    if (typeof String.prototype.contains != 'function') {
        String.prototype.contains = function (str) {
            return this.indexOf(str) != -1;
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