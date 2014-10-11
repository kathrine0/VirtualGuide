var app = angular.module('VirtualGuide', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);
// 'vgServices',
app.config(function ($routeProvider) {

    var site_prefix = '/VirtualGuide.UI';

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: site_prefix + "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: site_prefix + "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: site_prefix + "/app/views/signup.html"
    });

    $routeProvider.when("/travels", {
        controller: "travelsController",
        templateUrl: site_prefix + "/app/views/travels.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.run(function ($rootScope) {
    $rootScope.webservice = 'http://localhost/VirtualGuide/'; //global variable
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

