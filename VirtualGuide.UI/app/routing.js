'use strict';

app.config(function ($stateProvider, $urlRouterProvider) {

    //local config
    //var site_prefix = '/VirtualGuide.UI/';

    //remote config
    var site_prefix = '';

    $urlRouterProvider.otherwise("/admin/home");

    $stateProvider
    .state('admin', {
        url: "/admin",
        controller: function ($scope, $rootScope) {
            $scope.$on('$viewContentLoaded', 
                function (event) {
                    if (!$rootScope.isAdminJSRegistered)
                    {
                        loadAdmin();
                        $rootScope.isAdminJSRegistered = true;
                    }
                });
        },
        templateUrl: site_prefix + "/app/views/templates/admin.html"
    })
    .state('admin.profile', {
        url: "/profile",
        title: "Profile",
        controller: "profileController",
        templateUrl: site_prefix + "/app/views/profile.html"
    })
    .state('admin.home', {
        url: "/home",
        title: "Start",
        controller: "homeController",
        templateUrl: site_prefix + "/app/views/home.html"
    })
    .state('admin.travels', {
        url: "/travels",
        title: "Guides",
        controller: "getTravelsController",
        templateUrl: site_prefix + "/app/views/travel/travels.html"
    })

    .state('admin.travelshow', {
        url: "/travel/show/:id",
        title: "Guide",
        controller: "getTravelController",
        templateUrl: site_prefix + "/app/views/travel/travel.html"
    })
    .state('admin.travelnew', {
        url: "/travel/new",
        title: "Guide Wizard",
        controller: "newTravelController",
        templateUrl: site_prefix + "/app/views/travel/travel-wizard-general.html"
    })
    .state('admin.travelproperties', {
        url: "/travel/properties/:id",
        title: "Guide Wizard",
        controller: "newTravelPropertiesController",
        templateUrl: site_prefix + "/app/views/travel/travel-wizard-properties.html"
    })
    .state('admin.travelplaces', {
        url: "/travel/places/:id",
        title: "Guide Wizard",
        controller: "newTravelPlacesController",
        templateUrl: site_prefix + "/app/views/travel/travel-wizard-places.html"
    })

    .state('login', {
        url: "/login",
        controller: "loginController",
        templateUrl: site_prefix + "/app/views/login/login.html"
    })
});

//    $routeProvider.when("/signup", {
//        title: "Signup",
//        controller: "signupController",
//        templateUrl: site_prefix + "/app/views/signup.html"
//    });


app.run(['$state', '$rootScope', '$urlRouter', 'authService', function ($state, $rootScope, $urlRouter, authService) {
    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
        $rootScope.title = toState.title ? toState.title : "";
    });

    $rootScope.$on('$locationChangeSuccess', function(event) {

        event.preventDefault();
        if (!authService.authentication.isAuth) {
            $state.transitionTo('login');
        } else {
            $urlRouter.sync();
        }
    });

}]);