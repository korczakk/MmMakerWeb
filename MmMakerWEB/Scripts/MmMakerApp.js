var app = angular.module("MmMakerApp", []);


app.controller("AppController",  function ($scope, $http, $sce) {
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
            var file = new Blob([response.data], { type: "application/vnd.ms-excel" });
            
            var url = URL.createObjectURL(file);
            
            $scope.content = $sce.trustAsResourceUrl(url);
        });
    }
});


