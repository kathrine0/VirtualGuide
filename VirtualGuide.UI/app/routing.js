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
        templateUrl: site_prefix + "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        title: "Signup",
        controller: "signupController",
        templateUrl: site_prefix + "/app/views/signup.html"
    });

    $routeProvider.when("/travels", {
        title: "Travels",
        controller: "getTravelsAction",
        templateUrl: site_prefix + "/app/views/travels.html"
    });

    $routeProvider.when("/travel-edit/:id", {
        title: "Edit travel",
        controller: "travelEditController",
        templateUrl: site_prefix + "/app/views/travel-form.html"
    });
    
    $routeProvider.when("/travel-new", {
        title: "Create travel",
        controller: "travelNewController",
        templateUrl: site_prefix + "/app/views/travel-form.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });
});

app.run(['$location', '$rootScope', function ($location, $rootScope) {
    $rootScope.$on('$routeChangeSuccess', function (event, current, previous) {
        $rootScope.title = current.$$route.title;
    });
}]);