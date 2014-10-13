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

app.config(['GoogleMapApiProvider'.ns(), function (GoogleMapApi) {
    GoogleMapApi.configure({
        //key: 'AIzaSyDAHllI7-OdTPNly6FabANyid-W2xbhyM4',
        v: '3.17',
        libraries: 'places,geometry,drawing'
    });
}])
