'use strict';
app.controller('uploadController', ['$scope', '$http', '$timeout', '$upload',
    function ($scope, $http, $timeout, $upload) {
        $scope.upload = [];
        $scope.fileUploadObj = { testString1: "Test string 1", testString2: "Test string 2" };

        $scope.onFileSelect = function (f) {
            //$files: an array of files selected, each file has name, size, and type.


            if ($scope.fileReaderSupported && f.type.indexOf('image') > -1) {
                var fileReader = new FileReader();
                fileReader.readAsDataURL(f);
                var loadFile = function (fileReader) {
                    fileReader.onload = function (e) {
                        $timeout(function () {
                            $scope.dataUrl = e.target.result;
                        });
                    }
                }(fileReader);
            } else {
                //todo error
            }           
        }

        $scope.abortUpload = function (index) {
            $scope.upload[index].abort();
        }
    }]);