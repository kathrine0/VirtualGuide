'use strict';

app.factory('CreatorTravelsService', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/CreatedTravels', {}, {
        query: { method: 'GET', isArray: true },
        create: { method: 'POST' }
    });


}]);

app.factory('CreatorTravelService', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + '/api/travel/:id', {}, {
        show: { method: 'GET' },
        update: { method: 'PUT', params: { id: '@id' } },
        delete: { method: 'DELETE', params: { id: '@id' } }
    });

}]);