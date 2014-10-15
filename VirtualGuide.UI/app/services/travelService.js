'use strict';

app.factory('travelService', ['travelRepository', function (travelRepository) {

    var travelService = {};
    
    travelService.getAllForCreator = function ()
    {
        return travelRepository.query();
    }

    travelService.getItem = function (id)
    {
        return travelRepository.show({id: id}, 
            function success(item) {
            },
            function error(item) {
            });
    }

    travelService.createItem = function (travel, successCallback, errorCallback)
    {
        travel.Language = 'pl_PL';

        travelRepository.create(travel, 
            function success(item) {
                successCallback(item);
            }, 
            function error(item)
            {
                //TODO
            });
    }

    travelService.updateItem = function (travel)
    {
        travelRepository.update(travel)
    }

    travelService.deleteItem = function ()
    {

    }

    return travelService;

}]);