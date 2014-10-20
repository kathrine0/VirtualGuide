'use strict';

app.factory('uploadService', ['$timeout', 'uploadRepository', function ($timeout, uploadRepository) {
    
    var uploadService = {};

    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min)) + min;
    }

    uploadService.decodeImage = function(selectedFile, successCallback, errorCallback)
    {
        if (selectedFile.type.indexOf('image') > -1) {
            var fileReader = new FileReader();
            fileReader.readAsDataURL(selectedFile);
            var loadFile = function (fileReader) {
                fileReader.onload = function (e) {
                    $timeout(function () {
                        if (successCallback != undefined)
                        {
                            successCallback(e.target.result)
                        }
                    });
                }
            }(fileReader);
        } else {
            if (errorCallback != undefined)
            {
                return errorCallback();
            }
        }
    }

    uploadService.upload = function (fileUploadObj, filename)
    {
        uploadRepository.upload(fileUploadObj, filename);
    }

    uploadService.randomName = function (base, original) {

        var extension = original.split('.').pop();
        var stripped = base.replace(/(<([^>] +)>)/ig, "");

        if (stripped.lenght > 6) {
            stripped = stripped.substring(0, 6);
        }
        return "Uploads/" + stripped + getRandomInt(10000, 99999) + "." + extension;
    }

    return uploadService;

}]);