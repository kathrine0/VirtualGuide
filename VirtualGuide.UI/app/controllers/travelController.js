'use strict';
app.controller('getTravelsController', ['$scope', '$location', 'travelService',
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
            $location.path('/travel/edit/' + id);
        }

        $scope.newTravel = function () {
            $location.path('/travel/new');
        };
}]);

app.controller('editTravelController', ['$scope', '$location', '$routeParams', 'travelService',
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

app.controller('newTravelController', ['$scope', '$rootScope', '$location', 'travelService',
    function ($scope, $rootScope, $location, travelService) {

        $scope.travel = {};

        $scope.markers = [];

        $scope.createNewTravel = function () {
            var travel = $scope.travel;
            travel.ZoomLevel = 8;
            travel.Latitude = $scope.markers[0].lat;
            travel.Longitude = $scope.markers[0].lng;
            travel.ImageSrc = "";


            travelService.createItem(travel, function (newtravel) {
                $location.path('/travel/new/properties/' + newtravel.Id);
            });
        }

        $scope.$on('leafletDirectiveMap.geosearch_showlocation', function (jsEvent, leafletEvent) {
            var location = leafletEvent.leafletEvent.Location;
            $scope.markers = travelService.manageMarkers($scope.markers, parseFloat(location.Y), parseFloat(location.X));
        });

        $scope.$on('leafletDirectiveMap.click', function (jsEvent, leafletEvent) {
            var location = leafletEvent.leafletEvent.latlng;
            $scope.markers = travelService.manageMarkers($scope.markers, location.lat, location.lng);

        });
}]);


app.controller('newTravelPropertiesController', ['$scope', '$location', '$routeParams', 'propertyService',
    function ($scope, $location, $routeParams, propertyService) {

        $scope.properties = [];

        $scope.properties.push({
            Title: 'Historia',
            Description: '',
            Symbol: ''
        });


        $scope.addProperty = function () {
            $scope.properties.push({
                Title: '',
                Description: '',
                Symbol: ''
            });
        };

        $scope.removeProperty = function (index) {
            $scope.properties.splice(index, 1);
        };

        $scope.saveProperties = function ()
        {
            propertyService.createItems($scope.properties, $routeParams.id, function () {
                $location.path('/travel/new/places/' + $routeParams.id);
            });
        }

    }]);

app.controller('newTravelPlacesController', ['$scope', '$location', 'travelService',
    function ($scope, $location, travelService) {

        var editMode = false;
        $scope.activeMarker = null;
        $scope.markers = [];

        $scope.$on('leafletDirectiveMap.click', function (jsEvent, leafletEvent) {           
            var location = leafletEvent.leafletEvent.latlng;
            
            //TODO - more Angular style
            var searchValue = $("#leaflet-control-geosearch-qry").val();
            $("#leaflet-control-geosearch-qry").val("");
            
            $scope.activeMarker = {
                lat: location.lat,
                lng: location.lng,
                focus: true,
                draggable: true,
                get message() {
                    return this.place.Name;
                },
                place: {
                    _name: searchValue,
                    set Name(value)
                    {
                        this._name = value;
                    },
                    get Name() {
                        var value = this._name;
                        if (value == null || value == "") {
                            value = '\u00A0\u00A0';
                        }
                        return value;
                    },
                    Description: " ",
                    Category: 0
                }
            };

            

            $scope.markers.push($scope.activeMarker);

            editMode = true;

        });


        $scope.removeMarker = function (index) {
            $scope.activeMarker = null;
            $scope.markers.splice(index, 1);
        }

        $scope.focusMarker = function (index) {
            $scope.markers[index].focus = true;
        };

    }]);