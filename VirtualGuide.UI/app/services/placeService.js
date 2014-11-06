'use strict';

app.factory('placeService', ['$rootScope', 'placesRepository', 'placeRepository', 'placeCategoryRepository', 'uploadService',
    function ($rootScope, placesRepository, placeRepository, placeCategoryRepository, uploadService) {

    var placeService = {};

    placeService.getCategories = function (successCallback, errorCallback)
    {
        return placeCategoryRepository.query({ language: "pl_PL" });

    }

    placeService.placesToMarkers = function(places, placeholder)
    {
        var markers = [];
        places.forEach(function (place) {
            var marker = {
                lat: place.Latitude,
                lng: place.Longitude,
                focus: false,
                draggable: false,
                get message() {
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
                    ImageBase64: null,
                    _image: placeholder,
                    set Image(value) 
                    {
                        this._image = value;
                    },
                    get Image() {
                        if (this.ImageBase64 != null)
                        {
                            return this.ImageBase64;
                        }
                        else if (this.ImageSrc) {
                            return $rootScope.webservice + this.ImageSrc;
                        } else {
                            return placeholder
                        }
                    },
                    TravelId: place.TravelId
                }
            };

            markers.push(marker);
        });

        return markers;
    }

    placeService.createItems = function (markers, travelId, successCallback, errorCallback) {

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


    return placeService;

}]);