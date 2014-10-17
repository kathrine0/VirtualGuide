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

        $scope.markers = [];

        $scope.createNewTravel = function () {
            console.log($scope.markers);
            var travel = $scope.travel;
            travel.ZoomLevel = 8;
            travel.Latitude = $scope.markers[0].lat;
            travel.Longitude = $scope.markers[0].lng;

            travelService.createItem(travel, function (newtravel) {
                $location.path('/travel-new2/' + newtravel.Id);
            });
        }

        $scope.$on('leafletDirectiveMap.geosearch_showlocation', function (jsEvent, leafletEvent) {

            var location = leafletEvent.leafletEvent.Location;
            console.log(location);
            if ($scope.markers.length == 0) {
                var marker = {
                    lat: parseFloat(location.Y),
                    lng: parseFloat(location.X),
                    focus: true,
                    draggable: true
                };
                $scope.markers.push(marker);
            } else {
                $scope.markers[0].lat = parseFloat(location.Y);
                $scope.markers[0].lng = parseFloat(location.X);
            }
        });

        $scope.$on('leafletDirectiveMap.click', function (jsEvent, leafletEvent) {

            var location = leafletEvent.leafletEvent.latlng;
            console.log(location);
            if ($scope.markers.length == 0)
            {
                var marker = {
                    lat: location.lat,
                    lng: location.lng,
                    focus: true,
                    draggable: true
                };
                $scope.markers.push(marker);
            } else {
                $scope.markers[0].lat = location.lat;
                $scope.markers[0].lng = location.lng;
            }
        });
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