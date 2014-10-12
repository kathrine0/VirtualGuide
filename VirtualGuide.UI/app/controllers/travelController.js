'use strict';
app.controller('getTravelsAction', ['$scope', '$location', 'travelService',
    function ($scope, $location, travelService) {
    
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

        
        $scope.travels = travelService.getAllForCreator()

        $scope.editTravel = function(id) {
            $location.path('/travel-edit/' + id);
        }

        $scope.newTravel = function () {
            $location.path('/travel-new');
        };
}]);

app.controller('travelEditController', ['$scope', '$location', '$routeParams', 'travelService',
    function ($scope, $location, $routeParams, travelService) {

        $scope.submit = "Edit";
        $scope.title = "Edit Travel";

        var travel = travelService.getItem($routeParams.id);
        $scope.travel = travel;
        
}]);

app.controller('travelNewController', ['$scope', '$location', 'travelService',
    function ($scope, $location, travelService) {

        $scope.submit = "Create";
        $scope.title = "Create Travel";

        $scope.CreateNewTravel = function () {
            travelService.createItem($scope.travel);
            $location.path('/travels');
        }

}]);
