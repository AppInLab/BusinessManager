var app = angular.module('poissonnerie',
    [
        'MainController',
        'ngRoute',
        'angularUtils.directives.dirPagination',
        'angular-loading-bar',
        'ngCookies',
        'bootstrap-tagsinput',
        'angular.filter',
        'ngTagsInput'
    ]
    );

app.config(["$routeProvider",
        function ($routeProvider) {

            //Acueil            
            $routeProvider.when("/Home", {
                templateUrl: "views/home.html",
                controller: "HomeController"
            });

            //Depots
            $routeProvider.when("/Depots", {
                templateUrl: "views/depot.html",
                controller: "DepotController"
            });

            //Categories
            $routeProvider.when("/Categories", {
                templateUrl: "views/categorie.html",
                controller: "CategorieController"
            });

            //Produits            
            $routeProvider.when("/Produits", {
                templateUrl: "views/produit.html",
                controller: "ProduitsController"
            });

            //Clients            
            $routeProvider.when("/Clients", {
                templateUrl: "views/client.html",
                controller: "ClientController"
            });

            //Fiche Clients            
            $routeProvider.when("/FicheClients/:client", {
                templateUrl: "views/ficheclient.html",
                controller: "FicheClientController"
            });


            //Fournisseurs            
            $routeProvider.when("/Fournisseurs", {
                templateUrl: "views/fournisseur.html",
                controller: "FournisseurController"
            });

            //Vente           
            $routeProvider.when("/VenteComptoir", {
                templateUrl: "views/ventecomptoir.html",
                controller: "VenteController"
            });

            //CommandeFournisseur            
            $routeProvider.when("/CommandeFournisseur", {
                templateUrl: "views/commandefournisseur.html",
                controller: "CommandeFournisseurController"
            });

            //NouvelleCommandeFournisseur            
            $routeProvider.when("/NouvelleCommandeFournisseur", {
                templateUrl: "views/nouvellecommandefournisseur.html",
                controller: "NouvelleCommandeFournisseurController"
            });

            //EditerCommandeFournisseur            
            $routeProvider.when("/EditerCommandeFournisseur/:id", {
                templateUrl: "views/editercommandefournisseur.html",
                controller: "EditerCommandeFournisseurController"
            });

            //BonReception            
            $routeProvider.when("/BonReceptionFournisseur", {
                templateUrl: "views/bonreceptionfournisseur.html",
                controller: "BonReceptionFournisseurController"
            });

            //NouveauBonReceptionFournisseur            
            $routeProvider.when("/NouveauBonReceptionFournisseur", {
                templateUrl: "views/nouveaubonreceptionfournisseur.html",
                controller: "NouveauBonReceptionFournisseurController"
            });

            //EditerBonReceptionFournisseur            
            $routeProvider.when("/EditerBonReceptionFournisseur/:id", {
                templateUrl: "views/editerbonreceptionfournisseur.html",
                controller: "EditerBonReceptionFournisseurController"
            });

            //Caisse            
            $routeProvider.when("/Caisses", {
                templateUrl: "views/caisse.html",
                controller: "CaisseController"
            });

            //SortiesDeCaisse            
            $routeProvider.when("/SortiesDeCaisses", {
                templateUrl: "views/sortiesdecaisse.html",
                controller: "SortiesDeCaisseController"
            });

            //SessionCaisses            
            $routeProvider.when("/InfosSessionCaisses/:caisse", {
                templateUrl: "views/infossessioncaisse.html",
                controller: "InfosSessionCaisseController"
            });
            
            //ClotureDeCaisses           
            $routeProvider.when("/SessionCaisses", {
                templateUrl: "views/sessioncaisse.html",
                controller: "SessionCaisseController"
            });

            //InventaireDeStock caisse    
            $routeProvider.when("/InventaireDeStocks/:caisse", {
                templateUrl: "views/inventaire.html",
                controller: "InventaireDeStockController"
            });

            //Gestion des comptes          
            $routeProvider.when("/Comptes", {
                templateUrl: "views/compte.html",
                controller: "ComptesController"
            });

            //Gestion des Deplacements de produits      
            $routeProvider.when("/DeplacerProduits", {
                templateUrl: "views/deplacerproduit.html",
                controller: "DeplacerProduitsController"
            });

            //Gestion des Chineurs
            $routeProvider.when("/AdminChineurs", {
                templateUrl: "views/Admin/AdminChineurs.html",
                controller: "AdminChineursController"
            });

            //Gestion des Utilisateurs      
            $routeProvider.when("/AdminUtilisateurs", {
                    templateUrl: "views/Admin/AdminUtilisateurs.html",
                    controller: "AdminUtilisateursController"
                })
                .otherwise({
                    redirectTo: "/Home"
                });

        }]);

app.config(function (paginationTemplateProvider) {
    paginationTemplateProvider.setPath('libjs/dirPagination.tpl.html');
});

app.config(["$httpProvider",
        function ($httpProvider) {
            // Use x-www-form-urlencoded Content-Type
            $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];
            //$httpProvider.defaults.withCredentials = true;
            /**
             * The workhorses; converts an object to x-www-form-urlencoded serialization.
             * @param {Object} obj
             * @return {String}
             */
            var param = function (obj) {
                var query = '', name, value, fullSubName, subName, subValue, innerObj, i;

                for (name in obj) {
                    value = obj[name];

                    if (value instanceof Array) {
                        for (i = 0; i < value.length; ++i) {
                            subValue = value[i];
                            fullSubName = name + '[' + i + ']';
                            innerObj = {};
                            innerObj[fullSubName] = subValue;
                            query += param(innerObj) + '&';
                        }
                    }
                    else if (value instanceof Object) {
                        for (subName in value) {
                            subValue = value[subName];
                            fullSubName = name + '[' + subName + ']';
                            innerObj = {};
                            innerObj[fullSubName] = subValue;
                            query += param(innerObj) + '&';
                        }
                    }
                    else if (value !== undefined && value !== null)
                        query += encodeURIComponent(name) + '=' + encodeURIComponent(value) + '&';
                }

                return query.length ? query.substr(0, query.length - 1) : query;
            };

            // Override $http service's default transformRequest
            $httpProvider.defaults.transformRequest = [function (data) {
                return angular.isObject(data) && String(data) !== '[object File]' ? param(data) : data;
            }];
        }
]);


