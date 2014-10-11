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