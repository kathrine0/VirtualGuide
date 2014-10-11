'use strict';
app.controller('travelsController', ['$scope', 'travelsService', function ($scope, travelsService) {
    
    $scope.travels = [];

    //var successCallback = function (data, status, headers, config) {
    //    notificationFactory.success();

    //    return travelsService.getTravels().success(getTravelsSuccessCallback).error(errorCallback);
    //};

    //var successPostCallback = function (data, status, headers, config) {
    //    successCallback(data, status, headers, config).success(function () {
    //        $scope.toggleAddMode();
    //        $scope.travel = {};
    //    });
    //};

    var getTravelsSuccessCallback = function (data, status) {
        $scope.travels = data;
    };

    var errorCallback = function (data, status, headers, config) {
        console.log("error");
        //notificationFactory.error(data.ExceptionMessage);
    };

    travelsService.getTravels().success(getTravelsSuccessCallback).error(errorCallback);


    //$scope.AllTravels = travelsService.query();

    //console.log($scope.AllTravels);

    //$scope.loading = true;
    //$scope.editMode = false;

    //$http.get('/api/posts/').success(function (data) {
    //    $scope.posts = data;
    //    $scope.loading = false;
    //})
    //.error(function () {
    //    $scope.error = "An Error has occured while loading posts!";
    //    $scope.loading = false;
    //});

    //$scope.toggleEdit = function () {
    //    $scope.editMode = !$scope.editMode;
    //};

    //$scope.save = function () {
    //    $http.put('/api/posts/', $scope.posts).success(function (data) {
    //        alert("Saved Successfully!!");
    //    }).error(function (data) {
    //        $scope.error = "An Error has occured while Saving posts! " + data;
    //        $scope.loading = false;
    //    });
    //};
}]);