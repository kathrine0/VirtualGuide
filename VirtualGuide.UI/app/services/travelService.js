'use strict';

app.factory('travelService', ['travelRepository', function (travelRepository) {


    
    var _getAllForCreator = function ()
    {
        return travelRepository.query();
    }

    var _getItem = function(id)
    {
        return travelRepository.show({id: id});
    }

    var _createItem = function(travel)
    {
        travel.Language = 'pl_PL';

        travelRepository.create(travel);
    }

    var _updateItem = function(travel)
    {
        travelRepository.update(travel)
    }

    var _deleteItem = function()
    {

    }

    return {
        getAllForCreator: _getAllForCreator,
        createItem: _createItem,
        getItem: _getItem,
        updateItem: _updateItem
    }


}]);