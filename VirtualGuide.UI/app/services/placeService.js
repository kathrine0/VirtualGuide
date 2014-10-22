'use strict';

app.factory('placeService', ['placesRepository', 'placeRepository', 'placeCategoryRepository', function (placesRepository, placeRepository, placeCategoryRepository) {

    var placeService = {};

    placeService.getCategories = function (successCallback, errorCallback)
    {
        return placeCategoryRepository.query({ language: "pl_PL" });

    }

    placeService.placesToMarkers = function(places)
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
                    ImageSrc: place.ImageSrc
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

    placeRepository.updateItem = function (marker, successCallback, errorCallback) {

        var place = marker.place;
        place.Latitude = marker.lat;
        place.Longitude = marker.lng;

        propertyRepository.update({ id: place.Id }, place,
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