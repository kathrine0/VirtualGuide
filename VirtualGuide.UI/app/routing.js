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
        controller: "getTravelsAction",
        templateUrl: site_prefix + "/app/views/travels.html"
    });

    $routeProvider.when("/travel-edit/:id", {
        controller: "travelEditController",
        templateUrl: site_prefix + "/app/views/travel-form.html"
    });
    
    $routeProvider.when("/travel-new", {
        controller: "travelNewController",
        templateUrl: site_prefix + "/app/views/travel-form.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});