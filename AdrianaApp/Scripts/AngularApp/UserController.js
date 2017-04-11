angular.module("App.User", ["ngProgress"])

.controller("UserIndexController", function ($scope, $http) {
    $scope.Judul = "User Index";
    $scope.upload = function () {

    }
})

.controller("DeteksiController", function ($scope, $http,$sce,ngProgressFactory) {
    $scope.Judul = "Deteksi Kemiripan";
    $scope.Results = [];
    $scope.HaveResult = false;
    $scope.progressbar = ngProgressFactory.createInstance();
    $scope.model = {};

    $scope.Init = function () {

        $scope.model.TandaBaca = true;
        $scope.model.Angka = true;
        $scope.model.HurufBesar = true;
    }
    $scope.Proccess = function (item) {
        $scope.Results = [];
        var f = document.getElementById("file");
        var res = f.files[0];
        if (res.type == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
        {
            var form = new FormData();

            form.append("file", res);
            form.append("Judul", item.Judul);
            form.append("Angka", item.Angka);
            form.append("HurufBesar", item.HurufBesar);
            form.append("TandaBaca", item.TandaBaca);

       
            var settings = {
                "async": true,
                "crossDomain": true,
                "url": "/api/upload/proccess",
                "method": "POST",
                "headers": {
                    "cache-control": "no-cache",
                },
                "processData": false,
                "contentType": false,
                "mimeType": "multipart/form-data",
                "data": form
            };
            $scope.progressbar.start();

            $.ajax(settings).done(function (data, response) {
                if (response == "success") {
                    $scope.progressbar.complete();
                    $scope.HaveResult = true;
                    var newData = angular.fromJson(data);
                    $scope.Results = newData.Data;
                    $scope.NewAbstract = $sce.trustAsHtml(newData.StrHTML);
                    $scope.$apply();
              
                } else {
                    $scope.progressbar.reset();
                    alert("Gagal Menambahkan data");
                }
            }).error(function (err) {
                $scope.progressbar.reset();
                alert("Gagal Menambahkan data");
            });
        }else
        {
            alert("Pastikan Document Anda Adalah .docx")
        }
    };

    $scope.View = function (item) {
        var url = "/api/Kemiripan/View?Id="+item.Id;
        $http({
            method: 'Get',
            url: url
        }).success(
            function (data, status, header, cfg) {
                $scope.PageHtml =$sce.trustAsHtml(data);
            }

        ).error(function (err, status) {
            alert(err.Message);
        });
    }
})

.controller("AboutController", function ($scope, $http) {
    $scope.Judul = "About";
    $scope.upload = function () {

    }
})



;