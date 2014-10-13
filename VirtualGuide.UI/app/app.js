var app = angular.module('VirtualGuide', ['ngRoute', 'ngResource', 'google-maps'.ns(), 'ngSanitize', 'LocalStorageModule', 'angular-loading-bar']);
// 'vgServices',

app.run(function ($rootScope) {
    $rootScope.webservice = 'http://localhost/VirtualGuide/'; //global variable
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


