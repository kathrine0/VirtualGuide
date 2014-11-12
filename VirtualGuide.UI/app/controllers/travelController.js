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

        
        $scope.travels = travelService.getAllForCreator();

        $scope.showTravel = function (id) {
            $location.path('/travel/show/' + id);
        };

        $scope.newTravel = function () {
            $location.path('/travel/new');
        };
}]);

app.controller('getTravelController', ['$scope', '$location', '$routeParams', '$modal', '$filter', '$anchorScroll', 'travelService', 'propertyService', 'placeService', 'uploadService', 'anchorSmoothScroll',
function ($scope, $location, $routeParams, $modal, $filter, $anchorScroll, travelService, propertyService, placeService, uploadService, anchorSmoothScroll) {

        //#region local variables

        var imageToUpload = null;
        var placeholder = "http://fpoimg.com/300x300?text=Place%20your%20image%20here";

        var markerOldValues = [];
        var lastPlaceId = 0;

        //#endregion local variables

        //#region scope variables

        $scope.map =
        {
            markers: [],
            center: {
                lat: 0,
                lng: 0,
                zoom: 1
            },
            defaults: {
                scrollWheelZoom: false,
            }
        };

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
            lastPlaceId = $scope.map.markers.length;
        });

        $scope.travel.editMode = false;

        $scope.PlaceAddMode = false;
    

        //#endregion scope variables

        //#region scope actions

        //#region scope travel actions

        $scope.onTravelImageSelect = function ($files) {
            uploadService.decodeImage($files[0], function (image) {
                $scope.travel.Image = image;
                imageToUpload = $files[0];
            });
        };

        $scope.editTravel = function () {
            $scope.travel.oldValue = {
                Name: $scope.travel.Name,
                Description: $scope.travel.Description,
                Price: $scope.travel.Price,
                Image: $scope.travel.Image
            };

            $scope.travel.editMode = true;
        };

        $scope.saveTravel = function () {
            if (imageToUpload !== null) {
                var randomName = uploadService.randomName($scope.travel.Name, imageToUpload.name);
                $scope.travel.ImageSrc = randomName;
                uploadService.upload(imageToUpload, randomName);
                imageToUpload = null;
            }

            travelService.updateItem($scope.travel);
            $scope.travel.editMode = false;

        };

        $scope.cancelTravelEdit = function () {
            $scope.travel = {
                Name: $scope.travel.oldValue.Name,
                Description: $scope.travel.oldValue.Description,
                Price: $scope.travel.oldValue.Price,
                Image: $scope.travel.oldValue.Image
            };

            $scope.travel.editMode = false;
        };

        //#endregion scope travel actions
            
        //#region scope property actions

            $scope.addProperty = function () {
                $scope.travel.Properties.push({
                    TravelId: $scope.travel.Id,
                    Title: '',
                    Description: '',
                    Icon: '',
                    IconId: null,
                    editMode: true,
                    isNew: true
                });
            };

            $scope.editProperty = function (index) {
                $scope.travel.Properties[index].oldValue = {};
                angular.copy($scope.travel.Properties[index], $scope.travel.Properties[index].oldValue);
                $scope.travel.Properties[index].editMode = true;
            };

            $scope.saveProperty = function (index) {

                if ($scope.travel.Properties[index].isNew) {
                    propertyService.createItem($scope.travel.Properties[index]);
                    $scope.travel.Properties[index].isNew = false;
                } else {
                    propertyService.updateItem($scope.travel.Properties[index]);
                }

                $scope.travel.Properties[index].editMode = false;
                delete $scope.travel.Properties[index].oldValue;
            };

            $scope.cancelPropertyEdit = function (index) {
                angular.copy($scope.travel.Properties[index].oldValue, $scope.travel.Properties[index]);
                delete $scope.travel.Properties[index].oldValue;
                $scope.travel.Properties[index].editMode = false;
            };

            $scope.removeProperty = function (index) {
                //todo remove on server
                delete $scope.travel.Properties[index].oldValue;
                $scope.travel.Properties.splice(index, 1);

            };

            $scope.chooseIcon = function ($index) {

                var currentProperty = $index;

            var modalInstance = $modal.open({
                templateUrl: 'app/views/travel/icon-modal-content.html',
                controller: 'IconModalController',
                resolve: {
                    icons: function () {
                        return $scope.icons;
                    }
                }
            });

            modalInstance.result.then(function (iconIndex) {
                if (currentProperty !== -1) {
                    $scope.travel.Properties[currentProperty].Icon = $scope.icons[iconIndex];
                    $scope.travel.Properties[currentProperty].IconId = $scope.icons[iconIndex].Id;

                }
            }, function () {
                //nothing happens
            });
        };
        //#endregion scope property actions

        //#regionscope place actions


            $scope.savePlace = function (marker) {

                if (marker.place.imageToUpload != null)
                {
                    var randomName = uploadService.randomName($scope.travel.Name, marker.place.imageToUpload.name, marker.place.Name);
                    marker.place.ImageSrc = randomName;
                    uploadService.upload(marker.place.imageToUpload, randomName);
                    marker.place.imageToUpload = null;
                }

                if (marker.isNew)
                {
                    placeService.createItem(marker, function (newplace) {
                        marker.place = newplace;
                        delete marker.draggable;
                        delete marker.icon;
                        marker.isNew = false;
                        marker.draggable = false;
                        $scope.PlaceAddMode = false;
                    });
                }
                else {
                    placeService.updateItem(marker);
                }

                marker.editMode = false;
            };

            $scope.removePlace = function (marker) {
                //todo remove on server
                var index = $scope.map.markers.indexOf(marker);
                $scope.map.markers.splice(index, 1);
            };

            $scope.onPlaceImageSelect = function ($files, marker) {
                uploadService.decodeImage($files[0], function (image) {
                    marker.place.ImageBase64 = image;
                    marker.place.imageToUpload = $files[0];
                });
            };

            $scope.showOnMap = function (marker)
            {
                $scope.scrollTo('Map')

                marker.focus = true;
                $scope.map.center = {
                    lat: marker.lat,
                    lng: marker.lng,
                    zoom: 15
                };
            }


        //#endregion place actions

        $scope.scrollTo = function (id) {
            $location.hash(id);
            anchorSmoothScroll.scrollTo(id);
        }

        //#endregion scope action

        //#region scope events

        $scope.$on('marker.focus', function (jsEvent, leafletEvent) {
            var marker = $filter('getByProperty')('id', leafletEvent.target.options.id, $scope.map.markers);
            marker.isOpen = true;
        });

        $scope.$on('leafletDirectiveMap.click', function (jsEvent, leafletEvent) {

            if ($scope.PlaceAddMode == 'marker')
            {
                var location = leafletEvent.leafletEvent.latlng;

                var searchValue = $("#leaflet-control-geosearch-qry").val();
                $("#leaflet-control-geosearch-qry").val("");

                var marker = createMarker(location, searchValue, placeholder);
                $scope.map.markers.push(marker);
                $scope.PlaceAddMode = 'input';
            }

        });

        
        var createMarker = function (location, searchValue, placeholder)
        {
            return  {
                id: lastPlaceId++,
                lat: location.lat,
                lng: location.lng,
                focus: false,
                draggable: true,
                icon: placeService.activeIcon(),
                get message() {
                    return this.place.Name;
                },
                editMode: true,
                isOpen: true,
                isNew: true,
                place: {
                    Id: 0,
                    Name: searchValue,
                    Description: "",
                    CategoryId: 0,
                    Image: placeholder,
                    TravelId: $scope.travel.Id,
                },
                image: placeholder,
                imageToUpload: null
            };
        }
        //#endregion scope events

}]);

//#region Travel Wizard

app.controller('newTravelController', ['$scope', '$rootScope', '$location', 'travelService', 'uploadService',
    function ($scope, $rootScope, $location, travelService, uploadService) {

        //#region local variables
        var imageToUpload = null;
        var placeholder = "http://fpoimg.com/1000x300?text=Place%20your%20image%20here";
        //#endregion local variables

        //#region scope variables
        $scope.travel = {};
        $scope.markers = [];
        $scope.image = placeholder;
        //#endregion scope variables

        //#region scope actions
        $scope.createNewTravel = function () {
            var travel = $scope.travel;
            travel.ZoomLevel = 12;
            travel.Latitude = $scope.markers[0].lat;
            travel.Longitude = $scope.markers[0].lng;

            if (imageToUpload !== null) {
                var randomName = uploadService.randomName($scope.travel.Name, imageToUpload.name);
                travel.ImageSrc = randomName;
                uploadService.upload(imageToUpload, randomName);
            }


            travelService.createItem(travel, function (newtravel) {
                $location.path('/travel/new/properties/' + newtravel.Id);
            });
        };

        $scope.onFileSelect = function ($files) {
            uploadService.decodeImage($files[0], function (image) {
                $scope.image = image;
                imageToUpload = $files[0];
            });
        };
        
        //#endregion scope actions

        //#region scope events
        $scope.$on('leafletDirectiveMap.geosearch_showlocation', function (jsEvent, leafletEvent) {
            var location = leafletEvent.leafletEvent.Location;
            $scope.markers = travelService.manageMarkers($scope.markers, parseFloat(location.Y), parseFloat(location.X));
        });

        $scope.$on('leafletDirectiveMap.click', function (jsEvent, leafletEvent) {
            var location = leafletEvent.leafletEvent.latlng;
            $scope.markers = travelService.manageMarkers($scope.markers, location.lat, location.lng);

        });
        //#endregion scope events
}]);

app.controller('newTravelPropertiesController', ['$scope', '$location', '$routeParams', '$modal', 'propertyService',
    function ($scope, $location, $routeParams, $modal, propertyService) {

        //#region scope variables
        $scope.properties = [];
        $scope.properties.push({
            Title: 'Historia',
            Description: '',
            Icon: null,
            IconId: null
        });

        $scope.icons = propertyService.getIcons();

        //#endregion scope variables

        //#region scope actions
        $scope.chooseIcon = function ($index) {

            var currentProperty = $index;

            var modalInstance = $modal.open({
                templateUrl: 'app/views/travel/icon-modal-content.html',
                controller: 'IconModalController',
                resolve: {
                    icons: function () {
                        return $scope.icons;
                    }
                }
            });

            modalInstance.result.then(function (iconIndex) {
                if (currentProperty !== -1) {
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
                IconId: null,
                TravelId: $routeParams.id
            });
        };

        $scope.removeProperty = function (index) {
            $scope.properties.splice(index, 1);
        };

        $scope.saveProperties = function () {
            propertyService.createItems($scope.properties, function () {
                $location.path('/travel/new/places/' + $routeParams.id);
            });
        };
        //#endregion scope actions

    }]);

app.controller('newTravelPlacesController', ['$scope', '$location', '$filter', '$routeParams', 'placeService', 'uploadService',
function ($scope, $location, $filter, $routeParams, placeService, uploadService) {

    //#region local variables
    var editMode = false;
    var id = 0;
    var placeholder = "http://fpoimg.com/300x300?text=Place%20your%20image%20here";
    //#endregion local variables

    //#region scope variables
    $scope.activeMarker = null;
    $scope.markers = [];
    $scope.categories = placeService.getCategories();
    //#endregion scope variables

    //#region scope events
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
                    if (value === null || value === "") {
                        value = '\u00A0\u00A0'; //just space symbol for preservation of styles
                    }
                    return value;
                },
                Description: " ",
                CategoryId: 0,
                TravelId: $routeParams.id,
            },
            image: placeholder,
            imageToUpload: null
        };
            
        $scope.activeMarker = marker;
        $scope.markers.push(marker);

        editMode = true;

    });
    //#endregion scope events

    //#region scope actions
    $scope.removeMarker = function (index) {
        $scope.activeMarker = null;
        $scope.markers.splice(index, 1);
    };

    $scope.focusMarker = function (index) {
        $scope.markers[index].focus = true;
    };

    $scope.savePlaces = function () {

        placeService.createItems($scope.markers, function () {
            $location.path('/travel/show/' + $routeParams.id);
        });
    };

    $scope.onImageSelect = function ($files) {
        uploadService.decodeImage($files[0], function (image) {
            $scope.activeMarker.image = image;
            $scope.activeMarker.imageToUpload = $files[0];
        });
    };
    //#endregion scope actions
}]);

//#endregion

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