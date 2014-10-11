'use strict';
app.controller('travelsController', ['$scope', '$location', 'CreatorTravelsService',
    function ($scope, $location, CreatorTravelsService) {
    
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

    //var getTravelsSuccessCallback = function (data, status) {
    //    $scope.travels = data;
    //};

    //var errorCallback = function (data, status, headers, config) {
    //    notificationFactory.error(data.ExceptionMessage);
    //};

    //travelsService.getTravels().success(getTravelsSuccessCallback).error(errorCallback);


    $scope.travels = CreatorTravelsService.query();

    $scope.editTravel = function(id)
    {
        $location.path($location.path('/travel-edit/' + id));
    }

    $scope.newTravel = function () {
        $location.path('/travel-new');
    };
}]);

app.controller('travelEditController', ['$scope', '$location', 'CreatorTravelsService',
    function ($scope, $location, CreatorTravelsService) {

}]);

app.controller('travelNewController', ['$scope', '$location', 'CreatorTravelsService',
    function ($scope, $location, CreatorTravelsService) {

    $scope.CreateNewTravel = function () {

        var travel = $scope.travel;
        travel.Language = "pl_PL";

        CreatorTravelsService.create(travel);
        $location.path('/travels');
    }

}]);
