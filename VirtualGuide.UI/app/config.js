//global variables
app.run(function ($rootScope) {

    //WS config
    $rootScope.webservice = 'http://localhost/VirtualGuide/';

    //Maps config
    //Usage: 
    // var map = L.map('map').setView([51.505, -0.09], 13);
    // $rootScope.map.addTo(map);

    //Mapquest Maps
    var mapUrl = 'http://{s}.mqcdn.com/tiles/1.0.0/osm/{z}/{x}/{y}.png';
    var mapSettings = {
        attribution: 'Data, imagery and map information provided by <a href="http://open.mapquest.co.uk" target="_blank">MapQuest</a>, <a href="http://www.openstreetmap.org/" target="_blank">OpenStreetMap</a> and contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/" target="_blank">CC-BY-SA</a>',
        maxZoom: 20,
        minZoom: 1,
        subdomains: ['otile1', 'otile2', 'otile3', 'otile4']
    };

    // Open Street Maps
    //var mapUrl = 'http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
    //var mapSettings = {
    //    attribution: 'Map data © <a href="http://openstreetmap.org">OpenStreetMap</a> contributors',
    //    maxZoom: 20,
    //    minZoom: 1,
    //};


    $rootScope.map = new L.TileLayer(mapUrl, mapSettings);
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

