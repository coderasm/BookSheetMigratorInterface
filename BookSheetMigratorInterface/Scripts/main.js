﻿require.config({
    paths: {
        "jquery": "jquery-1.10.2",
        "datetimepicker": "bootstrap-datetimepicker",
        "knockout": "knockout-3.3.0"
    }
});

require(["app/app"], function (app) {
    app.init();
});