'use strict';

app.factory('uploadRepository', ['$upload', '$rootScope', function ($upload, $rootScope) {

    var serviceBase = $rootScope.webservice;
    var uploadRepository = {};

    uploadRepository.upload = function (fileUploadObj, filename, progressCallback, successCallback, errorCallback)
    {
        $upload.upload({
            url: serviceBase + "api/Upload", // webapi url
            method: "POST",
            data: { filename: filename},
            file: fileUploadObj
        }).progress(function (evt) {

            if (progressCallback != undefined)
            {
                progressCallback(evt);
            }

            // get upload percentage
            console.log('percent: ' + parseInt(100.0 * evt.loaded / evt.total));
        }).success(function (data, status, headers, config) {

            if (successCallback != undefined)
            {
                successCallback(data, status, headers, config);
            }

            // file is uploaded successfully
            console.log(data);
        }).error(function (data, status, headers, config) {

            if (errorCallback != undefined) {
                errorCallback(data, status, headers, config);
            }
            // file failed to upload
            console.log(data);
        });
    }

    return uploadRepository;
}]);