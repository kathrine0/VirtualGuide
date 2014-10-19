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

        $scope.showTravel = function(id) {
            $location.path('/travel/' + id);
        }

        $scope.newTravel = function () {
            $location.path('/travel/new');
        };
}]);

app.controller('getTravelController', ['$scope', '$routeParams', 'travelService',
    function ($scope, $routeParams, travelService) {


        $scope.travel = travelService.getTravelForCreator($routeParams.id);

}]);

app.controller('newTravelController', ['$scope', '$rootScope', '$location', 'travelService',
    function ($scope, $rootScope, $location, travelService) {

        //scope variables
        $scope.travel = {};
        $scope.markers = [];
        //end scope variables

        //scope actions
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
        //end scope actions

        //scope events
        $scope.$on('leafletDirectiveMap.geosearch_showlocation', function (jsEvent, leafletEvent) {
            var location = leafletEvent.leafletEvent.Location;
            $scope.markers = travelService.manageMarkers($scope.markers, parseFloat(location.Y), parseFloat(location.X));
        });

        $scope.$on('leafletDirectiveMap.click', function (jsEvent, leafletEvent) {
            var location = leafletEvent.leafletEvent.latlng;
            $scope.markers = travelService.manageMarkers($scope.markers, location.lat, location.lng);

        });
        //end scope events
}]);

app.controller('newTravelPropertiesController', ['$scope', '$location', '$routeParams', 'propertyService',
    function ($scope, $location, $routeParams, propertyService) {

        //scope variables
        $scope.properties = [];
        $scope.properties.push({
            Title: 'Historia',
            Description: '',
            Symbol: ''
        });

        //end scope variables

        //scope actions
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
        //end scope actions

    }]);

app.controller('newTravelPlacesController', ['$scope', '$location', '$filter', '$routeParams', 'placeService',
    function ($scope, $location, $filter, $routeParams, placeService) {

        //local variables
        var editMode = false;
        var id = 0;
        //end local variables

        //scope variables
        $scope.activeMarker = null;
        $scope.markers = [];
        $scope.categories = placeService.getCategories();
        //end scope variables

        //scope events
        $scope.$on('marker.focus', function(jsEvent, leafletEvent) {
            $scope.activeMarker = $filter('getByProperty')('id', leafletEvent.target.options.id, $scope.markers);
        });

        $scope.$on('marker.lostFocus', function (jsEvent, leafletEvent) {
            $scope.activeMarker = null;
        });

        $scope.$on('leafletDirectiveMap.click', function (jsEvent, leafletEvent) {           
            var location = leafletEvent.leafletEvent.latlng;
            
            //TODO - more Angular style
            var searchValue = $("#leaflet-control-geosearch-qry").val();
            $("#leaflet-control-geosearch-qry").val("");
            
            id++;
            var marker = {
                id: id,
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
                            value = '\u00A0\u00A0'; //just space symbol for preservation of styles
                        }
                        return value;
                    },
                    Description: " ",
                    CategoryId: 0
                }
            };
            
            $scope.activeMarker = marker;
            $scope.markers.push(marker);

            editMode = true;

        });
        //end scope events

        //scope actions
        $scope.removeMarker = function (index) {
            $scope.activeMarker = null;
            $scope.markers.splice(index, 1);
        }

        $scope.focusMarker = function (index) {
            $scope.markers[index].focus = true;
        };

        $scope.savePlaces = function () {
            placeService.createItems($scope.markers, $routeParams.id, function () {
                $location.path('/travel/' + $routeParams.id);
            });
        }
        //end scope actions
    }]);