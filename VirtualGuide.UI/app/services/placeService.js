'use strict';

app.factory('placeService', ['placeRepository', 'placeCategoryRepository', function (placeRepository, placeCategoryRepository) {

    var placeService = {};

    placeService.getCategories = function (successCallback, errorCallback)
    {
        return placeCategoryRepository.query({ language: "pl_PL" });

    }

    //placeService.createItems = function (properties, travelId, successCallback, errorCallback) {

    //    travelRepository.create({ id: travelId }, properties,
    //        function success() {
    //            successCallback();
    //        },
    //        function error() {
    //            errorCallback();
    //        });
    //}

    return placeService;

}]);