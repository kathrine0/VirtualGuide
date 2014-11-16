'use strict';
app.controller('indexController', ['$scope', '$state', 'authService', function ($scope, $state, authService) {

    $scope.logOut = function () {
        authService.logOut();
        $state.go('login');
    }

    $scope.authentication = authService.authentication;

}]);