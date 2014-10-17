'use strict';

app.factory('propertyService', ['propertyRepository', function (travelRepository) {

    var propertyService = {};

    propertyService.createItems = function (properties, travelId, successCallback, errorCallback) {

        travelRepository.create({ id: travelId }, properties,
            function success() {
                successCallback();
            },
            function error() {
                errorCallback();
            });
    }

    return propertyService;

}]);