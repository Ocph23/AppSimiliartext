angular.module("App", ['ngRoute', 'ngProgress', 'App.Admin', 'App.User'])
    .config(function ($routeProvider) {
        $routeProvider
        .when('/Index', {
            templateUrl: "IndexView.htm",
            controller: "IndexController"
        })
        .when("/Judul", {
            templateUrl: "JudulView.htm",
            controller: "JudulController"
        })

        .when("/TambahJudul", {
            templateUrl: "TambahJudulView.htm",
            controller: "TambahJudulController"
        })

         .when("/Mahasiswa", {
             templateUrl: "MahasiswaView.htm",
             controller: "MahasiswaController"
         })

        //User

         .when("/userindex", {
             templateUrl: "UserIndexView.htm",
             controller: "UserIndexController"
         })


         .when("/Deteksi", {
             templateUrl: "DeteksiView.htm",
             controller: "DeteksiController"
         })


         .when("/About", {
             templateUrl: "AboutView.htm",
             controller: "AboutController"
            })

            .when("/Help", {
                templateUrl: "HelpView.htm",

            })
            .when("/Dosen", {
                templateUrl: "DosenView.htm",
                controller: "DosenController"
            })
            .when("/ReportAnalisa", {
                templateUrl: "ReportAnalisaView.htm",
                controller: "ReportAnalisaController"
            })

           .when("/", {
               templateUrl: "UserIndexView.htm",
               controller: "UserIndexController"
           })

        
        ;




    })


;