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
    $rootScope.ServerURL = "http://localhost:26686/api/";

    $scope.isActive = function (route) {
        return route === $location.path();
    }

    //DEPOTS
    $rootScope.LISTE_DEPOTS_DATA = function () {
        $http.get($rootScope.ServerURL + "Depots")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Depots = response.Data;
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) { console.log(response); });
    }

    //CATEGORIES
    $rootScope.LISTE_CATEGORIES_DATA = function () {
        $http.get($rootScope.ServerURL + "Categories")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Categories = response.Data;
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) { console.log(response); });
    }

    //UNITES
    $rootScope.LISTE_UNITES_DATA = function () {
        $http.get($rootScope.ServerURL + "Unites")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Unites = response.Data;
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) { console.log(response); });
    }

    //BLOCKS
    $rootScope.LISTE_BLOCKS_DATA = function () {
        $http.get($rootScope.ServerURL + "Blocks")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Blocks = response.Data;
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) { console.log(response); });
    }

    //PRODUITS
    $rootScope.LISTE_PRODUITS_DATA = function () {
        $http.get($rootScope.ServerURL + "Produits")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Produits = response.Data;
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) { console.log(response); });
    }

    //-END LISTE

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

    $rootScope.connexion = {};

    $rootScope.PageName = "Dashboard";
    $rootScope.PageDescription = "Vue d'ensemble";

    console.log("Home result : ");
    console.log($rootScope.user);

}]);

//DepotController
MainController.controller('DepotController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Depot";
    //$rootScope.PageDescription = "Liste des depots";

    $scope.Init = function () {
        $scope.Depot = {};
    }

    $scope.SendData = function () {
        $http.post($rootScope.ServerURL + "Depots", $scope.Depot)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_DEPOTS_DATA();
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) {
            console.log(response);
        });
    }

    $scope.EditerDepot = function (depot) {
        $scope.Depot = depot;
    }

    //Charger les données
    $rootScope.LISTE_DEPOTS_DATA();

}]);

//DepotController
MainController.controller('CategorieController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Catégorie";

    $scope.Init = function () {
        $scope.Categorie = {};
    }

    $scope.SendData = function () {
        $http.post($rootScope.ServerURL + "Categories", $scope.Categorie)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_CATEGORIES_DATA();
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) {
            console.log(response);
        });
    }

    $scope.Modifier = function (data) {
        $scope.Categorie = data;
    }

    //Charger les données
    $rootScope.LISTE_CATEGORIES_DATA();

}]);

//ProduitsController
MainController.controller('ProduitsController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Produits";
    $scope.Uri = "Produits";

    $scope.Init = function () {
        $scope.Produit = {};
        $scope.Produit.Tva = 0;
        $scope.Produit.PrixAchat = 0;
        $scope.Produit.Ttc = 0;
        $scope.Produit.UniteParBlock = 1;
    }

    $scope.CalculerTTC = function () {
        $scope.Produit.Ttc = Number((($scope.Produit.Tva / 100) * $scope.Produit.PrixAchat)) + Number($scope.Produit.PrixAchat);
    }

    $scope.SendData = function () {
        $http.post($rootScope.ServerURL + "Produits", $scope.Produit)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_PRODUITS_DATA();
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) {
            console.log(response);
        });
    }

    $scope.Modifier = function (data) {
        $scope.Produit = data;
    }

    //Charger les données
    $rootScope.LISTE_PRODUITS_DATA();
    $rootScope.LISTE_CATEGORIES_DATA();
    $rootScope.LISTE_UNITES_DATA();
    $rootScope.LISTE_BLOCKS_DATA();

}]);

//AchatRapideController
MainController.controller('VenteController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Vente au comptoir";
    $rootScope.PageDescription = "Effectuer une vente au comptoir";


    //Recuperer les categories
    $rootScope.LISTE_CATEGORIES_DATA();
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
