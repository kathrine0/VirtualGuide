﻿'use strict';

app.factory('placeCategoryRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/Places/Categories/:language', {}, {
        query: { method: 'GET', isArray: true, params: { language: '@language' } },
        
    });

}]);

app.factory('placesRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/Places/:id', {}, {
        query: { method: 'GET', isArray: true, params: { id: '@id' } },
        create: { method: 'POST'},
        update: { method: 'PUT', params: { id: '@id' } },
        delete: { method: 'DELETE', params: { id: '@id' } }
    });

}]);

app.factory('placeRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/Place/:id', {}, {
        show: { method: 'GET'},
        create: { method: 'POST' },
        update: { method: 'PUT', params: { id: '@id' } },
        delete: { method: 'DELETE', params: { id: '@id' } }
    });

}]);