﻿'use strict';

///
/// Get properties for travel by ID
///
app.factory('propertiesRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/Properties/:id', {}, {
        show: { method: 'GET', isArray: true },
        create: { method: 'POST' }
    });

}]);

app.factory('propertyRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/Property/:id', {}, {
        show: { method: 'GET', isArray: true },
        create: { method: 'POST' },
        update: { method: 'PUT', params: { id: '@id' } },
        delete: { method: 'DELETE', params: { id: '@id' } }
    });

}]);