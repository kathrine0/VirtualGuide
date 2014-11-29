'use strict';

app.factory('uploadService', ['$timeout', 'uploadRepository', function ($timeout, uploadRepository) {

    var uploadService = {};

    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min)) + min;
    }

    function resize(data, min_width, min_height, imageEncoding)
    {
        var imageObj = new Image(),
        canvas = document.createElement('canvas'),
        context = null,
        blob = null,
        width = 0,
        height = 0;

        imageObj.src = data;
        var ratio = imageObj.width / imageObj.height;

        if (imageObj.width > imageObj.height) {
            height = min_height;
            width = height * ratio;
        } else {
            width = min_width;
            height = width * 1/ratio;
        }

        //create a hidden canvas object we can use to create the new resized image data
        canvas.id = "hiddenCanvas";
        canvas.width = width;
        canvas.height = height;
        canvas.style.visibility = "hidden";
        document.body.appendChild(canvas);

        //get the context to use 
        context = canvas.getContext('2d');
        context.clearRect(0, 0, width, height);
        context.drawImage(imageObj, 0, 0, imageObj.width, imageObj.height, 0, 0, width, height);

        return canvas.toDataURL(imageEncoding);
    }

    function dataURItoBlob(dataURI, name) {
        // convert base64/URLEncoded data component to raw binary data held in a string
        var byteString;
        if (dataURI.split(',')[0].indexOf('base64') >= 0)
            byteString = atob(dataURI.split(',')[1]);
        else
            byteString = unescape(dataURI.split(',')[1]);

        // separate out the mime component
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

        // write the bytes of the string to a typed array
        var ia = new Uint8Array(byteString.length);
        for (var i = 0; i < byteString.length; i++) {
            ia[i] = byteString.charCodeAt(i);
        }

        return new File([ia], name, { type: mimeString });
    }

    uploadService.decodeImage = function (selectedFile, successCallback, errorCallback) {
        if (selectedFile != undefined && selectedFile.type.indexOf('image') > -1) {
            var fileReader = new FileReader();
            fileReader.readAsDataURL(selectedFile);
            var loadFile = function (fileReader) {
                fileReader.onload = function (e) {
                    var data = e.target.result;
                    
                    var resizedData = resize(data, 300, 300, selectedFile.type);
                    var resizedFile = dataURItoBlob(resizedData, selectedFile.name);

                    $timeout(function () {
                        if (successCallback != undefined) {
                            successCallback(resizedData, resizedFile)
                        }
                    });
                }
            }(fileReader);
        } else {
            if (errorCallback != undefined) {
                return errorCallback();
            }
        }
    }

    uploadService.upload = function (fileUploadObj, filename) {
        uploadRepository.upload(fileUploadObj, filename);
    }

    uploadService.randomName = function (base, original, additional) {

        var extension = original.split('.').pop();
        var strippedBase = base.replace(/(<([^>] +)>)/ig, "");
        var strippedAdd = "";

        if (strippedBase.length > 6) {
            strippedBase = strippedBase.substring(0, 6);
        }

        if (additional != undefined) {
            strippedAdd = additional.replace(/(<([^>] +)>)/ig, "");
            if (strippedAdd.length > 6) {
                strippedAdd = strippedAdd.substring(0, 6);
            }
        }

        return "Uploads/" + strippedBase + "_" + strippedAdd + getRandomInt(10000, 99999) + "." + extension;
    }

    return uploadService;

}]);