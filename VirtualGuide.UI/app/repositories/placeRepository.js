'use strict';

app.factory('placeCategoryRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {

    var serviceBase = $rootScope.webservice;

    return $resource(serviceBase + 'api/Place/Categories/:language', {}, {
        query: { method: 'GET', isArray: true, params: { language: '@language' } },
        
    });

}]);

app.factory('placeRepository', ['$resource', '$rootScope', function ($resource, $rootScope) {
}]);