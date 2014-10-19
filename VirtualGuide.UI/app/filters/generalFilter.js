'use strict';

app.filter('imageSource', ['$rootScope', '$sce', function ($rootScope, $sce) {
    return function (str, customClass) {

        if (str == null || str == '' || str == undefined)
        {
            return "";
        }

        var path = $rootScope.webservice + str;

        return $sce.trustAsHtml('<img class="' + customClass + '" src="' + path + '" />');
    };
}]);

app.filter('getByProperty', function() {
    return function(propertyName, propertyValue, collection) {
        var i=0, len=collection.length;
        for (; i<len; i++) {
            if (collection[i][propertyName] == +propertyValue) {
                return collection[i];
            }
        }
        return null;
    }
});