'use strict';

app.factory('propertyService', ['propertiesRepository', 'propertyRepository', 'iconRepository', function (propertiesRepository, propertyRepository, iconRepository) {

    var propertyService = {};

    propertyService.createItem = function (property, successCallback, errorCallback) {
        propertyRepository.create(property,
            function success() {
                if (successCallback != undefined) {
                    successCallback();
                }
            },
            function error() {
                //errorCallback();
            });
    }

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

    ///
    /// Get available icons to choose from for the property
    ///
    propertyService.getIcons = function(successCallback, errorCallback)
    {
        return iconRepository.query(
            function success() {
                if (successCallback != undefined) {
                    successCallback();
                }
            },
            function error() {
                //errorCallback();
            }
        );
    }

    return propertyService;

}]);