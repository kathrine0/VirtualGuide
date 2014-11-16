'use strict';
app.controller('loginController', ['$scope', '$state', 'authService', function ($scope, $state, authService) {

    var init = function()
    {
        if (authService.authentication.isAuth)
        {
            $state.go("admin.home");
        }
    }

    init();

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.message = "";

    $scope.login = function () {

        authService.login($scope.loginData).then(function (response) {

            $state.go("admin.home");

        },
         function (err) {
             $scope.message = err.error_description;
         });
    };

}]);