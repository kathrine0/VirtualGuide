'use strict';

app.factory('travelsService', ['$http', '$rootScope', function ($http, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return {
        getTravels: function () {
            return $http.get(serviceBase + 'api/travels');
        },
        addTravel: function (travel) {
            return $http.post(serviceBase, travel);
        },
        deleteTravel: function (travel) {
            return $http.delete(serviceBase + travel.Id);
        },
        updateTravel: function (travel) {
            return $http.put(serviceBase + travel.Id, travel);
        }
    };

    

    //return $resource(serviceBase + 'api/travels', {}, {
    //    query: { method: 'GET', isArray: true },
    //    create: { method: 'POST' }
    //});

    //var ordersServiceFactory = {};

    //var _getAllTravels = function () {

    //    return $http.get(serviceBase + 'api/travels').then(function (results) {
    //        return results;
    //    });
    //};

    //ordersServiceFactory.getAllTravels = _getAllTravels;

    //return ordersServiceFactory;

}]);

app.factory('travelService', ['$http', '$rootScope', function ($http, $rootScope) {

    //var serviceBase = $rootScope.webservice;

    //return $resource(serviceBase + '/api/travel/:id', {}, {
    //    show: { method: 'GET' },
    //    update: { method: 'PUT', params: { id: '@id' } },
    //    delete: { method: 'DELETE', params: { id: '@id' } }
    //});

}]);