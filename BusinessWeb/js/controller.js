var MainController = angular.module('MainController', []);

//Controleur du GlobalController
MainController.controller('GlobalController', ['$rootScope', '$scope', '$http','$location','$cookies','$cookieStore',
function ($rootScope, $scope, $http, $location, $cookies, $cookieStore) {

    $rootScope.DateToday = new Date();
    $scope.user = {};

    $rootScope.Alert = function (type, icon, message) {
        $scope.alertType = type;
        $scope.alertIcon = icon;
        $scope.alertMessage = message;
        $("#alertbox").fadeIn(1000).delay(3000).fadeOut(1000)
    }

    $rootScope.AlertIntro = function (type, icon, message) {
        $rootScope.ialertType = type;
        $rootScope.ialertIcon = icon;
        $rootScope.ialertMessage = message;
        $("#intro-notification").fadeIn(1000).delay(3000).fadeOut(1000)
    }

    $rootScope.AlertInfo = function (type, icon, message) {
        $rootScope.infoAlertType = type;
        $rootScope.infoAlertIcon = icon;
        $rootScope.infoAlertMessage = message;
        $("#info-notification").fadeIn(1000).delay(100000).fadeOut(1000)
    }

    $rootScope.OpenModal = function (element) {
        $("#" + element).modal('show');
    }
    $rootScope.CloseModal = function (element) {
        $("#" + element).modal('hide');
    }

    $rootScope.OpenDatePicker = function (input) {
        ////console.log(input);
        $("#" + input.target.id).datepicker({
            dateFormat: "yy-mm-dd"
        });
    }

    //$rootScope.ServerURL = "http://10.18.8.58:91/api/";
    $rootScope.ServerURL = "http://svr-eastest-01/p10/service/api/";

    $scope.isActive = function (route) {
        return route === $location.path();
    }

    $rootScope.LISTE_DES_AGENTS = function () {
        $http.get($rootScope.ServerURL + "Agent")
       .success(function (response) {
           $scope.agents = response._responseData;
           ////console.log(response);
       });
    }

    $rootScope.PageName = "";
    $rootScope.PageDescription = "";

    //Start Connexion-------------------------------
    $rootScope.admin = {};
    $rootScope.connexionSuccess = false;
    $rootScope.profil = {};

    $rootScope.CheckConnexionState = function () {
        if ($cookies.connexionSuccess) {
            $rootScope.connexionSuccess = $cookies.connexionSuccess;
            //$rootScope.profil = JSON.parse($cookies.adminProfil);
        }
    }

    $rootScope.Connexion = function (admin) {
        $http.get($rootScope.url + "ConnectHandler.ashx?login=" + admin.Login + "&password=" + admin.Password)
            .success(function (response) {
                console.log(response);
                if (response.ReturnCode == 1) {
                    $("#errorAlert").removeClass("hidden").html(response.Message);
                }
                if (response.ReturnCode == 0) {
                    $cookies.connexionSuccess = true;
                    $cookies.adminProfil = JSON.stringify(response.Data);
                    window.location = "index.html";
                }
            })
            .error(function (data) {
                $("#errorAlert").removeClass("hidden").html(data.Message);
                $scope.loginError = true;
            });
    }

    $rootScope.Deconnexion = function () {
        $cookieStore.remove('connexionSuccess');
        $cookieStore.remove('adminProfil');
        window.location = "index.html";
    }


    //ESSAI
    $rootScope.connecter = function () {
        $cookies.connexionSuccess = true;
        //$cookies.adminProfil = JSON.stringify(response.Data);
        window.location = "index.html";
    }

    $rootScope.CheckConnexionState();
    //End Connexion ------------------------------

}]);

//Controleur du contenu Principal
MainController.controller('HomeController', ['$rootScope', '$scope', '$http','$cookies','$cookieStore',
function ($rootScope, $scope, $http, $cookies, $cookieStore) {

    //$rootScope.showSidebar = false;

    $rootScope.connexion = {};

    $rootScope.PageName = "Dashboard";
    $rootScope.PageDescription = "Vue d'ensemble";

    console.log("Home result : ");
    console.log($rootScope.user);

    //$http.get("../handlers/Test.ashx")
    //.success(function (data) {
    //    console.log(data);
    //})
    //.error(function (data) { $("#loading").fadeOut("fast"); });

}]);

//AchatRapideController
MainController.controller('VenteController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Vente au comptoir";
    $rootScope.PageDescription = "Effectuer une vente au comptoir";

}]);

//ProduitsController
MainController.controller('ProduitsController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Produits";
    $rootScope.PageDescription = "";

}]);

//ClientsController
MainController.controller('ClientsController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Clients";
    $rootScope.PageDescription = "Gestion des clients";

}]);

//ChineursController
MainController.controller('ChineursController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Chineurs";
    $rootScope.PageDescription = "Gestion des chineurs";

}]);

//FournisseursController
MainController.controller('FournisseursController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Fournisseurs";
    $rootScope.PageDescription = "Gestion des fournisseurs";

}]);

//BonLivraisonController
MainController.controller('BonLivraisonController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Bon de livraison";
    $rootScope.PageDescription = "";

}]);

//PointJourController
MainController.controller('PointJourController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Point du jour";
    $rootScope.PageDescription = "";

}]);

//DepensesController
MainController.controller('DepensesController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Dépenses";
    $rootScope.PageDescription = "";

}]);


//AdminMagasinsController
MainController.controller('AdminMagasinsController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Magasins";
    $rootScope.PageDescription = "Administration des magasins";


}]);

//AdminCategoriesController
MainController.controller('AdminCategoriesController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Catégories";
    $rootScope.PageDescription = "Administration des catégories";


}]);

//AdminProduitsController
MainController.controller('AdminProduitsController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Produits";
    $rootScope.PageDescription = "Administration des produits";


}]);

//AdminFournisseursController
MainController.controller('AdminFournisseursController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Fournisseurs";
    $rootScope.PageDescription = "Administration des fournisseurs";

}]);

//AdminChineursController
MainController.controller('AdminChineursController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Chineurs";
    $rootScope.PageDescription = "Administration des Chineurs";

}]);

//AdminUtilisateursController
MainController.controller('AdminUtilisateursController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Utilisateurs";
    $rootScope.PageDescription = "Administration des Utilisateurs";

}]);
