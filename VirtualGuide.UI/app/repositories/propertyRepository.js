'use strict';

app.factory('propertyRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/Properties/:id', {}, {
        query: { method: 'GET', isArray: true },
        show: { method: 'GET', isArray: true },
        create: { method: 'POST', params: { id: '@id' } },
        update: { method: 'PUT', params: { id: '@id' } },
        delete: { method: 'DELETE', params: { id: '@id' } }
    });

}]);