'use strict';

app.factory('travelService', ['travelRepository', 'travelApproveRepository',
    function (travelRepository, travelApproveRepository) {

    var travelService = {};
    
    travelService.getAllForCreator = function ()
    {
        return travelRepository.query();
    }

    travelService.getTravelForCreator = function (id, placeholder, successCallback)
    {
        var travel = travelRepository.show({id: id}, 
            function success(item) {
                if (successCallback != undefined)
                {
                    successCallback(item);
                }
            },
            function error(item) {
            });


        travel._image = null;
        travel.__defineGetter__("Image", function () {
            if (this._image != null) {
                return this._image;
            }
            else if (this.ImageSrc != null) {
                return $rootScope.webservice + this.ImageSrc;
            } else {
                return placeholder;
            }
        });
        travel.__defineSetter__("Image", function (value) {
            this._image = value;
        });

        return travel;
    }

    travelService.createItem = function (travel, successCallback, errorCallback)
    {
        travel.Language = 'pl_PL';

        travelRepository.create(travel,
            function success(item) {
                if (successCallback != undefined)
                {
                    successCallback(item);
                }
            }, 
            function error(item)
            {
                //TODO
            });
    }

    travelService.updateItem = function (travel, successCallback, errorCallback)
    {
        travelRepository.update({ id: travel.Id }, travel,
            function success(item) {
            if (successCallback != undefined)
            {
                successCallback(item);
            }
        }, function error(item) {
            if (errorCallback != undefined) {
                errorCallback(item);
            }
        });
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

    travelService.publishTravel = function (id, successCallback, errorCallback)
    {
        travelApproveRepository.update({ id: id })
    }

    return travelService;

}]);