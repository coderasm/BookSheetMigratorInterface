/*=============================================================================
	Author:			Eric M. Barnard - @ericmbarnard								
	License:		MIT (http://opensource.org/licenses/mit-license.php)		
																				
	Description:	Validation Library for KnockoutJS							
	Version:		2.0.3											
===============================================================================
*/

(function(e){typeof require=="function"&&typeof exports=="object"&&typeof module=="object"?e(require("knockout"),exports):typeof define=="function"&&define.amd?define(["knockout","exports"],e):e(ko,ko.validation={})})(function(e,t){function l(e){var t=e==="max";return function(r,i){if(n.utils.isEmptyVal(r))return!0;var s,o;i.typeAttr===undefined?(o="text",s=i):(o=i.typeAttr,s=i.value),!isNaN(s)&&!(s instanceof Date)&&(o="number");var u,a,f;switch(o.toLowerCase()){case"week":u=/^(\d{4})-W(\d{2})$/,a=r.match(u);if(a===null)throw new Error("Invalid value for "+e+" attribute for week input.  Should look like "+"'2000-W33' http://www.w3.org/TR/html-markup/input.week.html#input.week.attrs.min");f=s.match(u);if(!f)return!1;return t?a[1]<f[1]||a[1]===f[1]&&a[2]<=f[2]:a[1]>f[1]||a[1]===f[1]&&a[2]>=f[2];case"month":u=/^(\d{4})-(\d{2})$/,a=r.match(u);if(a===null)throw new Error("Invalid value for "+e+" attribute for month input.  Should look like "+"'2000-03' http://www.w3.org/TR/html-markup/input.month.html#input.month.attrs.min");f=s.match(u);if(!f)return!1;return t?a[1]<f[1]||a[1]===f[1]&&a[2]<=f[2]:a[1]>f[1]||a[1]===f[1]&&a[2]>=f[2];case"number":case"range":return t?!isNaN(r)&&parseFloat(r)<=parseFloat(s):!isNaN(r)&&parseFloat(r)>=parseFloat(s);default:return t?r<=s:r>=s}}}function c(e,t,r){return t.validator(e(),r.params===undefined?!0:i(r.params))?!0:(e.setError(n.formatMessage(r.message||t.message,i(r.params),e)),!1)}function h(e,t,r){e.isValidating(!0);var s=function(s){var o=!1,u="";if(!e.__valid__()){e.isValidating(!1);return}s.message?(o=s.isValid,u=s.message):o=s,o||(e.error(n.formatMessage(u||r.message||t.message,i(r.params),e)),e.__valid__(o)),e.isValidating(!1)};n.utils.async(function(){t.validator(e(),r.params===undefined?!0:i(r.params),s)})}if(typeof e=="undefined")throw new Error("Knockout is required, please ensure it is loaded before loading this validation plug-in");e.validation=t;var n=e.validation,r=e.utils,i=r.unwrapObservable,s=r.arrayForEach,o=r.extend,u={registerExtenders:!0,messagesOnModified:!0,errorsAsTitle:!0,errorsAsTitleOnModified:!1,messageTemplate:null,insertMessages:!0,parseInputAttributes:!1,writeInputAttributes:!1,decorateInputElement:!1,decorateElementOnModified:!0,errorClass:null,errorElementClass:"validationElement",errorMessageClass:"validationMessage",allowHtmlMessages:!1,grouping:{deep:!1,observable:!0,live:!1},validate:{}},a=o({},u);a.html5Attributes=["required","pattern","min","max","step"],a.html5InputTypes=["email","number","date"],a.reset=function(){o(a,u)},n.configuration=a,n.utils=function(){var e=(new Date).getTime(),t={},r="__ko_validation__";return{isArray:function(e){return e.isArray||Object.prototype.toString.call(e)==="[object Array]"},isObject:function(e){return e!==null&&typeof e=="object"},isNumber:function(e){return!isNaN(e)},isObservableArray:function(e){return!!e&&typeof e.remove=="function"&&typeof e.removeAll=="function"&&typeof e.destroy=="function"&&typeof e.destroyAll=="function"&&typeof e.indexOf=="function"&&typeof e.replace=="function"},values:function(e){var t=[];for(var n in e)e.hasOwnProperty(n)&&t.push(e[n]);return t},getValue:function(e){return typeof e=="function"?e():e},hasAttribute:function(e,t){return e.getAttribute(t)!==null},getAttribute:function(e,t){return e.getAttribute(t)},setAttribute:function(e,t,n){return e.setAttribute(t,n)},isValidatable:function(e){return!!(e&&e.rules&&e.isValid&&e.isModified)},insertAfter:function(e,t){e.parentNode.insertBefore(t,e.nextSibling)},newId:function(){return e+=1},getConfigOptions:function(e){var t=n.utils.contextFor(e);return t||n.configuration},setDomData:function(e,i){var s=e[r];s||(e[r]=s=n.utils.newId()),t[s]=i},getDomData:function(e){var n=e[r];return n?t[n]:undefined},contextFor:function(e){switch(e.nodeType){case 1:case 8:var t=n.utils.getDomData(e);if(t)return t;if(e.parentNode)return n.utils.contextFor(e.parentNode)}return undefined},isEmptyVal:function(e){if(e===undefined)return!0;if(e===null)return!0;if(e==="")return!0},getOriginalElementTitle:function(e){var t=n.utils.getAttribute(e,"data-orig-title"),r=e.title,i=n.utils.hasAttribute(e,"data-orig-title");return i?t:r},async:function(e){window.setImmediate?window.setImmediate(e):window.setTimeout(e,0)},forEach:function(e,t){if(n.utils.isArray(e))return s(e,t);for(var r in e)e.hasOwnProperty(r)&&t(e[r],r)}}}();var f=function(){function f(e){s(e.subscriptions,function(e){e.dispose()}),e.subscriptions=[]}function l(e){e.options.deep&&(s(e.flagged,function(e){delete e.__kv_traversed}),e.flagged.length=0),e.options.live||f(e)}function c(e,t){t.validatables=[],f(t),h(e,t),l(t)}function h(t,n,r){var i=[],s=t.peek?t.peek():t;if(t.__kv_traversed===!0)return;n.options.deep&&(t.__kv_traversed=!0,n.flagged.push(t)),r=r!==undefined?r:n.options.deep?1:-1,e.isObservable(t)&&(!t.errors&&!a.isValidatable(t)&&t.extend({validatable:!0}),n.validatables.push(t),n.options.live&&a.isObservableArray(t)&&n.subscriptions.push(t.subscribe(function(){n.graphMonitor.valueHasMutated()}))),s&&!s._destroy&&(a.isArray(s)?i=s:a.isObject(s)&&(i=a.values(s))),r!==0&&a.forEach(i,function(t){t&&!t.nodeType&&(!e.isComputed(t)||t.rules)&&h(t,n,r+1)})}function p(e){var t=[];return s(e,function(e){a.isValidatable(e)&&!e.isValid()&&t.push(e.error.peek())}),t}var t=0,u=n.configuration,a=n.utils;return{init:function(e,r){if(t>0&&!r)return;e=e||{},e.errorElementClass=e.errorElementClass||e.errorClass||u.errorElementClass,e.errorMessageClass=e.errorMessageClass||e.errorClass||u.errorMessageClass,o(u,e),u.registerExtenders&&n.registerExtenders(),t=1},reset:n.configuration.reset,group:function(n,i){i=o(o({},u.grouping),i);var f={options:i,graphMonitor:e.observable(),flagged:[],subscriptions:[],validatables:[]},l=null;return i.observable?l=e.computed(function(){return f.graphMonitor(),c(n,f),p(f.validatables)}):l=function(){return c(n,f),p(f.validatables)},l.showAllMessages=function(e){e===undefined&&(e=!0),l.forEach(function(t){a.isValidatable(t)&&t.isModified(e)})},l.isAnyMessageShown=function(){var e;return e=!!l.find(function(e){return a.isValidatable(e)&&!e.isValid()&&e.isModified()}),e},l.filter=function(e){return e=e||function(){return!0},l(),r.arrayFilter(f.validatables,e)},l.find=function(e){return e=e||function(){return!0},l(),r.arrayFirst(f.validatables,e)},l.forEach=function(e){e=e||function(){},l(),s(f.validatables,e)},l.map=function(e){return e=e||function(e){return e},l(),r.arrayMap(f.validatables,e)},l._updateState=function(e){if(!a.isObject(e))throw new Error("An object is required.");n=e;if(!i.observable)return c(e,f),p(f.validatables);f.graphMonitor.valueHasMutated()},l},formatMessage:function(e,t,n){a.isObject(t)&&t.typeAttr&&(t=t.value);if(typeof e=="function")return e(t,n);var r=i(t);return r==null&&(r=[]),a.isArray(r)||(r=[r]),e.replace(/{(\d+)}/gi,function(e,t){return typeof r[t]!="undefined"?r[t]:e})},addRule:function(e,t){e.extend({validatable:!0});var n=!!r.arrayFirst(e.rules(),function(e){return e.rule&&e.rule===t.rule});return n||e.rules.push(t),e},addAnonymousRule:function(e,t){t.message===undefined&&(t.message="Error"),t.onlyIf&&(t.condition=t.onlyIf),n.addRule(e,t)},addExtender:function(t){e.extenders[t]=function(e,r){return r&&(r.message||r.onlyIf)?n.addRule(e,{rule:t,message:r.message,params:a.isEmptyVal(r.params)?!0:r.params,condition:r.onlyIf}):n.addRule(e,{rule:t,params:r})}},registerExtenders:function(){if(u.registerExtenders)for(var t in n.rules)n.rules.hasOwnProperty(t)&&(e.extenders[t]||n.addExtender(t))},insertValidationMessage:function(e){var t=document.createElement("SPAN");return t.className=a.getConfigOptions(e).errorMessageClass,a.insertAfter(e,t),t},parseInputValidationAttributes:function(e,t){s(n.configuration.html5Attributes,function(r){if(a.hasAttribute(e,r)){var i=e.getAttribute(r)||!0;if(r==="min"||r==="max"){var s=e.getAttribute("type");if(typeof s=="undefined"||!s)s="text";i={typeAttr:s,value:i}}n.addRule(t(),{rule:r,params:i})}});var r=e.getAttribute("type");s(n.configuration.html5InputTypes,function(e){e===r&&n.addRule(t(),{rule:e==="date"?"dateISO":e,params:!0})})},writeInputValidationAttributes:function(t,i){var o=i();if(!o||!o.rules)return;var u=o.rules();s(n.configuration.html5Attributes,function(n){var i=r.arrayFirst(u,function(e){return e.rule&&e.rule.toLowerCase()===n.toLowerCase()});if(!i)return;e.computed({read:function(){var r=e.unwrap(i.params);i.rule==="pattern"&&r instanceof RegExp&&(r=r.source),t.setAttribute(n,r)},disposeWhenNodeIsRemoved:t})}),u=null},makeBindingHandlerValidatable:function(t){var n=e.bindingHandlers[t].init;e.bindingHandlers[t].init=function(t,r,i,s,o){return n(t,r,i,s,o),e.bindingHandlers.validationCore.init(t,r,i,s,o)}},setRules:function(t,r){var s=function(t,r){if(!t||!r)return;for(var o in r){if(!r.hasOwnProperty(o))continue;var u=r[o];if(!t[o])continue;var f=t[o],l=i(f),c={},h={};for(var p in u){if(!u.hasOwnProperty(p))continue;n.rules[p]?c[p]=u[p]:h[p]=u[p]}e.isObservable(f)&&f.extend(c);if(l&&a.isArray(l))for(var d=0;d<l.length;d++)s(l[d],h);else s(l,h)}};s(t,r)}}}();o(e.validation,f),n.rules={},n.rules.required={validator:function(e,t){var n;return e===undefined||e===null?!t:(n=e,typeof e=="string"&&(String.prototype.trim?n=e.trim():n=e.replace(/^\s+|\s+$/g,"")),t?(n+"").length>0:!0)},message:"This field is required."},n.rules.min={validator:l("min"),message:"Please enter a value greater than or equal to {0}."},n.rules.max={validator:l("max"),message:"Please enter a value less than or equal to {0}."},n.rules.minLength={validator:function(e,t){if(n.utils.isEmptyVal(e))return!0;var r=n.utils.isNumber(e)?""+e:e;return r.length>=t},message:"Please enter at least {0} characters."},n.rules.maxLength={validator:function(e,t){if(n.utils.isEmptyVal(e))return!0;var r=n.utils.isNumber(e)?""+e:e;return r.length<=t},message:"Please enter no more than {0} characters."},n.rules.pattern={validator:function(e,t){return n.utils.isEmptyVal(e)||e.toString().match(t)!==null},message:"Please check this value."},n.rules.step={validator:function(e,t){if(n.utils.isEmptyVal(e)||t==="any")return!0;var r=e*100%(t*100);return Math.abs(r)<1e-5||Math.abs(1-r)<1e-5},message:"The value must increment by {0}."},n.rules.email={validator:function(e,t){return t?n.utils.isEmptyVal(e)||t&&/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$/i.test(e):!0},message:"Please enter a proper email address."},n.rules.date={validator:function(e,t){return t?n.utils.isEmptyVal(e)||t&&!/Invalid|NaN/.test(new Date(e)):!0},message:"Please enter a proper date."},n.rules.dateISO={validator:function(e,t){return t?n.utils.isEmptyVal(e)||t&&/^\d{4}[-/](?:0?[1-9]|1[012])[-/](?:0?[1-9]|[12][0-9]|3[01])$/.test(e):!0},message:"Please enter a proper date."},n.rules.number={validator:function(e,t){return t?n.utils.isEmptyVal(e)||t&&/^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(e):!0},message:"Please enter a number."},n.rules.digit={validator:function(e,t){return t?n.utils.isEmptyVal(e)||t&&/^\d+$/.test(e):!0},message:"Please enter a digit."},n.rules.phoneUS={validator:function(e,t){return t?n.utils.isEmptyVal(e)?!0:typeof e!="string"?!1:(e=e.replace(/\s+/g,""),t&&e.length>9&&e.match(/^(1-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$/)):!0},message:"Please specify a valid phone number."},n.rules.equal={validator:function(e,t){var r=t;return e===n.utils.getValue(r)},message:"Values must equal."},n.rules.notEqual={validator:function(e,t){var r=t;return e!==n.utils.getValue(r)},message:"Please choose another value."},n.rules.unique={validator:function(e,t){var i=n.utils.getValue(t.collection),s=n.utils.getValue(t.externalValue),o=0;return!e||!i?!0:(r.arrayFilter(i,function(n){e===(t.valueAccessor?t.valueAccessor(n):n)&&o++}),o<(s?1:2))},message:"Please make sure the value is unique."},function(){n.registerExtenders()}(),e.bindingHandlers.validationCore=function(){return{init:function(t,r,i,s,o){var u=n.utils.getConfigOptions(t),a=r();u.parseInputAttributes&&n.utils.async(function(){n.parseInputValidationAttributes(t,r)});if(u.insertMessages&&n.utils.isValidatable(a)){var f=n.insertValidationMessage(t);u.messageTemplate?e.renderTemplate(u.messageTemplate,{field:a},null,f,"replaceNode"):e.applyBindingsToNode(f,{validationMessage:a})}u.writeInputAttributes&&n.utils.isValidatable(a)&&n.writeInputValidationAttributes(t,r),u.decorateInputElement&&n.utils.isValidatable(a)&&e.applyBindingsToNode(t,{validationElement:a})}}}(),n.makeBindingHandlerValidatable("value"),n.makeBindingHandlerValidatable("checked"),e.bindingHandlers.textInput&&n.makeBindingHandlerValidatable("textInput"),n.makeBindingHandlerValidatable("selectedOptions"),e.bindingHandlers.validationMessage={update:function(t,s){var o=s(),u=n.utils.getConfigOptions(t),a=i(o),f=null,l=!1,c=!1;if(o===null||typeof o=="undefined")throw new Error("Cannot bind validationMessage to undefined value. data-bind expression: "+t.getAttribute("data-bind"));l=o.isModified&&o.isModified(),c=o.isValid&&o.isValid();var h=null;if(!u.messagesOnModified||l)h=c?null:o.error;var p=!u.messagesOnModified||l?!c:!1,d=t.style.display!=="none";u.allowHtmlMessages?r.setHtml(t,h):e.bindingHandlers.text.update(t,function(){return h}),d&&!p?t.style.display="none":!d&&p&&(t.style.display="")}},e.bindingHandlers.validationElement={update:function(t,r,s){var o=r(),u=n.utils.getConfigOptions(t),a=i(o),f=null,l=!1,c=!1;if(o===null||typeof o=="undefined")throw new Error("Cannot bind validationElement to undefined value. data-bind expression: "+t.getAttribute("data-bind"));l=o.isModified&&o.isModified(),c=o.isValid&&o.isValid();var h=function(){var e={},t=!u.decorateElementOnModified||l?!c:!1;return e[u.errorElementClass]=t,e};e.bindingHandlers.css.update(t,h,s);if(!u.errorsAsTitle)return;e.bindingHandlers.attr.update(t,function(){var e=!u.errorsAsTitleOnModified||l,r=n.utils.getOriginalElementTitle(t);if(e&&!c)return{title:o.error,"data-orig-title":r};if(!e||c)return{title:r,"data-orig-title":null}})}},e.bindingHandlers.validationOptions=function(){return{init:function(e,t,r,s,u){var a=i(t());if(a){var f=o({},n.configuration);o(f,a),n.utils.setDomData(e,f)}}}}(),e.extenders.validation=function(e,t){return s(n.utils.isArray(t)?t:[t],function(t){n.addAnonymousRule(e,t)}),e},e.extenders.validatable=function(t,r){n.utils.isObject(r)||(r={enable:r}),"enable"in r||(r.enable=!0);if(r.enable&&!n.utils.isValidatable(t)){var i=n.configuration.validate||{},s={throttleEvaluation:r.throttle||i.throttle};t.error=e.observable(null),t.rules=e.observableArray(),t.isValidating=e.observable(!1),t.__valid__=e.observable(!0),t.isModified=e.observable(!1),t.isValid=e.computed(t.__valid__),t.setError=function(e){var n=t.error.peek(),r=t.__valid__.peek();t.error(e),t.__valid__(!1),n!==e&&!r&&t.isValid.notifySubscribers()},t.clearError=function(){return t.error(null),t.__valid__(!0),t};var u=t.subscribe(function(){t.isModified(!0)}),a=e.computed(o({read:function(){var e=t(),r=t.rules();return n.validateObservable(t),!0}},s));o(a,s),t._disposeValidation=function(){t.isValid.dispose(),t.rules.removeAll(),u.dispose(),a.dispose(),delete t.rules,delete t.error,delete t.isValid,delete t.isValidating,delete t.__valid__,delete t.isModified,delete t.setError,delete t.clearError,delete t._disposeValidation}}else r.enable===!1&&t._disposeValidation&&t._disposeValidation();return t},n.validateObservable=function(e){var t=0,r,i,s=e.rules(),o=s.length;for(;t<o;t++){i=s[t];if(i.condition&&!i.condition())continue;r=i.rule?n.rules[i.rule]:i;if(r.async||i.async)h(e,r,i);else if(!c(e,r,i))return!1}return e.clearError(),!0};var p={},d;n.defineLocale=function(e,t){return e&&t?(p[e.toLowerCase()]=t,t):null},n.locale=function(e){if(e){e=e.toLowerCase();if(!p.hasOwnProperty(e))throw new Error("Localization "+e+" has not been loaded.");n.localize(p[e]),d=e}return d},n.localize=function(e){var t=n.rules;for(var r in e)t.hasOwnProperty(r)&&(t[r].message=e[r])},function(){var e={},t=n.rules;for(var r in t)t.hasOwnProperty(r)&&(e[r]=t[r].message);n.defineLocale("en-us",e)}(),d="en-us",e.applyBindingsWithValidation=function(t,r,i){var s=document.body,u;r&&r.nodeType?(s=r,u=i):u=r,n.init(),u&&(u=o(o({},n.configuration),u),n.utils.setDomData(s,u)),e.applyBindings(t,s)};var v=e.applyBindings;e.applyBindings=function(e,t){n.init(),v(e,t)},e.validatedObservable=function(t,r){if(!r&&!n.utils.isObject(t))return e.observable(t).extend({validatable:!0});var i=e.observable(t);return i.errors=n.group(n.utils.isObject(t)?t:{},r),i.isValid=e.observable(i.errors().length===0),e.isObservable(i.errors)?i.errors.subscribe(function(e){i.isValid(e.length===0)}):e.computed(i.errors).subscribe(function(e){i.isValid(e.length===0)}),i.subscribe(function(e){n.utils.isObject(e)||(e={}),i.errors._updateState(e),i.isValid(i.errors().length===0)}),i}});