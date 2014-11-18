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

app.filter('errorLabel', ['$rootScope', '$sce', '$translate', function ($rootScope, $sce, $translate) {
    return function (property) {

        if (property == null ||
            property == '' ||
            property == undefined ||
            !$rootScope.errors ||
            !$rootScope.errors[property]) {
            return "";
        }

        var markup = "<label ng-show=\"" + $rootScope.errors[property] +"\">"+
                        "<i class=\"fa fa-times-circle-o\"></i>"+
                        "<span> " + $translate.instant($rootScope.errors[property][0]) + "</span>" +
                     "</label>";

        return $sce.trustAsHtml(markup);
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