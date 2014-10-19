'use strict';

app.factory('placeService', ['placeRepository', 'placeCategoryRepository', function (placeRepository, placeCategoryRepository) {

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

        placeRepository.create({ id: travelId }, places,
            function success() {
                successCallback();
            },
            function error() {
                //errorCallback();
            });
    }


    return placeService;

}]);