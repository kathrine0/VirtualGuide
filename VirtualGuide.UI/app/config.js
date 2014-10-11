app.config(function ($provide, $httpProvider) {

    ///
    /// Adds Bearer Token to every request
    ///
    $httpProvider.interceptors.push('authInterceptorService');

    ///
    /// GlobalError Handling
    ///
    $httpProvider.interceptors.push('errorInterceptorService');

});
