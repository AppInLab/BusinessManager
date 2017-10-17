var MainController = angular.module('MainController', ['ngPrint']);

MainController.directive('ngRightClick', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngRightClick);
        element.bind('contextmenu', function (event) {
            scope.$apply(function () {
                event.preventDefault();
                fn(scope, { $event: event });
            });
        });
    };
});

//Controleur du GlobalController
MainController.controller('GlobalController', ['$rootScope', '$scope', '$http','$location','$cookies','$cookieStore','$routeParams','$locale',
function ($rootScope, $scope, $http, $location, $cookies, $cookieStore, $routeParams,$locale) {

    $rootScope.DateToday = new Date();
    $scope.user = {};
    $rootScope.TYPE_VENTE_UNITE_KEY = 1;
    $rootScope.TYPE_VENTE_BLOCK_KEY = 2;

    $locale.NUMBER_FORMATS.GROUP_SEP = ' ';

    $rootScope.TypeNetworkError = "NetworkError";
    $rootScope.NetworkError = "Une erreur s'est produite pendant le traitement. Merci de vérifier votre connexion internet puis réessayer. Si le problème persiste, veuillez contacter votre administrateur";

    $rootScope.HandleClick = function () {
        //console.log("Test");
    }

    $rootScope.DateFormat = function (_datetime, _delimiter) {

        var dateArray = _datetime.split("T");
        var _date = dateArray[0];

        var dateItems = _date.split(_delimiter);
        var yearIndex = 0;
        var monthIndex = 1;
        var dayIndex = 2;
        //var month = parseInt(dateItems[monthIndex]);
        //month -= 1;
        //var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
        return dateItems[dayIndex] + "/" + dateItems[monthIndex] + "/" + dateItems[yearIndex];
    }

    $rootScope.Alert = function (message/*, type*/) {
        //$scope.alertType = type;
        //if (type === $rootScope.TypeNetworkError)
        //    message = $rootScope.NetworkError;
        //$scope.alertIcon = icon;
        $scope.AlertMessage = message;
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

    $rootScope.OpenDateTimePicker = function (input) {
        jQuery.datetimepicker.setLocale('fr');
        jQuery("#" + input.target.id).datetimepicker({
            format: "d/m/Y",
            minDate: '2016/01/01',
            timepicker: false
        });
    }

    //$rootScope.BaseUrl = "http://192.168.1.12"
    //$rootScope.ServerURL = $rootScope.BaseUrl +"/Poissonnerie/ws/api/";//PROD
    //$rootScope.LienAbsPoisson = $rootScope.BaseUrl + "/Poissonnerie/assets/myimg/poisson.png";//PROD
    //$rootScope.LienCssFacture = $rootScope.BaseUrl + "/Poissonnerie/assets/";//PROD
    $rootScope.ServerURL = "http://localhost:26686/api/";
    $rootScope.LienAbsPoisson = "../assets/myimg/poisson.png";
    $rootScope.LienPoisson = $rootScope.LienAbsPoisson;
    $rootScope.LienCssFacture = "../assets/";

    $scope.isActive = function (route) {
        var path = $location.path();
        if (path.length > route.length) {
            path = path.substring(0, route.length);
        }

        return route === path;
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
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
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
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
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
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
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
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
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
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //CLIENTS
    $rootScope.LISTE_CLIENTS_DATA = function () {
        $http.get($rootScope.ServerURL + "Clients")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Clients = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //FOURNISSEURS
    $rootScope.LISTE_FOURNISSEURS_DATA = function () {
        $http.get($rootScope.ServerURL + "Fournisseurs")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Fournisseurs = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //FOURNISSEURS INTERNE
    $rootScope.LISTE_FOURNISSEURS_INTERNE_DATA = function () {
        $http.get($rootScope.ServerURL + "Fournisseurs?interne=true")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Fournisseurs = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //COMMANDES FOURNISSEUR
    $rootScope.LISTE_COMMANDES_FOURNISSEUR_DATA = function () {
        $http.get($rootScope.ServerURL + "CommandeFournisseur")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.CommandesFournisseur = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //BON DE RECEPTION FOURNISSEUR
    $rootScope.LISTE_BONRECEPTION_FOURNISSEUR_DATA = function () {
        $http.get($rootScope.ServerURL + "BonReceptionFournisseur")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.BonsReceptionFournisseur = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //BON DE TRANSFERT
    $rootScope.LISTE_TRANSFERT_DATA = function () {
        $http.get($rootScope.ServerURL + "Transfert")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Transferts = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //COMPTES
    $rootScope.LISTE_COMPTES_DATA = function () {
        $http.get($rootScope.ServerURL + "Comptes")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Comptes = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //PRIVILEGES
    $rootScope.LISTE_PRIVILEGES_DATA = function () {
        $http.get($rootScope.ServerURL + "Privileges")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Privileges = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //CAISSE
    $rootScope.LISTE_CAISSES_DATA = function () {
        $http.get($rootScope.ServerURL + "Caisses")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Caisses = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //SORTIES DE CAISSE
    $rootScope.LISTE_SORTIESDECAISSES_DATA = function () {
        $http.get($rootScope.ServerURL + "SortieDeCaisses?user="+$rootScope.Profil.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.SortiesDeCaisses = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //SESSION DE CAISSE
    $rootScope.LISTE_SESSIONCAISSES_DATA = function (idCaisse) {
        $http.get($rootScope.ServerURL + "SessionCaisses?caisse=" + idCaisse)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.SessionCaisses = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //LISTE SESSION DE CAISSE USER OUVERTE
    $rootScope.SESSIONCAISSE_USER_DATA = function () {
        $http.get($rootScope.ServerURL + "SessionCaisses?user=" + $rootScope.Profil.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.SessionCaisseUser = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //JOURNAL SESSION DE CAISSE
    $rootScope.JOURNAL_SESSIONCAISSE_DATA = function () {
        $http.get($rootScope.ServerURL + "Journal?user=" + $rootScope.Profil.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) { 
                $scope.ListJournal = response.Data;
            } else {//Error
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //--END LISTE

    //Start Connexion-------------------------------
    $rootScope.ConnexionSuccess = false;
    $rootScope.Profil = {};
    $rootScope.FactureData = {};

    $rootScope.CheckConnexionState = function () {
        if ($cookies.ConnexionSuccess && !$rootScope.ConnexionSuccess) {
            $rootScope.ConnexionSuccess = $cookies.ConnexionSuccess;
            $rootScope.Profil = JSON.parse($cookies.Profil);
            $rootScope.FactureData = JSON.parse($cookies.FactureData);
            $rootScope.CheckHauth();
        }
    }

    $rootScope.CheckHauth = function () {
        $http.get($rootScope.ServerURL + "Comptes?checkhauth="+$rootScope.Profil.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.ConnexionSuccess = $cookies.ConnexionSuccess;
                $cookies.Profil = JSON.stringify(response.Data);
                $cookies.FactureData = JSON.stringify(response.Sender);

                $rootScope.Profil = JSON.parse($cookies.Profil);
                $rootScope.FactureData = JSON.parse($cookies.FactureData);
            } else if (response.ResponseCode == -2) {
                $rootScope.Deconnexion();
            }
            else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $rootScope.Connexion = function () {
        $http.post($rootScope.ServerURL + "Connexion", $rootScope.Profil)
            .success(function (response) {
                console.log(response);
                if (response.ResponseCode == 0) {
                    $cookies.ConnexionSuccess = true;
                    $cookies.Profil = JSON.stringify(response.Data);
                    $cookies.FactureData = JSON.stringify(response.Sender);
                    window.location = "index.html";
                } else {
                    $rootScope.Alert(response.Message);
                }
            })
            .error(function (response) {
                $rootScope.Alert(response.Message);
            });
    }

    $rootScope.Deconnexion = function () {
        $cookieStore.remove('ConnexionSuccess');
        $cookieStore.remove('Profil');
        $cookieStore.remove('InfosFacture');
        window.location = "index.html";
    }

    $scope.AuthorizeUser = ['/Home', '/VenteComptoir', '/SessionCaisses', '/SortiesDeCaisses', '/Clients', '/FicheClients/:client', '/Transferts', '/NouveauTransfert'];
    $scope.AuthorizeStockUser = ['/Home', '/Caisses', '/InfosSessionCaisses/:caisse', '/InventaireDeStocks/:caisse', '/Clients', '/FicheClients/:client'];

    $scope.AuthorizeAdmin = ['/Home', '/VenteComptoir', '/Categories', '/Produits',
                      '/Fournisseurs', '/CommandeFournisseur', '/BonReceptionFournisseur', '/Clients', '/FicheClients/:client',
                      '/Comptes', '/Caisses', '/InfosSessionCaisses/:caisse', '/InventaireDeStocks/:caisse',
                      '/NouveauBonReceptionFournisseur', '/NouvelleCommandeFournisseur', '/EditerBonReceptionFournisseur/:id', '/EditerCommandeFournisseur/:id', '/Transferts', '/NouveauTransfert'];
    $scope.Authorize = [];
    $rootScope.$on('$routeChangeStart', function (next, current) {
        if ($rootScope.Profil.IsUser)
            $scope.Authorize = $scope.AuthorizeUser;
        else if ($rootScope.Profil.IsStockUser)
            $scope.Authorize = $scope.AuthorizeStockUser;
        else if ($rootScope.Profil.IsAdmin)
            $scope.Authorize = $scope.AuthorizeAdmin;

        if (!$rootScope.Profil.IsSu) {
            if ($scope.Authorize.indexOf(current.$$route.originalPath) === -1) {
                window.location = "#/Home";
            }
        }
        //console.log(next);
    });

    $rootScope.CheckConnexionState();
    //End Connexion ------------------------------

}]);

//Controleur du contenu Principal
MainController.controller('HomeController', ['$rootScope', '$scope', '$http','$cookies','$cookieStore',
function ($rootScope, $scope, $http, $cookies, $cookieStore) {

    $rootScope.connexion = {};

    $rootScope.PageName = "Dashboard";
    $rootScope.PageDescription = "Vue d'ensemble";

    //console.log("Home result : ");
    //console.log($rootScope.user);

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
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
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
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
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

    $scope.CopyOfProduits = [];//Sauvegarde
    $scope.Init = function () {
        $scope.Produit = {};
        $scope.Produit.Tva = 0;
        $scope.Produit.PrixAchat = 0;
        $scope.Produit.Ttc = 0;
        $scope.Produit.UniteParBlock = 1;
        $scope.ShowBlock = null;

        //Faire une sauvegarde les produits existants
        angular.copy($scope.Produits, $scope.CopyOfProduits);
    }

    $scope.CanShowBlock = function (unite) {
        for (var i = 0; i < $scope.Unites.length; i++) {
            if ($scope.Unites[i].Id != unite) continue;
            $scope.ShowBlock = $scope.Unites[i].IsBlock;
            $scope.Produit.Block = null;
            $scope.Produit.UniteParBlock = 1;
            break;
        }
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
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    
    $scope.Modifier = function (data) {
        $scope.Produit = data;
        $scope.IsModification = true;
        angular.copy($scope.Produits, $scope.CopyOfProduits);

        for (var i = 0; i < $scope.Unites.length; i++) {
            if ($scope.Unites[i].Id != $scope.Produit.Unite.Id) continue;
            $scope.ShowBlock = $scope.Unites[i].IsBlock;
        }
    }

    $scope.Annuler = function () {
        $scope.IsModification = false;
        $scope.Produits = angular.copy($scope.CopyOfProduits);
    }
	
	$scope.ResetSearch = function () {
        $scope.searchText = "";
    }

    //Charger les données
    $rootScope.LISTE_PRODUITS_DATA();
    $rootScope.LISTE_CATEGORIES_DATA();
    $rootScope.LISTE_UNITES_DATA();
    $rootScope.LISTE_BLOCKS_DATA();

}]);

//AchatRapideController
MainController.controller('VenteController', ['$rootScope', '$scope', '$http','$filter',
function ($rootScope, $scope, $http, $filter) {

    $rootScope.PageName = "Vente au comptoir";

    //Index de chargement des produits en fonction de la categorie
    $scope.SelectedItem = null;
    $scope.Panier = [];
    $scope.TotauxPanier = 0;
    $scope.ClientSelected = null;

    $scope.SelectionnerClient = function (client) {
        $scope.ClientSelected = client;
    }

    $scope.Init = function (produit) {
        $scope.IsNew = true;
        $scope.ProduitAajouter = {};
        $scope.ProduitAajouter.Produit = produit;
        $scope.ProduitAajouter.PrixUnitaire = produit.PrixVenteUniteParDefaut;
        $scope.ProduitAajouter.PrixBlock = produit.PrixVenteBlockParDefaut;
        $scope.ProduitAajouter.QuantiteUnitaire = 0;
        $scope.ProduitAajouter.QuantiteBlock = 0;
        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * produit.PrixVenteBlockParDefaut;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * produit.PrixVenteUniteParDefaut;
        $scope.ProduitAajouter.Totaux = Number($scope.ProduitAajouter.TotalBlock) + Number($scope.ProduitAajouter.TotalUnitaire);

        $scope.TypesDeVente = [];
        if (produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: produit.Block.Libelle });
            if(produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            else
                $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        }
    }

    $scope.CaisseInit = function () {
        $scope.Caisse = {};
        $scope.Caisse.TotauxPanier = $scope.TotauxPanier;
        $scope.Caisse.Remise = 0;
        $scope.Caisse.NetApayer = $scope.TotauxPanier - $scope.Caisse.Remise;
        $scope.Caisse.MontantPercu = 0;
        $scope.Caisse.MontantRendu = 0;
        $scope.ClientSelected = null;
    }

    $scope.CalculerCaisse = function () {
        $scope.Caisse.NetApayer = $scope.TotauxPanier - $scope.Caisse.Remise;
        $scope.Caisse.MontantRendu = $scope.Caisse.MontantPercu - $scope.Caisse.NetApayer;
    }

    $scope.CopierNetApayer = function () {
        $scope.Caisse.MontantPercu = angular.copy($scope.Caisse.NetApayer);
        $scope.CalculerCaisse();
    }

    $scope.Calculer = function () {
        if (isNaN($scope.ProduitAajouter.QuantiteBlock))
            $scope.ProduitAajouter.QuantiteBlock = 0;

        if (isNaN($scope.ProduitAajouter.QuantiteUnitaire))
            $scope.ProduitAajouter.QuantiteUnitaire = 0;

        $scope.ProduitAajouter.TotalBlock = Math.round($scope.ProduitAajouter.QuantiteBlock * $scope.ProduitAajouter.PrixBlock);
        $scope.ProduitAajouter.TotalUnitaire = Math.round($scope.ProduitAajouter.QuantiteUnitaire * $scope.ProduitAajouter.PrixUnitaire);

        $scope.ProduitAajouter.Totaux = 0;
        if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
            $scope.ProduitAajouter.Totaux = $scope.ProduitAajouter.TotalUnitaire;
        else if($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
            $scope.ProduitAajouter.Totaux = $scope.ProduitAajouter.TotalBlock;
    }

    $scope.AjouterAuPanier = function (produit) {
        if($scope.IsNew)
            $scope.Panier.push(produit);
        else
            $scope.Panier = angular.copy($scope.Panier);

        $scope.CalclerTotalPanier();
    }

    $scope.CalclerTotalPanier = function () {
        //Calculer le montant total du panier
        $scope.TotauxPanier = 0;
        for (i = 0; i < $scope.Panier.length; i++) {
            item = $scope.Panier[i];
            if (item.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
                $scope.TotauxPanier = Number($scope.TotauxPanier) + Number(item.TotalUnitaire);
            else if (item.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
                $scope.TotauxPanier = Number($scope.TotauxPanier) + Number(item.TotalBlock);
        }
    }

    $scope.AncienPanier = [];
    $scope.Modifier = function (data) {
        $scope.ProduitAajouter = data;
        angular.copy($scope.Panier, $scope.AncienPanier);
        $scope.IsNew = false;
    }

    $scope.SupprimerItemPanier = function (index) {
        $scope.Panier.splice(index, 1);
        $scope.CalclerTotalPanier();
    }

    $scope.Annuler = function () {
        if (!$scope.IsNew)
            $scope.Panier = angular.copy($scope.AncienPanier);
        $scope.ClientSelected = null;
        $scope.InitNewClient();
    }

    $scope.ChargerProduits = function (categorie) {
        $scope.SelectedItem = categorie.Id;
        $http.get($rootScope.ServerURL + "Produits?categorieAvecQuantite=" + categorie.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.ProduitsParCategorie = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.ResetComptoir = function () {
        $scope.SelectedItem = null;
        $scope.Panier = [];
        $scope.TotauxPanier = 0;
        $scope.ProduitsParCategorie = [];
        $scope.ClientSelected = null;
        //$scope.Facture = {};
    }

    $scope.InfosFacture = {};
    $scope.SendData = function (mode, print) {		
        $scope.Facture = {};
        $scope.Facture.Client = $scope.ClientSelected;
        $scope.Facture.User = $rootScope.Profil;
        $scope.Facture.Caisse = $scope.Caisse;
        $scope.Facture.Panier = $scope.Panier;
        $scope.Facture.MontantPercu = $scope.Caisse.MontantPercu;
        //$scope.Facture.Remise = $scope.Caisse.Remise;
        
        $http.post($rootScope.ServerURL + "FacturesClient", $scope.Facture)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                //$rootScope.CloseModal('cash');
                $scope.InfosFacture = response.Data;
                //Impression
                if (print) {
                    if(mode === 'cash')
                        $scope.PrintDoPreview("factureCash");
                    else
                        $scope.PrintDoPreview("factureClient");
                }
                //Remise à zero
                $scope.ResetComptoir();//Remettre le comptoir à Zero pour une nouvelle vente
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.InitNewClient = function () {
        $scope.Client = {};
    }

    $scope.SendNewClientData = function () {
        $http.post($rootScope.ServerURL + "Clients", $scope.Client)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_CLIENTS_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.PrintDoPreview = function (factureId) {
        var toPrint = document.getElementById(factureId);
        var winPop = window.open('', '_blank', 'fullscreen=1,left=0,top=0');
        winPop.document.write('<html><head><title>::Preview::</title><link href="'+$rootScope.LienCssFacture+'css/style-print.css" rel="stylesheet" /></head><body>');
        var outout = toPrint.outerHTML;

        var dateFacture = $filter('date')($scope.InfosFacture.DateCreation, 'EEE dd MMM yyyy HH:mm');

        outout = outout.replace("[:numFacture]", $scope.InfosFacture.Id)
            .replace("[:dateFacture]", dateFacture)
            .replace("[:resteApayer]", $scope.InfosFacture.SommeDue);
        winPop.document.write(outout);
        winPop.document.write('</body></html>');
        winPop.document.close();

        print = function () {
            winPop.print();
            winPop.close();
        }

        setTimeout(print, 50);
    }

    //Recuperer les categories
    $rootScope.LISTE_CATEGORIES_DATA();
    $rootScope.LISTE_CLIENTS_DATA();
}]);

//ClientsController
MainController.controller('ClientController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Clients";
    
    $scope.Init = function () {
        $scope.Client = {};
        angular.copy($scope.Clients, $scope.AncienClients);
    }   

    $scope.SendData = function () {
        $http.post($rootScope.ServerURL + "Clients", $scope.Client)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_CLIENTS_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.AncienClients = [];
    $scope.Modifier = function (data) {
        $scope.Client = data;
        $scope.Modif = true;
        angular.copy($scope.Clients, $scope.AncienClients);
    }

    $scope.Annuler = function () {
        $scope.Clients = angular.copy($scope.AncienClients);
    }

    $scope.ResetSearch = function () {
        $scope.searchText = "";
    }

    $rootScope.LISTE_CLIENTS_DATA();
}]);

//ClientsController
MainController.controller('FicheClientController', ['$rootScope', '$scope', '$http', '$routeParams',
function ($rootScope, $scope, $http, $routeParams) {

    $rootScope.PageName = "Fiche client";
    $scope.idClient = $routeParams.client;
    $scope.Data = {};
    //$scope.dateOperation = "";
    $scope.dateAnterieur = "";
    $scope.soldeAnterieurClient = 0;

    if (isNaN($scope.idClient)) window.location = "#/Home";

    $scope.soldeAnterieur = function (dateAnterieur) {
        $http.get($rootScope.ServerURL + "Clients?client=" + $scope.idClient + "&dateAnterieur=" + dateAnterieur)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.soldeAnterieurClient = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.InitPaiement = function () {
        $scope.Paiement = {};
        $scope.Paiement.Client = $scope.FicheClient.Client;
        $scope.Paiement.User = $rootScope.Profil;
    }

    $scope.FICHE_CLIENT_DATA = function () {
        $http.get($rootScope.ServerURL + "Clients?client=" + $scope.idClient)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.FicheClient = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.FICHE_ANTERIEURE_CLIENT_DATA = function () {
        $http.get($rootScope.ServerURL + "Clients?client=" + $scope.idClient + "&anterieure=true")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.FicheClientAnterieure = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.DETAILS_FACTURE_CLIENT_DATA = function (facture) {
        $scope.DetailsFacture = {};
        $scope.FactureSelect = facture;

        $http.get($rootScope.ServerURL + "FacturesClient?details=" + facture.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.DetailsFacture = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.DETAILS_RETOUR_FACTURE_CLIENT_DATA = function (facture) {
        $scope.DetailsFacture = {};
        $scope.FactureSelect = facture;

        $http.get($rootScope.ServerURL + "FacturesClient?detailsRetourFacture=" + facture.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.DetailsRetourFacture = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.SendData = function () {
        $http.post($rootScope.ServerURL + "Paiements", $scope.Paiement)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.FICHE_CLIENT_DATA();

                //Imprimer le reçu du versement
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //RETOUR PRODUITS 
    $scope.GetRetourFactureProduits = function (facture) {
        $http.get($rootScope.ServerURL + "RetourFacturesClient?facture="+facture.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.DetailsRetourFacturesClient = response.Data;
                $scope.Data.DetailsRetourFacturesClient = $scope.DetailsRetourFacturesClient;
                $scope.Data.UserId = $scope.Profil.Id;
                $scope.Data.FactureClientId = facture.Id;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.RetourFactureClientSend = function () {
        $http.post($rootScope.ServerURL + "RetourFacturesClient", $scope.Data)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Data = {};
                $scope.FICHE_CLIENT_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.Calculer = function () {
        $scope.Paiement.Monnaie = Number($scope.Paiement.MontantPercu) - Number($scope.Paiement.Versement);
    }

    $scope.FICHE_CLIENT_DATA();
}]);

//FournisseurController
MainController.controller('FournisseurController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Fournisseurs";

    $scope.Init = function () {
        $scope.Fournisseur = {};
    }

    $scope.SendData = function () {
        $http.post($rootScope.ServerURL + "Fournisseurs", $scope.Fournisseur)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_FOURNISSEURS_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.AncienFournisseurs = [];
    $scope.Modifier = function (data) {
        $scope.Fournisseur = data;
        angular.copy($scope.Fournisseurs, $scope.AncienFournisseurs);
    }

    $scope.Annuler = function () {
        $scope.Fournisseurs = angular.copy($scope.AncienFournisseurs);
    }

    $rootScope.LISTE_FOURNISSEURS_DATA();

}]);

//CommandeFournisseurController
MainController.controller('CommandeFournisseurController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Commande fournisseur";

    $scope.Init = function () {
        $scope.CommandeFournisseur = {};
    }

    $scope.SendData = function () {
        $http.post($rootScope.ServerURL + "CommandeFournisseur", $scope.CommandeFournisseur)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_FOURNISSEURS_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.SetCommandeAtransferer = function (data) {
        $scope.CommandeAtransferer = data;
    }

    $scope.TransfererVersBonReception = function () {        
        $http.get($rootScope.ServerURL + "CommandeFournisseur?transfertBonReception=" + $scope.CommandeAtransferer.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_COMMANDES_FOURNISSEUR_DATA();
            } else {//Error
                console.log(response);
            }
        })
        .error(function (response) {
            console.log(response);
        });
    }

    $scope.InfosBonReception = function (data) {
        $scope.Commande = {};
        $http.get($rootScope.ServerURL + "CommandeFournisseur?id=" + data.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Commande = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.LISTE_COMMANDES_FOURNISSEUR_DATA();
}]);

//NouvelleCommandeFournisseurController
MainController.controller('NouvelleCommandeFournisseurController', ['$rootScope', '$scope', '$http', '$window','$routeParams','$filter',
function ($rootScope, $scope, $http, $window, $routeParams, $filter) {

    $rootScope.PageName = "Nouvelle commande fournisseur";
    
    //Index de chargement des produits en fonction de la categorie
    $scope.SelectedItem = null;
    $scope.Panier = [];
    $scope.Fournisseur = {};
    $scope.TotauxPanierHt = 0;
    $scope.TotauxPanierTva = 0;
    $scope.TotauxPanierTtc = 0;
    $scope.DateCreation = jQuery("#dateCommande").val();

    $scope.Init = function (produit) {
        $scope.IsNew = true;
        $scope.ProduitAajouter = {};
        $scope.ProduitAajouter.Produit = produit;
        $scope.ProduitAajouter.PrixAchat = produit.PrixAchat;
        $scope.ProduitAajouter.QuantiteUnitaire = 0;
        $scope.ProduitAajouter.QuantiteBlock = 0;
        $scope.ProduitAajouter.Tva = 0;
        $scope.ProduitAajouter.Ttc = 0;
        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * produit.PrixAchat;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * produit.PrixAchat;
        $scope.ProduitAajouter.Tht = Number($scope.ProduitAajouter.TotalBlock) + Number($scope.ProduitAajouter.TotalUnitaire);

        $scope.TypesDeVente = [];
        if (produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: produit.Block.Libelle });
            if (produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            else
                $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        }
    }

    $scope.Calculer = function () {
        if (isNaN($scope.ProduitAajouter.QuantiteBlock))
            $scope.ProduitAajouter.QuantiteBlock = 0;

        if (isNaN($scope.ProduitAajouter.QuantiteUnitaire))
            $scope.ProduitAajouter.QuantiteUnitaire = 0;

        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * $scope.ProduitAajouter.PrixAchat;//PrixBlock;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * $scope.ProduitAajouter.PrixAchat;//PrixUnitaire;

        $scope.ProduitAajouter.Tht = 0;
        if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
            $scope.ProduitAajouter.Tht = $scope.ProduitAajouter.TotalUnitaire;
        else if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
            $scope.ProduitAajouter.Tht = $scope.ProduitAajouter.TotalBlock;

        $scope.ProduitAajouter.MontantTva = (($scope.ProduitAajouter.Tva / 100) * $scope.ProduitAajouter.Tht);
        $scope.ProduitAajouter.Ttc = Number($scope.ProduitAajouter.MontantTva) + Number($scope.ProduitAajouter.Tht);
    }

    $scope.AjouterAuPanier = function (produit) {
        if ($scope.IsNew)
            $scope.Panier.push(produit);
        else
            $scope.Panier = angular.copy($scope.Panier);

        $scope.CalculerTotalPanier();
    }

    $scope.CalculerTotalPanier = function () {
        //Calculer le montant total du panier
        $scope.TotauxPanierHt = 0;
        $scope.TotauxPanierTva = 0;
        $scope.TotauxPanierTtc = 0;

        for (i = 0; i < $scope.Panier.length; i++) {
            item = $scope.Panier[i];
            if (item.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
                $scope.TotauxPanierHt = Number($scope.TotauxPanierHt) + Number(item.TotalUnitaire);
            else if (item.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
                $scope.TotauxPanierHt = Number($scope.TotauxPanierHt) + Number(item.TotalBlock);

            $scope.TotauxPanierTva = Number($scope.TotauxPanierTva) + Number(item.MontantTva);
            $scope.TotauxPanierTtc = Number($scope.TotauxPanierTtc) + Number(item.Ttc);
        }
    }

    $scope.AncienPanier = [];
    $scope.Modifier = function (data) {
        $scope.ProduitAajouter = data;
        angular.copy($scope.Panier, $scope.AncienPanier);
        $scope.IsNew = false;

        //Mise a jour des type de vente
        $scope.TypesDeVente = [];
        if (data.Produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: data.Produit.Block.Libelle });
            if (data.Produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: data.Produit.Unite.Libelle });
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: data.Produit.Unite.Libelle });
        }
    }

    $scope.SupprimerItemPanier = function (index) {
        $scope.Panier.splice(index, 1);
        $scope.CalculerTotalPanier();
    }

    $scope.Annuler = function () {
        if (!$scope.IsNew)
            $scope.Panier = angular.copy($scope.AncienPanier);
    }

    $scope.ChargerProduits = function (categorie) {
        $scope.SelectedItem = categorie.Id;
        $http.get($rootScope.ServerURL + "Produits?categorie=" + categorie.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.ProduitsParCategorie = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.ResetComptoir = function () {
        $scope.SelectedItem = null;
        $scope.Commentaire = null;
        $scope.Panier = [];
        $scope.TotauxPanierHt = 0;
        $scope.TotauxPanierTva = 0;
        $scope.TotauxPanierTtc = 0;
        $scope.ProduitsParCategorie = [];
        $scope.Vente = {};
        //$scope.DateCreation = $filter('date')(date[$scope.DateToday, "dd/MM/yyyy"]);
        $scope.Fournisseur = {};
    }

    $scope.SendData = function (bonDeReception) {
        $scope.Commande = {};
        $scope.Commande.Panier = $scope.Panier;
        $scope.Commande.Fournisseur = $scope.Fournisseur;
        $scope.Commande.Commentaire = $scope.Commentaire;
        $scope.Commande.DateCreation = $scope.DateCreation;
        $scope.Commande.TransfertVersBonDeReception = bonDeReception;

        $http.post($rootScope.ServerURL + "CommandeFournisseur", $scope.Commande)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.CloseModal('confirmationValidation');
                $window.location.href = "#/CommandeFournisseur";
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //Recuperer les categories
    $rootScope.LISTE_CATEGORIES_DATA();
    $rootScope.LISTE_FOURNISSEURS_DATA();

}]);

//EditerCommandeFournisseurController
MainController.controller('EditerCommandeFournisseurController', ['$rootScope', '$scope', '$http', '$window', '$routeParams',
function ($rootScope, $scope, $http, $window, $routeParams) {

    $rootScope.PageName = "Editer commande fournisseur";
    $scope.commandeId = $routeParams.id;

    //Index de chargement des produits en fonction de la categorie
    $scope.SelectedItem = null;
    $scope.Panier = [];
    $scope.Fournisseur = {};
    $scope.TotauxPanierHt = 0;
    $scope.TotauxPanierTva = 0;
    $scope.TotauxPanierTtc = 0;

    if (!isNaN($scope.commandeId)) {
        $http.get($rootScope.ServerURL + "CommandeFournisseur?id=" + $scope.commandeId)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {

                $scope.Commentaire = response.Data.Commentaire;
                $scope.Panier = response.Data.Panier;
                $scope.Fournisseur = response.Data.Fournisseur;
                $scope.DateCreation = $rootScope.DateFormat(response.Data.DateCreation, "-");

                $scope.Commande = {};
                $scope.Commande.Id = response.Data.Id;
                $scope.Commande.DateCreation = response.Data.DateCreation;
                $scope.Commande.Panier = $scope.Panier;
                $scope.Commande.Fournisseur = $scope.Fournisseur;
                $scope.Commande.Commentaire = $scope.Commentaire;

                $scope.CalculerTotalPanier();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.Init = function (produit) {
        $scope.IsNew = true;
        $scope.ProduitAajouter = {};
        $scope.ProduitAajouter.Produit = produit;
        $scope.ProduitAajouter.PrixAchat = produit.PrixAchat;
        $scope.ProduitAajouter.QuantiteUnitaire = 0;
        $scope.ProduitAajouter.QuantiteBlock = 0;
        $scope.ProduitAajouter.Tva = 0;
        $scope.ProduitAajouter.Ttc = 0;
        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * produit.PrixAchat;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * produit.PrixAchat;
        $scope.ProduitAajouter.Tht = Number($scope.ProduitAajouter.TotalBlock) + Number($scope.ProduitAajouter.TotalUnitaire);

        $scope.TypesDeVente = [];
        if (produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: produit.Block.Libelle });
            if (produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            else
                $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        }
    }

    $scope.Calculer = function () {
        if (isNaN($scope.ProduitAajouter.QuantiteBlock))
            $scope.ProduitAajouter.QuantiteBlock = 0;

        if (isNaN($scope.ProduitAajouter.QuantiteUnitaire))
            $scope.ProduitAajouter.QuantiteUnitaire = 0;

        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * $scope.ProduitAajouter.PrixAchat;//PrixBlock;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * $scope.ProduitAajouter.PrixAchat;//PrixUnitaire;

        $scope.ProduitAajouter.Tht = 0;
        if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
            $scope.ProduitAajouter.Tht = $scope.ProduitAajouter.TotalUnitaire;
        else if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
            $scope.ProduitAajouter.Tht = $scope.ProduitAajouter.TotalBlock;

        $scope.ProduitAajouter.MontantTva = (($scope.ProduitAajouter.Tva / 100) * $scope.ProduitAajouter.Tht);
        $scope.ProduitAajouter.Ttc = Number($scope.ProduitAajouter.MontantTva) + Number($scope.ProduitAajouter.Tht);
    }

    $scope.AjouterAuPanier = function (produit) {
        if ($scope.IsNew)
            $scope.Panier.push(produit);
        else
            $scope.Panier = angular.copy($scope.Panier);

        $scope.CalculerTotalPanier();
    }

    $scope.CalculerTotalPanier = function () {
        //Calculer le montant total du panier
        $scope.TotauxPanierHt = 0;
        $scope.TotauxPanierTva = 0;
        $scope.TotauxPanierTtc = 0;

        for (i = 0; i < $scope.Panier.length; i++) {
            item = $scope.Panier[i];
            if (item.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
                $scope.TotauxPanierHt = Number($scope.TotauxPanierHt) + Number(item.TotalUnitaire);
            else if (item.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
                $scope.TotauxPanierHt = Number($scope.TotauxPanierHt) + Number(item.TotalBlock);

            $scope.TotauxPanierTva = Number($scope.TotauxPanierTva) + Number(item.MontantTva);
            $scope.TotauxPanierTtc = Number($scope.TotauxPanierTtc) + Number(item.Ttc);
        }
    }

    $scope.AncienPanier = [];
    $scope.Modifier = function (data) {
        $scope.ProduitAajouter = data;
        angular.copy($scope.Panier, $scope.AncienPanier);
        $scope.IsNew = false;

        //Mise a jour des type de vente
        $scope.TypesDeVente = [];
        if (data.Produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: data.Produit.Block.Libelle });
            if (data.Produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: data.Produit.Unite.Libelle });
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: data.Produit.Unite.Libelle });
        }
    }

    $scope.SupprimerItemPanier = function (index) {
        $scope.Panier.splice(index, 1);
        $scope.CalculerTotalPanier();
    }

    $scope.Annuler = function () {
        if (!$scope.IsNew)
            $scope.Panier = angular.copy($scope.AncienPanier);
    }

    $scope.ChargerProduits = function (categorie) {
        $scope.SelectedItem = categorie.Id;
        $http.get($rootScope.ServerURL + "Produits?categorie=" + categorie.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.ProduitsParCategorie = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.ResetComptoir = function () {
        $rootScope.CloseModal('confirmationAnnulation');
        $window.location.href = "#/CommandeFournisseur";
    }

    $scope.SendData = function (bonDeReception) {
        $scope.Commande.Panier = $scope.Panier;
        $scope.Commande.Fournisseur = $scope.Fournisseur;
        $scope.Commande.Commentaire = $scope.Commentaire;
        $scope.Commande.DateCreation = $scope.DateCreation;
        $scope.Commande.TransfertVersBonDeReception = bonDeReception;

        $http.post($rootScope.ServerURL + "CommandeFournisseur", $scope.Commande)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.CloseModal('confirmationValidation');
                $window.location.href = "#/CommandeFournisseur";
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //Recuperer les categories
    $rootScope.LISTE_CATEGORIES_DATA();
    $rootScope.LISTE_FOURNISSEURS_DATA();

}]);

//BonReceptionFournisseurController
MainController.controller('BonReceptionFournisseurController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Bon de reception";

    $scope.InfosBonReception = function (data) {
        $scope.Commande = {};
        $http.get($rootScope.ServerURL + "BonReceptionFournisseur?id=" + data.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Commande = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.SetBonReception = function (data) {
        $scope.BonReception = data;
    }

    $scope.MarquerCommeRecu = function () {
        $http.get($rootScope.ServerURL +
        "BonReceptionFournisseur?marquerCommeRecu=" + $scope.BonReception.Id +
        "&date=" + $scope.DateValidation)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_BONRECEPTION_FOURNISSEUR_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $rootScope.LISTE_BONRECEPTION_FOURNISSEUR_DATA();
}]);

//NouvelleCommandeFournisseurController
MainController.controller('NouveauBonReceptionFournisseurController', ['$rootScope', '$scope', '$http', '$window', '$routeParams',
function ($rootScope, $scope, $http, $window, $routeParams) {

    $rootScope.PageName = "Nouveau bon de reception";

    //Index de chargement des produits en fonction de la categorie
    $scope.SelectedItem = null;
    $scope.Panier = [];
    $scope.Fournisseur = {};
    $scope.TotauxPanierHt = 0;
    $scope.TotauxPanierTva = 0;
    $scope.TotauxPanierTtc = 0;

    $scope.Init = function (produit) {
        $scope.IsNew = true;
        $scope.ProduitAajouter = {};
        $scope.ProduitAajouter.Produit = produit;
        $scope.ProduitAajouter.PrixAchat = produit.PrixAchat;
        $scope.ProduitAajouter.QuantiteUnitaire = 0;
        $scope.ProduitAajouter.QuantiteBlock = 0;
        $scope.ProduitAajouter.Tva = 0;
        $scope.ProduitAajouter.Ttc = 0;
        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * produit.PrixAchat;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * produit.PrixAchat;
        $scope.ProduitAajouter.Tht = Number($scope.ProduitAajouter.TotalBlock) + Number($scope.ProduitAajouter.TotalUnitaire);

        $scope.TypesDeVente = [];
        if (produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: produit.Block.Libelle });
            if (produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            else
                $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        }
    }

    $scope.Calculer = function () {
        if (isNaN($scope.ProduitAajouter.QuantiteBlock))
            $scope.ProduitAajouter.QuantiteBlock = 0;

        if (isNaN($scope.ProduitAajouter.QuantiteUnitaire))
            $scope.ProduitAajouter.QuantiteUnitaire = 0;

        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * $scope.ProduitAajouter.PrixAchat;//PrixBlock;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * $scope.ProduitAajouter.PrixAchat;//PrixUnitaire;

        $scope.ProduitAajouter.Tht = 0;
        if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
            $scope.ProduitAajouter.Tht = $scope.ProduitAajouter.TotalUnitaire;
        else if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
            $scope.ProduitAajouter.Tht = $scope.ProduitAajouter.TotalBlock;

        $scope.ProduitAajouter.MontantTva = (($scope.ProduitAajouter.Tva / 100) * $scope.ProduitAajouter.Tht);
        $scope.ProduitAajouter.Ttc = Number($scope.ProduitAajouter.MontantTva) + Number($scope.ProduitAajouter.Tht);
    }

    $scope.AjouterAuPanier = function (produit) {
        if ($scope.IsNew)
            $scope.Panier.push(produit);
        else
            $scope.Panier = angular.copy($scope.Panier);

        $scope.CalculerTotalPanier();
    }

    $scope.CalculerTotalPanier = function () {
        //Calculer le montant total du panier
        $scope.TotauxPanierHt = 0;
        $scope.TotauxPanierTva = 0;
        $scope.TotauxPanierTtc = 0;

        for (i = 0; i < $scope.Panier.length; i++) {
            item = $scope.Panier[i];
            if (item.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
                $scope.TotauxPanierHt = Number($scope.TotauxPanierHt) + Number(item.TotalUnitaire);
            else if (item.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
                $scope.TotauxPanierHt = Number($scope.TotauxPanierHt) + Number(item.TotalBlock);

            $scope.TotauxPanierTva = Number($scope.TotauxPanierTva) + Number(item.MontantTva);
            $scope.TotauxPanierTtc = Number($scope.TotauxPanierTtc) + Number(item.Ttc);
        }
    }

    $scope.AncienPanier = [];
    $scope.Modifier = function (data) {
        $scope.ProduitAajouter = data;
        angular.copy($scope.Panier, $scope.AncienPanier);
        $scope.IsNew = false;

        //Mise a jour des type de vente
        $scope.TypesDeVente = [];
        if (data.Produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: data.Produit.Block.Libelle });
            if (data.Produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: data.Produit.Unite.Libelle });
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: data.Produit.Unite.Libelle });
        }
    }

    $scope.SupprimerItemPanier = function (index) {
        $scope.Panier.splice(index, 1);
        $scope.CalculerTotalPanier();
    }

    $scope.Annuler = function () {
        if (!$scope.IsNew)
            $scope.Panier = angular.copy($scope.AncienPanier);
    }

    $scope.ChargerProduits = function (categorie) {
        $scope.SelectedItem = categorie.Id;
        $http.get($rootScope.ServerURL + "Produits?categorie=" + categorie.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.ProduitsParCategorie = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.ResetComptoir = function () {
        $scope.SelectedItem = null;
        $scope.Commentaire = null;
        $scope.Panier = [];
        $scope.TotauxPanierHt = 0;
        $scope.TotauxPanierTva = 0;
        $scope.TotauxPanierTtc = 0;
        $scope.ProduitsParCategorie = [];
        $scope.Vente = {};
        $scope.Fournisseur = {};
    }

    $scope.SendData = function (bonDeReception) {
        $scope.Commande = {};
        $scope.Commande.Panier = $scope.Panier;
        $scope.Commande.Fournisseur = $scope.Fournisseur;
        $scope.Commande.Commentaire = $scope.Commentaire;
        $scope.Commande.DateCreation = $scope.DateCreation;
        //$scope.Commande.MarquerRecu = bonDeReception;

        $http.post($rootScope.ServerURL + "BonReceptionFournisseur", $scope.Commande)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.CloseModal('confirmationValidation');
                $window.location.href = "#/BonReceptionFournisseur";
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //Recuperer les categories
    $rootScope.LISTE_CATEGORIES_DATA();
    $rootScope.LISTE_FOURNISSEURS_DATA();

}]);

//EditerCommandeFournisseurController
MainController.controller('EditerBonReceptionFournisseurController', ['$rootScope', '$scope', '$http', '$window', '$routeParams',
function ($rootScope, $scope, $http, $window, $routeParams) {

    $rootScope.PageName = "Editer Bon de Reception";
    $scope.BonReceptionId = $routeParams.id;

    //Index de chargement des produits en fonction de la categorie
    $scope.SelectedItem = null;
    $scope.Panier = [];
    $scope.Fournisseur = {};
    $scope.TotauxPanierHt = 0;
    $scope.TotauxPanierTva = 0;
    $scope.TotauxPanierTtc = 0;

    if (!isNaN($scope.BonReceptionId)) {
        $http.get($rootScope.ServerURL + "BonReceptionFournisseur?id=" + $scope.BonReceptionId)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {

                $scope.Commentaire = response.Data.Commentaire;
                $scope.Panier = response.Data.Panier;
                $scope.Fournisseur = response.Data.Fournisseur;
                $scope.DateCreation = $rootScope.DateFormat(response.Data.DateCreation, "-");
                $scope.CommandesFournisseur = response.Data.CommandesFournisseur;

                $scope.Commande = {};
                $scope.Commande.Id = response.Data.Id;
                $scope.Commande.DateCreation = response.Data.DateCreation;
                $scope.Commande.Panier = $scope.Panier;
                $scope.Commande.Fournisseur = $scope.Fournisseur;
                $scope.Commande.Commentaire = $scope.Commentaire;
                $scope.Commande.CommandesFournisseur = $scope.CommandesFournisseur;

                $scope.CalculerTotalPanier();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.Init = function (produit) {
        $scope.IsNew = true;
        $scope.ProduitAajouter = {};
        $scope.ProduitAajouter.Produit = produit;
        $scope.ProduitAajouter.PrixAchat = produit.PrixAchat;
        $scope.ProduitAajouter.QuantiteUnitaire = 0;
        $scope.ProduitAajouter.QuantiteBlock = 0;
        $scope.ProduitAajouter.Tva = 0;
        $scope.ProduitAajouter.Ttc = 0;
        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * produit.PrixAchat;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * produit.PrixAchat;
        $scope.ProduitAajouter.Tht = Number($scope.ProduitAajouter.TotalBlock) + Number($scope.ProduitAajouter.TotalUnitaire);

        $scope.TypesDeVente = [];
        if (produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: produit.Block.Libelle });
            if (produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            else
                $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        }
    }

    $scope.Calculer = function () {
        if (isNaN($scope.ProduitAajouter.QuantiteBlock))
            $scope.ProduitAajouter.QuantiteBlock = 0;

        if (isNaN($scope.ProduitAajouter.QuantiteUnitaire))
            $scope.ProduitAajouter.QuantiteUnitaire = 0;

        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * $scope.ProduitAajouter.PrixAchat;//PrixBlock;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * $scope.ProduitAajouter.PrixAchat;//PrixUnitaire;

        $scope.ProduitAajouter.Tht = 0;
        if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
            $scope.ProduitAajouter.Tht = $scope.ProduitAajouter.TotalUnitaire;
        else if ($scope.ProduitAajouter.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
            $scope.ProduitAajouter.Tht = $scope.ProduitAajouter.TotalBlock;

        $scope.ProduitAajouter.MontantTva = (($scope.ProduitAajouter.Tva / 100) * $scope.ProduitAajouter.Tht);
        $scope.ProduitAajouter.Ttc = Number($scope.ProduitAajouter.MontantTva) + Number($scope.ProduitAajouter.Tht);
    }

    $scope.AjouterAuPanier = function (produit) {
        if ($scope.IsNew)
            $scope.Panier.push(produit);
        else
            $scope.Panier = angular.copy($scope.Panier);

        $scope.CalculerTotalPanier();
    }

    $scope.CalculerTotalPanier = function () {
        //Calculer le montant total du panier
        $scope.TotauxPanierHt = 0;
        $scope.TotauxPanierTva = 0;
        $scope.TotauxPanierTtc = 0;

        for (i = 0; i < $scope.Panier.length; i++) {
            item = $scope.Panier[i];
            if (item.TypeVenteId == $rootScope.TYPE_VENTE_UNITE_KEY)
                $scope.TotauxPanierHt = Number($scope.TotauxPanierHt) + Number(item.TotalUnitaire);
            else if (item.TypeVenteId == $rootScope.TYPE_VENTE_BLOCK_KEY)
                $scope.TotauxPanierHt = Number($scope.TotauxPanierHt) + Number(item.TotalBlock);

            $scope.TotauxPanierTva = Number($scope.TotauxPanierTva) + Number(item.MontantTva);
            $scope.TotauxPanierTtc = Number($scope.TotauxPanierTtc) + Number(item.Ttc);
        }
    }

    $scope.AncienPanier = [];
    $scope.Modifier = function (data) {
        $scope.ProduitAajouter = data;
        angular.copy($scope.Panier, $scope.AncienPanier);
        $scope.IsNew = false;

        //Mise a jour des type de vente
        $scope.TypesDeVente = [];
        if (data.Produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: data.Produit.Block.Libelle });
            if (data.Produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: data.Produit.Unite.Libelle });
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: data.Produit.Unite.Libelle });
        }
    }

    $scope.SupprimerItemPanier = function (index) {
        $scope.Panier.splice(index, 1);
        $scope.CalculerTotalPanier();
    }

    $scope.Annuler = function () {
        if (!$scope.IsNew)
            $scope.Panier = angular.copy($scope.AncienPanier);
    }

    $scope.ChargerProduits = function (categorie) {
        $scope.SelectedItem = categorie.Id;
        $http.get($rootScope.ServerURL + "Produits?categorie=" + categorie.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.ProduitsParCategorie = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.ResetComptoir = function () {
        $rootScope.CloseModal('confirmationAnnulation');
        $window.location.href = "#/BonReceptionFournisseur";
    }

    $scope.SendData = function (bonDeReception) {
        $scope.Commande.Panier = $scope.Panier;
        $scope.Commande.Fournisseur = $scope.Fournisseur;
        $scope.Commande.Commentaire = $scope.Commentaire;
        $scope.Commande.DateCreation = $scope.DateCreation;
        $scope.Commande.CommandesFournisseur = $scope.CommandesFournisseur;
        $scope.Commande.MarquerRecu = bonDeReception;

        $http.post($rootScope.ServerURL + "BonReceptionFournisseur", $scope.Commande)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.CloseModal('confirmationValidation');
                $window.location.href = "#/BonReceptionFournisseur";
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //Recuperer les categories
    $rootScope.LISTE_CATEGORIES_DATA();
    $rootScope.LISTE_FOURNISSEURS_DATA();

}]);

//DepensesController
MainController.controller('ComptesController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Comptes utilisateurs";
    $scope.ActifTable = [{ Id: true, Libelle: 'Oui' }, { Id: false, Libelle: 'Non' }];

    $scope.CopyOfComptes = [];//Sauvegarde
    $scope.Init = function () {
        $scope.Compte = {};
        //$scope.Compte.IsActif = "true";
        //Faire une sauvegarde les produits existants
        angular.copy($scope.Comptes, $scope.CopyOfComptes);
    }

    $scope.SendData = function () {
        $http.post($rootScope.ServerURL + "Comptes", $scope.Compte)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_COMPTES_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.ChangeCompteStatus = function (user, activer) {
        $http.get($rootScope.ServerURL + "Comptes?user=" + user + "&activer=" + activer)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_COMPTES_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.Modifier = function (data) {
        $scope.Compte = data;
        angular.copy($scope.Comptes, $scope.CopyOfComptes);
    }

    //Charger les données
    $rootScope.LISTE_COMPTES_DATA();
    $rootScope.LISTE_PRIVILEGES_DATA();

}]);

//CaisseController
MainController.controller('CaisseController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Caisse";

    $scope.CloturerCaisse = function () {

        $scope.Data = {
            ListStockPhysique: $scope.StockPhysiques,
            CaisseId: $scope.CaisseInventairePhysique,
            UserId: $scope.Profil.Id
        };

        $http.post($rootScope.ServerURL + "Stocks", $scope.Data)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_CAISSES_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.GetSortieDeCaisse = function (caisse) {
        $http.get($rootScope.ServerURL + "SortieDeCaisses?caisse=" + caisse)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.SortiesDeCaisses = response.Data;
                $scope.TotalSortiesDeCaisses = response.Sender;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //PRODUITS 
    $scope.GetProduisSansQuantite = function () {
        $http.get($rootScope.ServerURL + "Stocks")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.StockPhysiques = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.CaisseInventairePhysique = null; 
    $scope.GetProduitsPourInventairePhysique = function (caisse) {
        $scope.CaisseInventairePhysique = caisse;

        //Recuperations de tous les produits
        //$rootScope.LISTE_PRODUITS_DATA();
        $scope.GetProduisSansQuantite();
    }

    $rootScope.LISTE_CAISSES_DATA();
}]);

//SortiesDeCaisseController
MainController.controller('SortiesDeCaisseController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Sorties de caisse";

    $scope.Init = function () {
        $scope.SortiesDeCaisse = {};
    }

    $scope.SendData = function () {
        $scope.SortiesDeCaisse.User = $scope.Profil;
        $http.post($rootScope.ServerURL + "SortieDeCaisses", $scope.SortiesDeCaisse)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_SORTIESDECAISSES_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.Annuler = function () {
        $scope.SortiesDeCaisses = angular.copy($scope._copyOfSortiesDeCaisses);
    }

    $scope._copyOfSortiesDeCaisses = [];
    $scope.Modifier = function (data) {
        $scope.SortiesDeCaisse = data;
        angular.copy($scope.SortiesDeCaisses, $scope._copyOfSortiesDeCaisses);
    }

    $rootScope.LISTE_SORTIESDECAISSES_DATA();
}]);

//InfosSessionCaisseController
MainController.controller('InfosSessionCaisseController', ['$rootScope', '$scope', '$http', '$routeParams',
function ($rootScope, $scope, $http, $routeParams) {

    $rootScope.PageName = "Infos sessions de caisse";
    $scope.idCaisse = $routeParams.caisse;

    if (isNaN($scope.idCaisse)) window.location = "#/Home";
    
    $scope.Cloturer = function () {
        $http.get($rootScope.ServerURL + "Caisses?cloturer=" + $scope.idCaisse + "&user="+$rootScope.Profil.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_SESSIONCAISSES_DATA($scope.idCaisse);
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.FermerSession = function (sessionCaisse) {
        //$scope.SortiesDeCaisse.User = $scope.Profil;
        $http.get($rootScope.ServerURL + "SessionCaisses?cloturer=" + sessionCaisse)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_SESSIONCAISSES_DATA($scope.idCaisse);
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.GetSortieDeCaisse = function (sessiondecaisse) {
        $http.get($rootScope.ServerURL + "SortieDeCaisses?sessiondecaisse=" + sessiondecaisse)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.SortiesDeCaisses = response.Data;
                $scope.TotalSortiesDeCaisses = response.Sender;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //JOURNAL SESSION DE CAISSE
    $scope.CurrentSession = {};
    $scope.JournalSessionCaisse = function (session) {
        $scope.CurrentSession = session;
        $scope.ListJournal = [];

        $http.get($rootScope.ServerURL + "Journal?sessionCaisse=" + session.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.ListJournal = response.Data;
            } else {//Error
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.DelItem = {};
    $scope.DelItemAction = function (journal, session) {
        $scope.DelItem = journal;
    }
    //
    //JOURNAL 
    $scope.Del = function () {
        if ($scope.DelItem.OpCode === "OP_FACTURE") {
            $http.get($rootScope.ServerURL + "FacturesClient?del=" + $scope.DelItem.IdOperation)
            .success(function (response) {
                console.log(response);
                if (response.ResponseCode == 0) {
                    $rootScope.LISTE_SESSIONCAISSES_DATA($scope.idCaisse);
                    $scope.JournalSessionCaisse($scope.CurrentSession);
                } else {//Error
                    $rootScope.Alert(response.Message);
                }
            })
            .error(function (response) {
                $rootScope.Alert($rootScope.NetworkError);
            });
        } else if ($scope.DelItem.OpCode === "OP_VERSEMENT") {
            $http.get($rootScope.ServerURL + "Paiements?del=" + $scope.DelItem.IdOperation)
            .success(function (response) {
                console.log(response);
                if (response.ResponseCode == 0) {
                    $rootScope.LISTE_SESSIONCAISSES_DATA($scope.idCaisse);
                    $scope.JournalSessionCaisse($scope.CurrentSession);
                } else {//Error
                    $rootScope.Alert(response.Message);
                }
            })
            .error(function (response) {
                $rootScope.Alert($rootScope.NetworkError);
            });
        }

        
    }
    
    $rootScope.LISTE_SESSIONCAISSES_DATA($scope.idCaisse);
}]);

//SessionCaisseController
MainController.controller('SessionCaisseController', ['$rootScope', '$scope', '$http', '$routeParams',
function ($rootScope, $scope, $http, $routeParams) {

    $rootScope.PageName = "Sessions de caisse";

    $scope.CalculerDifference = function () {
        $scope.SessionCaisseUser.DifferenceTotalEspece = Number($scope.SessionCaisseUser.TotalEspeceTheorique) - Number($scope.SessionCaisseUser.TotalEspeceSaisi);
    }

    $scope.FermerSession = function () {
        $scope.SessionCaisseUser.User = $scope.Profil.Id;
        $http.get($rootScope.ServerURL + "SessionCaisses?user=" + $scope.SessionCaisseUser.User + "&especeSaisi=" + $scope.SessionCaisseUser.TotalEspeceSaisi + "&observation=" + $scope.SessionCaisseUser.Observation)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.SESSIONCAISSE_USER_DATA();
            } else {//Error
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $rootScope.SESSIONCAISSE_USER_DATA();
    $rootScope.JOURNAL_SESSIONCAISSE_DATA();//Recuperer la derniere session
}]);

//InventaireDeStockController
MainController.controller('InventaireDeStockController', ['$rootScope', '$scope', '$http', '$routeParams',
function ($rootScope, $scope, $http, $routeParams) {

    $rootScope.PageName = "Inventaire du stock";
    $scope.idCaisse = $routeParams.caisse;

    if (isNaN($scope.idCaisse)) window.location = "#/Home";

    //LISTE SESSION DE CAISSE USER OUVERTE
    $rootScope.INVENTAIRE_DATA = function () {
        $http.get($rootScope.ServerURL + "Produits?inventaire=" + $scope.idCaisse)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Inventaires = response.Data;
                $scope.Caisse = response.Sender;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.detailsProduits = function (inventaire) {
        $scope.detailsFactureClient = inventaire.DetailsFactureClient;
    }

    $rootScope.INVENTAIRE_DATA();
}]);

//BanquesController
MainController.controller('BanquesController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Banque";

    $scope.CopyOfBanques = [];//Sauvegarde
    $scope.Init = function () {
        $scope.Banque = {};
        //Faire une sauvegarde de l'existants
        angular.copy($scope.Banque, $scope.CopyOfBanques);

        $scope.Calculer();
    }

    $scope.Annuler = function () {
        $scope.Produits = angular.copy($scope.CopyOfBanques);
    }

    //LISTE SESSION DE CAISSE USER OUVERTE
    $rootScope.BanqueInfos = function () {
        $http.get($rootScope.ServerURL + "Banques")
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.TotalEnBanque = response.Data.TotalEnBanque;
                $scope.TotalEnCaisse = response.Data.TotalEnCaisse;
                $scope.Transactions = response.Data.ListVersement;

                $scope.Banque = {};
                $scope.Banque.TotalEnBanque = $scope.TotalEnBanque;
                $scope.Banque.TotalEnCaisse = $scope.TotalEnCaisse;

                //console.log($scope.Banque);
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.Calculer = function () {
        $scope.ResteEnCaisse = $scope.TotalEnCaisse - $scope.Banque.Versement;
    }

    $scope.SendData = function () {
        $scope.Banque.User = {};
        $scope.Banque.User.Id = $rootScope.Profil.Id;
        $http.post($rootScope.ServerURL + "Banques", $scope.Banque)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.BanqueInfos();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $rootScope.BanqueInfos();
}]);

//TransfertsController
MainController.controller('TransfertsController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Transferts de produits";

    $scope.MarquerCommeTransfere = function () {
        $http.get($rootScope.ServerURL +
        "Transfert?marquerCommeTransfere=" + $scope.Transfert.Id +
        "&date=" + $scope.DateValidation)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.LISTE_TRANSFERT_DATA();
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.InfosTransfert = function (data) {
        $scope.Commande = { };
        $http.get($rootScope.ServerURL + "Transfert?id=" +data.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.Transfert = response.Data;
                } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
                }
            })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
            });
    }

    $scope.SetTransfert = function (data) {
        $scope.Transfert = data;
    }

    $rootScope.LISTE_TRANSFERT_DATA();
}]);

//NouveauTransfertsController
MainController.controller('NouveauTransfertsController', ['$rootScope', '$scope', '$http','$window',
function ($rootScope, $scope, $http, $window) {

    $rootScope.PageName = "Nouveau transfert de produits";

    //Index de chargement des produits en fonction de la categorie
    $scope.SelectedItem = null;
    $scope.Panier = [];

    $scope.Init = function (produit) {
        $scope.IsNew = true;
        $scope.ProduitAajouter = {};
        $scope.ProduitAajouter.Produit = produit;
        $scope.ProduitAajouter.PrixAchat = produit.PrixAchat;
        $scope.ProduitAajouter.QuantiteUnitaire = 0;
        $scope.ProduitAajouter.QuantiteBlock = 0;
        $scope.ProduitAajouter.Tva = 0;
        $scope.ProduitAajouter.Ttc = 0;
        $scope.ProduitAajouter.TotalBlock = $scope.ProduitAajouter.QuantiteBlock * produit.PrixAchat;
        $scope.ProduitAajouter.TotalUnitaire = $scope.ProduitAajouter.QuantiteUnitaire * produit.PrixAchat;
        $scope.ProduitAajouter.Tht = Number($scope.ProduitAajouter.TotalBlock) + Number($scope.ProduitAajouter.TotalUnitaire);

        $scope.TypesDeVente = [];
        if (produit.Unite.IsBlock) {//Vente au carton
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_BLOCK_KEY, Libelle: produit.Block.Libelle });
            if (produit.UniteParBlock > 1)
                $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            else
                $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        } else {//Vente en plaquette
            $scope.TypesDeVente.push({ Id: $rootScope.TYPE_VENTE_UNITE_KEY, Libelle: produit.Unite.Libelle });
            $scope.ProduitAajouter.TypeVenteId = $scope.TypesDeVente[0].Id;//Selectionner le premier élément
        }
    }

    $scope.AjouterAuPanier = function (produit) {
        if ($scope.IsNew)
            $scope.Panier.push(produit);
        else
            $scope.Panier = angular.copy($scope.Panier);
    }

    $scope.SupprimerItemPanier = function (index) {
        $scope.Panier.splice(index, 1);
    }

    $scope.Annuler = function () {
        if (!$scope.IsNew)
            $scope.Panier = angular.copy($scope.AncienPanier);
    }

    $scope.ResetComptoir = function () {
        $scope.SelectedItem = null;
        $scope.Commentaire = null;
        $scope.Panier = [];
        $scope.TotauxPanierHt = 0;
        $scope.TotauxPanierTva = 0;
        $scope.TotauxPanierTtc = 0;
        $scope.ProduitsParCategorie = [];
        $scope.Vente = {};
        $scope.Fournisseur = {};
    }

    $scope.ChargerProduits = function (categorie) {
        $scope.SelectedItem = categorie.Id;
        $http.get($rootScope.ServerURL + "Produits?categorie=" + categorie.Id)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $scope.ProduitsParCategorie = response.Data;
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    $scope.SendData = function () {
        $scope.Transfert = {};
        $scope.Transfert.Panier = $scope.Panier;
        $scope.Transfert.Fournisseur = $scope.Fournisseur;
        $scope.Transfert.Commentaire = $scope.Commentaire;
        $scope.Transfert.DateCreation = $scope.DateCreation;

        $http.post($rootScope.ServerURL + "Transfert", $scope.Transfert)
        .success(function (response) {
            console.log(response);
            if (response.ResponseCode == 0) {
                $rootScope.CloseModal('confirmationValidation');
                $window.location.href = "#/Transferts";
            } else {//Error
                console.log(response);
                $rootScope.Alert(response.Message);
            }
        })
        .error(function (response) {
            console.log(response);
            $rootScope.Alert($rootScope.NetworkError);
        });
    }

    //Recuperer les categories
    $rootScope.LISTE_CATEGORIES_DATA();
    $rootScope.LISTE_FOURNISSEURS_INTERNE_DATA();
}]);

//AdminUtilisateursController
MainController.controller('AdminUtilisateursController', ['$rootScope', '$scope', '$http',
function ($rootScope, $scope, $http) {

    $rootScope.PageName = "Utilisateurs";
    $rootScope.PageDescription = "Administration des Utilisateurs";

}]);
