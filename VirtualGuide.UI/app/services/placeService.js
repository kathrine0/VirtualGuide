'use strict';

app.factory('placeService', ['$rootScope', '$q', 'placesRepository', 'placeRepository', 'placeCategoryRepository', 'uploadService',
    function ($rootScope, $q, placesRepository, placeRepository, placeCategoryRepository, uploadService) {

    var placeService = {};

    placeService.getCategories = function (successCallback, errorCallback)
    {
        return placeCategoryRepository.query({ language: "pl_PL" });

    }

    placeService.placesToMarkers = function (places, placeholder)
    {
        var markers = [];
        var id = 0;
        places.forEach(function (place) {
            var marker = {
                id: id++,
                lat: place.Latitude,
                lng: place.Longitude,
                focus: false,
                draggable: false,
                get message() {
                    //TODO - make anchor scroll to place (modify library)
                    //return '<a app-anchor="place'+this.id+'">' + this.place.Name + '</a>';
                    return this.place.Name;
                },
                place: {
                    Id: place.Id,
                    _name: place.Name,
                    set Name(value) {
                        this._name = value;
                    },
                    get Name() {
                        var value = this._name;
                        if (value == null || value == "") {
                            value = '\u00A0\u00A0'; //just space symbol for preservation of styles
                        }
                        return value;
                    },
                    Description: place.Description,
                    CategoryId: place.CategoryId,
                    CategoryName: place.CategoryName,
                    ImageSrc: place.ImageSrc,
                    _image: null,
                    set Image(value) 
                    {
                        this._image = value;
                    },
                    get Image() {
                        if (this._image != null) {
                            return this._image;
                        }
                        else if (this.ImageSrc != null) {
                            return $rootScope.webservice + this.ImageSrc;
                        } else {
                            return placeholder;
                        }
                    },
                    TravelId: place.TravelId
                }
            };

            markers.push(marker);
        });

        return markers;
    }

    placeService.activeIcon = function()
    {
        return {
            iconUrl: "data:image/png;base64,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",
            //iconUrl: 'Content/images/avatar3.png/activeMarker.png',
            //shadowUrl: '',
            iconSize: [29, 45], // size of the icon
            iconAnchor: [14, 50], // point of the icon which will correspond to marker's location
            //shadowAnchor: [4, 62],  // the same for the shadow
            popupAnchor: [0, -45] // point from which the popup should open relative to the iconAnchor
        };
    }

    placeService.createItems = function (markers, successCallback, errorCallback) {

        var places = [];

        markers.forEach(function (marker) {

            var place = marker.place;
            place.Latitude = marker.lat;
            place.Longitude = marker.lng;

            if (marker.imageToUpload != null)
            {
                var randomName = uploadService.randomName(place.Name, marker.imageToUpload.name);
                place.ImageSrc = randomName;
                uploadService.upload(marker.imageToUpload, randomName);
                marker.imageToUpload = null;
            }

            places.push(marker.place);
        });

        placesRepository.create({ id: travelId }, places,
            function success() {
                if (successCallback != undefined) {
                    successCallback();
                }
            },
            function error() {
                //errorCallback();
            });
    }

    placeService.updateItems = function (markers, successCallback, errorCallback)
    {
        var places = [];

        markers.forEach(function (marker) {

            var place = marker.place;
            place.Latitude = marker.lat;
            place.Longitude = marker.lng;

            if (marker.imageToUpload != null) {
                var randomName = uploadService.randomName(place.Name, marker.imageToUpload.name);
                place.ImageSrc = randomName;
                uploadService.upload(marker.imageToUpload, randomName);
                marker.imageToUpload = null;
            }

            places.push(marker.place);
        });

        placesRepository.update(places,
            function success() {
                if (successCallback != undefined) {
                    successCallback();
                }
            },
            function error() {
                //errorCallback();
            });
    }

    placeService.updateItem = function (marker, successCallback, errorCallback) {

        var place = marker.place;
        place.Latitude = marker.lat;
        place.Longitude = marker.lng;

        placeRepository.update({ id: place.Id }, place,
            function success() {
                if (successCallback != undefined) {
                    successCallback();
                }
            },
            function error() {
                //errorCallback();
            });
    }

    placeService.createItem = function (marker, successCallback, errorCallback)
    {
        var place = marker.place;
        place.Latitude = marker.lat;
        place.Longitude = marker.lng;

        placeRepository.create(place,
            function success(item) {
                if (successCallback != undefined) {
                    successCallback(item);
                }
            },
            function error(item) {
                //TODO
            })
    }


    return placeService;

}]);