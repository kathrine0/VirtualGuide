'use strict';

app.factory('propertyService', ['propertiesRepository', 'propertyRepository', function (propertiesRepository, propertyRepository) {

    var propertyService = {};

    propertyService.createItems = function (properties, travelId, successCallback, errorCallback) {

        propertiesRepository.create({ id: travelId }, properties,
            function success() {
                successCallback();
            },
            function error() {
                //errorCallback();
            });
    }

    propertyService.updateItem = function (property, successCallback, errorCallback) {

        propertyRepository.update({ id: property.Id }, property,
            function success() {
                if (successCallback != undefined)
                {
                    successCallback();
                }
            },
            function error() {
                //errorCallback();
            });
    }

    return propertyService;

}]);