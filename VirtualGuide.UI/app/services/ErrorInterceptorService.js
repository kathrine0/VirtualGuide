'use strict';

///
/// GlobalError Handling
///
app.factory('errorInterceptorService', ['$q', function ($q) {
    return {
        responseError: function (rejection) {
            console.log(rejection);
            return $q.reject(rejection);
        }
    };
}]);