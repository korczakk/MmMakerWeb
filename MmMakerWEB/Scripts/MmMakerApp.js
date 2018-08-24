var app = angular.module("MmMakerApp", []);


app.controller("AppController",  function ($scope, $http) {
    $scope.excelContent = [];
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

    }
});


