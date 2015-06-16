require.config({
    paths: {
        "jquery": "jquery-1.10.2",
        "signalr": "jquery.signalR-2.2.0",
        "signalr.hubs": "/signalr/hubs?",
        "datetimepicker": "bootstrap-datetimepicker",
        "knockout": "knockout-3.3.0",
        "kovalidation": "knockout.validation"
    },
    shim: {
        "jquery": { exports: "$" },
        "signalr": { deps: ["jquery"] },
        "signalr.hubs": { deps: ["signalr"] }
    }
});

require(["app/app"], function (app) {
    app.init();
});