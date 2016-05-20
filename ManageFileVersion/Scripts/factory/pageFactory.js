// factory
function pageFactory(modelName) {
    var app = angular.module(modelName, ['ngSanitize']);
     var bindings = new Array();
    app.factory('pageFactory', function ($http) {        
        return {
            getData: function (url) {
                return ($http.get(url).then(function (res) {
                    return res.data;
                }));
            },
            postData: function (url, formData) {                
                return ($http.post(url, formData, {
                    //transformRequest: angular.identity,
                    headers: { 'Content-Type': undefined }
                }).then(function (res) {
                    return res.data;
                }));
            },
        }
    });

    app.filter("mydate", function () {
        return function (x) {
            return new Date(parseInt(x.substr(6)));
        };
    });
    return app;
};
