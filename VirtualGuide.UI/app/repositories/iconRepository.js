'use strict';

app.factory('iconRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/Icon', {}, {
        query: { method: 'GET', isArray: true }
    });

}]);