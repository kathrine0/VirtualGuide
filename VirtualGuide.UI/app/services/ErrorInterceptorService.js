'use strict';

///
/// GlobalError Handling
///
app.factory('errorInterceptorService', ['$q', '$rootScope', function ($q, $rootScope) {
    return {
        responseError: function (rejection) {
            console.log('rejection');
            if (rejection.status == 400)
            {
                var errors = {};

                for (var key in rejection.data.ModelState) {
                    errors[key] = [];
                    for (var i = 0; i < rejection.data.ModelState[key].length; i++) {
                        errors[key].push(rejection.data.ModelState[key][i]);
                    }
                }

                $rootScope.errors = errors;
                console.log($rootScope.errors['travel.Description'][0]);
                //$rootScope.$broadcast('errors', { 'errors': errors });
            }
            return $q.reject(rejection);
        }
    };
}]);