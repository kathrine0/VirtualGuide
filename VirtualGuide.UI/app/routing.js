app.config(function ($routeProvider) {

    var site_prefix = '/VirtualGuide.UI';

    $routeProvider.when("/home", {
        title: "Home",
        controller: "homeController",
        templateUrl: site_prefix + "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        title: "Login",
        controller: "loginController",
        templateUrl: site_prefix + "/app/views/login/login.html"
    });

    $routeProvider.when("/signup", {
        title: "Signup",
        controller: "signupController",
        templateUrl: site_prefix + "/app/views/signup.html"
    });

    $routeProvider.when("/travels", {
        title: "Travels",
        controller: "getTravelsController",
        templateUrl: site_prefix + "/app/views/travel/travels.html"
    });

    $routeProvider.when("/travel/edit/:id", {
        title: "Edit travel",
        controller: "editTravelController",
        templateUrl: site_prefix + "/app/views/travel-form.html"
    });
    
    $routeProvider.when("/travel/new", {
        title: "Create travel",
        controller: "newTravelController",
        templateUrl: site_prefix + "/app/views/travel/travel-wizard-general.html"
    });

    $routeProvider.when("/travel/new/properties/:id", {
        title: "Create travel",
        controller: "newTravelPropertiesController",
        templateUrl: site_prefix + "/app/views/travel/travel-wizzard-properties.html"
    });

    $routeProvider.when("/travel/new/places/:id", {
        title: "Create travel",
        controller: "newTravelPlacesController",
        templateUrl: site_prefix + "/app/views/traveltravel-wizzard-places.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.run(['$location', '$rootScope', function ($location, $rootScope) {
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        $rootScope.title = current.$$route.title;
    });
}]);