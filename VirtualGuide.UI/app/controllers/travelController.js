'use strict';
app.controller('getTravels', ['$scope', '$location', 'travelService',
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
        
        $scope.map = {
            center: {
                latitude: 45,
                longitude: -73
            },
            zoom: 8
        };

        var travel = travelService.getItem($routeParams.id);
        $scope.travel = travel;
        
}]);

app.controller('newTravel', ['$scope', '$rootScope', '$location', 'travelService',
    function ($scope, $rootScope, $location, travelService) {

        $scope.travel = {};

        $scope.createNewTravel = function () {
            $scope.travel.ZoomLevel = $scope.map.zoom;
            $scope.travel.Latitude = $scope.map.latitude;
            $scope.travel.Longitude = $scope.map.longitude;

            travelService.createItem($scope.travel, function (travel) {
                $location.path('/travel-new2/' + travel.Id);
            });
        }

        var map = L.map('map').setView([51.505, -0.09], 13);
        $rootScope.map.addTo(map);

        var marker = L.marker([51.5, -0.09]).addTo(map);
}]);


app.controller('newTravelProperties', ['$scope', '$location', 'travelService',
    function ($scope, $location, travelService) {

        $scope.properties = [];

        $scope.travel.properties.push({
            Title: 'Historia',
            Description: '',
            Symbol: ''
        });


        //$scope.addProperty = function () {
        //    $scope.travel.properties.push({
        //        Title: '',
        //        Description: '',
        //        Symbol: ''
        //    });
        //};

        //$scope.removeProperty = function (index) {
        //    $scope.travel.properties.splice(index, 1);
        //};

    }]);

app.controller('newTravelPlaces', ['$scope', '$location', 'travelService',
    function ($scope, $location, travelService) {

    }]);