'use strict';

//global variables
app.run(function ($rootScope) {

    //WS config
    $rootScope.webservice = 'http://localhost/VirtualGuide/';

    //Maps config
    $rootScope.tiles = {
        url: "http://{s}.mqcdn.com/tiles/1.0.0/osm/{z}/{x}/{y}.png",
        options: {
            attribution: 'Data, imagery and map information provided by <a href="http://open.mapquest.co.uk" target="_blank">MapQuest</a>, <a href="http://www.openstreetmap.org/" target="_blank">OpenStreetMap</a> and contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/" target="_blank">CC-BY-SA</a>',
            subdomains: ['otile1', 'otile2', 'otile3', 'otile4'],
            maxZoom: 20,
            minZoom: 1
        }
    };

    //var geo = new L.Control.GeoSearch({ provider: new L.GeoSearch.Provider.OpenStreetMap() });

    $rootScope.controls = {
        custom: [new L.Control.GeoSearch({
            provider: new L.GeoSearch.Provider.OpenStreetMap(),
            showMarker: false
        })]
    };

    $rootScope.center = {
        lat: 51.505,
        lng: -0.09,
        zoom: 3
    };

    //angular.extend($scope, {
    //    defaults: {
    //        maxZoom: 20,
    //        minZoom: 1,
    //        path: {
    //            weight: 10,
    //            color: '#800000',
    //            opacity: 1
    //        }
    //    },  
    //});


    // Open Street Maps
    //var mapUrl = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';


    //$rootScope.map = new L.TileLayer(mapUrl, mapSettings);
});


    
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

