'use strict';

app.factory('placeService', ['placeRepository', 'placeCategoryRepository', function (placeRepository, placeCategoryRepository) {

    var placeService = {};

    placeService.getCategories = function (successCallback, errorCallback)
    {
        return placeCategoryRepository.query({ language: "pl_PL" });

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