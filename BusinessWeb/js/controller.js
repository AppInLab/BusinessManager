var MainController = angular.module('MainController', []);

//Controleur du GlobalController
MainController.controller('GlobalController', ['$rootScope', '$scope', '$http','$location',
function ($rootScope, $scope, $http, $location) {

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

    $rootScope.user = {};
    $rootScope.showSidebar = true;
    //$rootScope.login = null;

    console.log($rootScope.user);

}]);

//Controleur du contenu Principal
MainController.controller('HomeController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Dashboard";
    $rootScope.PageDescription = "Vue d'ensemble";

    console.log("Home result : ");
    console.log($rootScope.user);

    $http.get("../handlers/Test.ashx")
    .success(function (data) {
        console.log(data);
    })
    .error(function (data) { $("#loading").fadeOut("fast"); });

}]);

//Controleur d'authentification
MainController.controller('LoginController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {
    
    $rootScope.showSidebar = false;
    $scope.connexion = { modeavance: false };

    //console.log("Controleur d'authentification");
    $scope.connecter = function () {

        $rootScope.user = $scope.connexion;
        console.log($rootScope.user);

        window.location = '#/home';
        $rootScope.showSidebar = true;

        //var fd = { login: 'anicet', password: 'anicet' };
        //$("#loading").fadeIn("slow");
        //$http.get("Handlers/ConnexionHandler.ashx")
        //.success(function (data) {
        //    if (data["succes"]) {
        //        $("#loading").fadeOut("fast");
        //        //alert("Welcome !");
        //        if (data["Config"]["IsConfig"])
        //            document.location = "index.html#/home";
        //        else
        //            window.location = "startconfig.html";
        //    } else {
        //        $("#loading").fadeOut("fast");
        //        $("#login-form").hide();

        //        $("#error-login").css("display", "table");
        //        $("#error-login").html('<i class="glyphicon glyphicon-remove-circle"></i> ' + data['message']);
        //    }
        //})
        //.error(function (data) { $("#loading").fadeOut("fast"); });
    }
    //$scope.connecter();
}]);

//AchatRapideController
MainController.controller('AchatRapideController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Achat rapide";
    $rootScope.PageDescription = "Effectuer un achat rapide";

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
