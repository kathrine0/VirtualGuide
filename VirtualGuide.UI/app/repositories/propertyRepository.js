'use strict';

app.factory('propertyRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/CreatorTravel/:id', {}, {
        query: { method: 'GET', isArray: true },
        show: { method: 'GET' },
        create: { method: 'POST' },
        update: { method: 'PUT', params: { id: '@id' } },
        delete: { method: 'DELETE', params: { id: '@id' } }
    });

}]);