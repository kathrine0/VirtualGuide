'use strict';

app.factory('propertyService', ['propertyRepository', function (propertyRepository) {

    var propertyService = {};

    propertyService.createItems = function (properties, travelId, successCallback, errorCallback) {

        propertyRepository.create({ id: travelId }, properties,
            function success() {
                successCallback();
            },
            function error() {
                //errorCallback();
            });
    }

    return propertyService;

}]);