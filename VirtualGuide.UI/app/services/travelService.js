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

    travelService.manageMarkers = function(markers, lat, lng)
    {
        if (markers.length == 0) {
            var marker = {
                lat: lat,
                lng: lng,
                focus: true,
                draggable: true
            };
            markers.push(marker);
        } else {
            markers[0].lat = lat;
            markers[0].lng = lng;
        }

        return markers;
    }

    return travelService;

}]);