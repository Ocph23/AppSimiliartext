angular.module("App.User", ["ngProgress"])

.controller("UserIndexController", function ($scope, $http) {
    $scope.Judul = "User Index";
    $scope.upload = function () {

    }
})

.controller("DeteksiController", function ($scope, $rootScope,$location, $http, $sce, ngProgressFactory,$timeout) {
    $scope.Judul = "DETEKSI KEMIRIPAN";
    $scope.Results = [];
    $scope.HaveResult = false;
    $scope.progressbar = ngProgressFactory.createInstance();
    $scope.progressbar.setHeight("5px");
    

    $scope.model = {};
    $scope.NewFileTipe = "";
    $scope.Init = function () {
       
        $scope.model.TandaBaca = true;
        $scope.model.Angka = true;
        $scope.model.HurufBesar = true;
        document.getElementById('fake-file-button-browse').addEventListener('click', function () {
            document.getElementById('file').click();
        });

        document.getElementById('file').addEventListener('change', function () {
            document.getElementById('fake-file-input-name').value = this.value;
        });

    }


    $scope.Proccess = function (item) {
        $scope.Results = [];
        $scope.progressbar.start();
        var f = document.getElementById("file");
        var res = f.files[0];
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
      
        $scope.NewFileTipe = res.type;
        
        $.ajax(settings).done(function (data, response) {
            if (response == "success") {
                $scope.progressbar.complete();
                $scope.HaveResult = true;
                var newData = angular.fromJson(data);
                $scope.Results = newData.Data;
                $scope.StrHTML = newData.StrHTML;

            } else {
                $scope.progressbar.reset();
                alert("Gagal Menambahkan data");
            }
        }).error(function (err) {
            $scope.progressbar.reset();
            alert(err.responseText);
        });
    };
   
   $scope.readyCallback = function () {
        Materialize.toast("Modal ready", 1000);
    }
   $scope.completeCallback = function () {
        Materialize.toast("Modal complete", 1000);
    }

   $scope.View = function (item) {

        var url = "/api/Kemiripan/View?Id="+item.Id;
        $http({
            method: 'Get',
            url: url
        }).success(
            function (data, status, header, cfg) {
                $scope.FileTipe = item.FileTipe;
                $scope.PageHtml = {};
                if (item.FileTipe == "application/pdf") {
                    $scope.PageHtml = $sce.trustAsResourceUrl('data:' + item.FileTipe + ';base64,' + data);
                  
                } else {
                    $scope.PageHtml = $sce.trustAsHtml(data);
                }

                if ($scope.NewFileTipe == "application/pdf") {
                    $scope.NewAbstract = $sce.trustAsResourceUrl('data:' + $scope.NewFileTipe + ';base64,' + $scope.StrHTML);
                } else {
                    $scope.NewAbstract = $sce.trustAsHtml($scope.StrHTML);
                }

                
            }

        ).error(function (err, status) {
            alert(err.Message);
        });

   }
   $scope.ViewProccess = function (item)
   {
       $scope.SelectedItem = item;
   }
   $scope.Print = function (user,model)
   {
       $rootScope.Result = {};
       $rootScope.Result.User = user;
       $rootScope.Result.Model = model;
       $rootScope.Result.Data = $scope.Results;
       $location.url("/ReportAnalisa")
   }
})

.controller("AboutController", function ($scope, $http) {
    $scope.Judul = "About";
    $scope.upload = function () {

    }
})


    .controller("ReportAnalisaController", function ($rootScope,$location, $scope, $route, $http,$window,$timeout) {
        $scope.Results = [];
        $scope.Title = "HASIL ANALISA KEMIRIPAN DOKUMEN";
        $scope.Result = $rootScope.Result;
        $scope.Init = function ()
        {
            $timeout(function () {
                window.print();
                location.reload();
                $location.url("/Deteksi");
            });
          
        }
    })



;