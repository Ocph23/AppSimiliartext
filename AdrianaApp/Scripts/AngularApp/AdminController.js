angular.module("App.Admin", ["ngProgress"])

.controller("IndexController",function ($scope, $http) {
    $scope.Judul = "Index";
    $scope.upload=function()
    {
        
    }
})

.controller("JudulController", function ($scope, $http,$sce, ngProgressFactory) {

    $scope.Judul = "DAFTAR JUDUL";
    $scope.Juduls = [];
    $scope.progressbar = ngProgressFactory.createInstance();
    $scope.Init = function () {

        var url = "/api/Abstract/Get";
        $http({
            method: 'Get',
            url: url
        }).success(
            function (data, status, header, cfg) {
                $scope.Abstracts = data;
            }

        ).error(function (err, status) {
            alert("err");
        });



        var url = "/api/Mahasiswa/Get";
        $http({
            method: 'Get',
            url: url
        }).success(
            function (data, status, header, cfg) {
                $scope.Mahasiswa = data;
            }

        ).error(function (err, status) {
            alert("err");
        });

    };
    $scope.AddNewAbstract = function (item) {

        var f = document.getElementById("file");
        var res = f.files[0];

        var form = new FormData();

        form.append("file", res);
        form.append("Judul", item.Judul);
        form.append("Pembimbing1", item.Pembimbing1);
        form.append("Pembimbing2", item.Pembimbing2);
        form.append("NPM", item.NPM);
        form.append("Nama", item.Nama);
        form.append("Jurusan", item.Jurusan);
        form.append("Alamat", item.Alamat);

        var settings = {
            "async": true,
            "crossDomain": true,
            "url": "/api/upload/post",
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
                item.Id = data;
                $scope.Abstracts.push(item);
                alert("Berhasil menambah data");
            } else {
                $scope.progressbar.reset();
                alert(["Gagal Menambahkan data"]);
            }
        }).error(function (err) {
            $scope.progressbar.reset();
            alert(["Gagal Menambahkan data"]);
        });


    };

    $scope.RemoveAbstract = function (item) {
        var url = "/api/Abstract/Delete?Id=" + item.Id;
        $http({
            method: 'Delete',
            url: url,
            data: item
        }).success(
            function (data, status, header, cfg) {
                $scope.Abstracts.splice($scope.Abstracts.indexOf(item), 1);
                alert(data);
            }

        ).error(function (err, status) {
            alert(err.Message);
        });
    }

    $scope.SelectedAbstract = function (item) {
        $scope.Selected = item;
    }

    $scope.UpdateAbstract=function(item)
    {
        var url = "/api/abstract/Put";
        $http({
            method: 'Put',
            url: url,
            data: item
        }).success(
            function (data, status, header, cfg) {
                alert("Data Mahasiswa Berhasil Diperbaharui");
            }

        ).error(function (err, status) {
            alert(err.Message);
        });
    }


    $scope.ViewAbstract = function (item) {
        var url = "/api/Kemiripan/View?Id=" + item.Id;
        $http({
            method: 'Get',
            url: url
        }).success(
            function (data, status, header, cfg) {
                $scope.PageHtml = $sce.trustAsHtml(data);
            }

        ).error(function (err, status) {
            alert(err.Message);
        });
    }


})


.controller("TambahJudulController", function ($scope, $http) {
    $scope.Judul = "Data Mahasiswa";
   

})


.controller("MahasiswaController", function ($scope, $http) {
    $scope.Judul = "DATA MAHASISWA";
    $scope.Mahasiswa = [];
    $scope.Init = function () {
       
        var url = "/api/Mahasiswa/Get";
        $http({
            method: 'Get',
            url: url
        }).success(
            function (data, status, header, cfg) {
                $scope.Mahasiswa = data;
            }

        ).error(function (err, status) {
            alert("err");
        });
    };


    $scope.AddNewMahasiswa = function (item) {
        var url = "/api/Mahasiswa/Post";
        $http({
            method: 'Post',
            url: url,
            data:item
        }).success(
            function (data, status, header, cfg) {
                item.Id = data;
                $scope.Mahasiswa.push(item);
                alert("Data Mahasiswa Berhasil Ditambahkan");
            }

        ).error(function (err, status) {
            alert(err.Message);
        });
    };

    $scope.RemoveMahasiswa = function (item) {
        var url = "/api/Mahasiswa/Delete?Id="+item.Id;
        $http({
            method: 'Delete',
            url: url,
            data: item
        }).success(
            function (data, status, header, cfg) {
                $scope.Mahasiswa.splice($scope.Mahasiswa.indexOf(item),1);
                alert(data);
            }

        ).error(function (err, status) {
            alert(err.Message);
        });
    };

    $scope.UpdateMahasiswa = function (item) {
        var url = "/api/Mahasiswa/Put";
        $http({
            method: 'Put',
            url: url,
            data: item
        }).success(
            function (data, status, header, cfg) {
                alert("Data Mahasiswa Berhasil Diperbaharui");
            }

        ).error(function (err, status) {
            alert(err.Message);
        });
    };

    $scope.SelectedMahasiswa = function (item) {
        $scope.Selected = item;

    }

})


;