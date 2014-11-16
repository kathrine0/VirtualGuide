﻿var app = angular.module('VirtualGuide', ['ngRoute', 'ngResource', 'ngSanitize', 'LocalStorageModule', 'angular-loading-bar', 'angularFileUpload', 'leaflet-directive', 'ui.bootstrap', 'ui.router']);
// 'vgServices',

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


