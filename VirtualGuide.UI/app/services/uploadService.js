'use strict';

app.factory('uploadService', ['$timeout', 'uploadRepository', function ($timeout, uploadRepository) {
    
    var uploadService = {};

    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min)) + min;
    }

    uploadService.decodeImage = function(selectedFile, successCallback, errorCallback)
    {
        if (selectedFile != undefined && selectedFile.type.indexOf('image') > -1) {
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

    uploadService.randomName = function (base, original, additional) {

        var extension = original.split('.').pop();
        var strippedBase = base.replace(/(<([^>] +)>)/ig, "");
        var strippedAdd = "";

        if (strippedBase.length > 6) {
            strippedBase = strippedBase.substring(0, 6);
        }

        if (additional != undefined)
        {
            strippedAdd = additional.replace(/(<([^>] +)>)/ig, "");
            if (strippedAdd.length > 6) {
                strippedAdd = strippedAdd.substring(0, 6);
            }
        }

        return "Uploads/" + strippedBase + "_" + strippedAdd + getRandomInt(10000, 99999) + "." + extension;
    }

    return uploadService;

}]);