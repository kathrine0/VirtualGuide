var app = angular.module('VirtualGuide', ['ngResource', 'ngSanitize', 'LocalStorageModule', 'angular-loading-bar',
    'angularFileUpload', 'leaflet-directive', 'ui.bootstrap', 'ui.router', 'pascalprecht.translate']);
// 'vgServices',

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


