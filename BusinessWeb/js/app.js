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

            //BonLivraison            
            $routeProvider.when("/BonReceptionFournisseur", {
                templateUrl: "views/bonreceptionfournisseur.html",
                controller: "BonReceptionFournisseurController"
            });

            //PointJournee            
            $routeProvider.when("/PointJour", {
                templateUrl: "views/PointJour.html",
                controller: "PointJourController"
            });

            //Depenses            
            $routeProvider.when("/Depenses", {
                templateUrl: "views/Depenses.html",
                controller: "DepensesController"
            });
            
            /* Menus Administrateurs */

            //Gestion des magasins           
            $routeProvider.when("/AdminMagasins", {
                templateUrl: "views/Admin/AdminMagasins.html",
                controller: "AdminMagasinsController"
            });

            //Gestion des catégories de produits            
            $routeProvider.when("/AdminCategories", {
                templateUrl: "views/Admin/AdminCategories.html",
                controller: "AdminCategoriesController"
            });

            //Gestion des Produits          
            $routeProvider.when("/AdminProduits", {
                templateUrl: "views/Admin/AdminProduits.html",
                controller: "AdminProduitsController"
            });

            //Gestion des fornisseurs      
            $routeProvider.when("/AdminFournisseurs", {
                templateUrl: "views/Admin/AdminFournisseurs.html",
                controller: "AdminFournisseursController"
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


