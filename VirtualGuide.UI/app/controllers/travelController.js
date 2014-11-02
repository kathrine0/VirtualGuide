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
            $location.path('/travel/show/' + id);
        }

        $scope.newTravel = function () {
            $location.path('/travel/new');
        };
}]);

app.controller('getTravelController', ['$scope', '$routeParams', '$modal', 'travelService', 'propertyService', 'placeService', 'uploadService',
    function ($scope, $routeParams, $modal, travelService, propertyService, placeService, uploadService) {

        //local variables

        var imagesToUpload = [];
        var placeholder = "http://fpoimg.com/300x300?text=Place%20your%20image%20here";

        //end local variables

        $scope.map =
        {
            markers: [],
            center: {
                lat: 0,
                lng: 0,
                zoom: 1
            },
            defaults: {
                scrollWheelZoom: false
            }

        }

        $scope.icons = propertyService.getIcons();

        $scope.categories = placeService.getCategories();

        $scope.travel = travelService.getTravelForCreator($routeParams.id, function (travel)
        {
            $scope.map.center = {
                lat: travel.Latitude,
                lng: travel.Longitude,
                zoom: travel.ZoomLevel
            };

            $scope.map.markers = placeService.placesToMarkers(travel.Places, placeholder);
        });

        $scope.travel.editMode = false;

        $scope.editTravel = function()
        {
            $scope.travel.editMode = true;
        }

        $scope.saveTravel = function()
        {
            travelService.updateItem($scope.travel);
            $scope.travel.editMode = false;
        }

        $scope.editProperty = function(index)
        {
            $scope.travel.Properties[index].editMode = true;
        }

        $scope.saveProperty = function (index) {
            propertyService.updateItem($scope.travel.Properties[index]);
            $scope.travel.Properties[index].editMode = false;
        }

        $scope.cancelPropertyEdit = function(index)
        {
            $scope.travel.Properties[index].editMode = false;
        }

        $scope.removeProperty = function (index) {
            //todo remove on server
            $scope.travel.Properties.splice(index, 1);
        }

        $scope.editPlace = function (index) {
            $scope.map.markers[index].editMode = true;
        }

        $scope.savePlace = function (index) {
            propertyService.updateItem($scope.travel.Properties[index]);
            $scope.map.markers[index].editMode = false;
        }

        $scope.removePlace = function (index) {
            //todo remove on server
            $scope.map.markers.splice(index, 1);
        }

        $scope.chooseIcon = function ($index) {

            var currentProperty = $index

            var modalInstance = $modal.open({
                templateUrl: 'app/views/travel/icon-modal-content.html',
                controller: 'IconModalController',
                resolve: {
                    icons: function () {
                        return $scope.icons
                    }
                }
            });

            modalInstance.result.then(function (iconIndex) {
                if (currentProperty != -1) {
                    $scope.travel.Properties[currentProperty].Icon = $scope.icons[iconIndex];
                    $scope.travel.Properties[currentProperty].IconId = $scope.icons[iconIndex].Id;

                }
            }, function () {
                //nothing happens
            });
        };

        $scope.onFileSelect = function ($files, index) {
            uploadService.decodeImage($files[0], function (image) {
                $scope.map.markers[index].place.image = image;
                imagesToUpload.push($files[0]);
            });
        }


}]);

app.controller('newTravelController', ['$scope', '$rootScope', '$location', 'travelService', 'uploadService',
    function ($scope, $rootScope, $location, travelService, uploadService) {

        //local variables
        var imageToUpload = null;
        var placeholder = "http://fpoimg.com/1000x300?text=Place%20your%20image%20here";
        //end local variables

        //scope variables
        $scope.travel = {};
        $scope.markers = [];
        $scope.image = placeholder;
        //end scope variables

        //scope actions
        $scope.createNewTravel = function () {
            var travel = $scope.travel;
            travel.ZoomLevel = 12;
            travel.Latitude = $scope.markers[0].lat;
            travel.Longitude = $scope.markers[0].lng;

            if (imageToUpload != null)
            {
                var randomName = uploadService.randomName($scope.travel.Name, imageToUpload.name);
                travel.ImageSrc = randomName;
                uploadService.upload(imageToUpload, randomName);
            }


            travelService.createItem(travel, function (newtravel) {
                $location.path('/travel/new/properties/' + newtravel.Id);
            });
        }

        //todo add remove image option
        $scope.onFileSelect = function ($files) {
            uploadService.decodeImage($files[0], function (image) {
                $scope.image = image;
                imageToUpload = $files[0];
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

app.controller('newTravelPropertiesController', ['$scope', '$location', '$routeParams', '$modal', 'propertyService',
    function ($scope, $location, $routeParams, $modal, propertyService) {

        //scope variables
        $scope.properties = [];
        $scope.properties.push({
            Title: 'Historia',
            Description: '',
            Icon: null,
            IconId: null
        });

        $scope.icons = propertyService.getIcons();

        //end scope variables

        //scope actions
        $scope.chooseIcon = function ($index) {

            var currentProperty = $index

            var modalInstance = $modal.open({
                templateUrl: 'app/views/travel/icon-modal-content.html',
                controller: 'IconModalController',
                resolve: {
                    icons: function () {
                        return $scope.icons
                    }
                }
            });

            modalInstance.result.then(function (iconIndex) {
                if (currentProperty != -1) {
                    $scope.properties[currentProperty].Icon = $scope.icons[iconIndex];
                    $scope.properties[currentProperty].IconId = $scope.icons[iconIndex].Id;

                }
            }, function () {
                //nothing happens
            });
        };

        $scope.addProperty = function () {
            $scope.properties.push({
                Title: '',
                Description: '',
                Icon: '',
                IconId: null
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

app.controller('IconModalController', ['$scope', '$modalInstance', 'icons',
    function ($scope, $modalInstance, icons) {

    $scope.icons = icons;

    $scope.setIcon = function ($index) {
        $modalInstance.close($index);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
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
                $location.path('/travel/show/' + $routeParams.id);
            });
        }
        //end scope actions
    }]);