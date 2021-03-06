﻿var app = angular.module("MmMakerApp", []);


app.controller("AppController", function ($scope, $http, $sce) {
    $scope.excelContent = [];
    $scope.content;
    $scope.loadFile = function (files) {
        var file = files[0];

        var fd = new FormData();
        fd.append("file", file);

        $http({
            url: '/api/mmmakerapi/Transformfile',
            method: 'POST',
            headers: { 'Content-Type': undefined },
            data: fd
        }).then(function (result) {
            $scope.excelContent = $scope.excelContent.concat(result.data);
        });
    }
    $scope.sendExcelContent = function () {
        var data = JSON.stringify($scope.excelContent);

        $http({
            url: '/api/mmmakerapi/ExportToExcel',
            method: 'POST',
            data: data,
            responseType: 'arraybuffer'
        }).then(function (response) {
            var file = new Blob([response.data], { type: 'application/octet-stream' });
            var url = URL.createObjectURL(file);


            if (window.navigator.msSaveBlob) {  //IE
                window.navigator.msSaveBlob(file, "Scalony.xls");
            }
            else {

                var a = document.createElement('a');
                a.href = url;
                a.download = "test.xls";
                document.body.appendChild(a);
                a.click();
                a.remove();
            }

            URL.revokeObjectURL(url);
        });
    }
});


