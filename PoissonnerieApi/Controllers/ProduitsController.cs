using BusinessEngine.DataModel;
using BusinessEngine.Manager;
using BusinessEngine.Models;
using Newtonsoft.Json.Linq;
using PoissonnerieApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PoissonnerieApi.Controllers
{
    public class ProduitsController : ApiController
    {

        // GET api/produits
        public object GetAll(bool withQuantity = true)
        {
            ResponseData responseData;
            try
            {
                var produits = DataManager.GetAll<Produit>();
                if (withQuantity)
                {
                    foreach (var produit in produits)
                    {
                        CalculerQuantiteProduit(produit);
                    }
                }
                else
                {
                    foreach (var prod in produits)
                    {
                        prod.QuantiteStockBlock = 0;
                        prod.QuantiteStockResteEnUnite = 0;
                    }
                }
                responseData = ResponseData.GetSuccess(produits);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/produits?categorie=5
        public object GetProduitsParCategorie(long categorie)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.GetList<Produit>("Categorie.Id", categorie, "Libelle ASC"));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/produits?categorieAvecQuantite=5
        public object GetProduitsParCategorieAvecQuantite(long categorieAvecQuantite)
        {
            ResponseData responseData;
            try
            {
                var produits = DataManager.GetList<Produit>("Categorie.Id", categorieAvecQuantite, "Libelle ASC");
                foreach (var produit in produits)
                    CalculerQuantiteProduit(produit);
                responseData = ResponseData.GetSuccess(produits);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/produits/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.Get<Produit>(id));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/produits
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var _data = data.ToObject<Produit>();
                if (_data.Id > 0)
                {
                    _data.DateModification = DateTime.UtcNow;
                }else{
                    _data.DateCreation = DateTime.UtcNow;
                    _data.DateModification = DateTime.UtcNow;
                }

                DataManager.Save(_data);
                responseData = ResponseData.GetSuccess(_data);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/produits/5
        public object GetInventaire(long inventaire)
        {
            ResponseData responseData;
            try
            {
                var _caisse = DataManager.Get<Caisse>(inventaire);

                //Recuperer la liste des produits
                var produits = DataManager.GetAll<Produit>("Libelle");
                //Recuperer chaque produits et faire l'inventaire
                var listInventaire = new List<Inventaire>();
                foreach (var produit in produits)
                {
                    var inv = new Inventaire();
                    inv.Produit = produit;

                    #region STOCK INITIAL
                    //Recuperer la liste des produits en stock avant date debut ouverture de caisse
                    //Recuperer la liste des produits vendu avant date debut ouverture de caisse
                    //Faire la différence pour avoir la liste du stock initial

                    ///-------- ENTREE INITIAL ---------
                    //1. On recupère tous les produits des bon de reception du reçu
                    var detailsBonReceptionParProduit = DataManager.GetListProduitsParBonRecu(produit.Id, _caisse.DateOuverture);
                    //2. On calcul le nombre en unité : Quantité x UniteParBlock
                    var produitsEntres = CalculerQuantiteEntreeEnStockProduits(produit, detailsBonReceptionParProduit);

                    ///-------- SORTIE INITIAL ---------
                    //3. On recupère les produits vendu : factures (A revoir: Bon de livraison livré)
                    var facturesClientInitialSorti = DataManager.GetListProduitsParFactures(produit.Id, _caisse.DateOuverture);
                    decimal produitsSorti = CalculerQuantiteSortieDuStockProduits(produit, facturesClientInitialSorti);

                    ///-------- TRANSFERT INITIAL ---------
                    //4. On recupère la liste des produit transferé
                    var transfertInitialSorti = DataManager.GetListProduitsParTransfert(produit.Id, _caisse.DateOuverture);
                    decimal produitTransfere = CalculerQuantiteTransfertParProduits(produit, transfertInitialSorti);

                    //5. On fait la difference
                    decimal produitsInitial = produitsEntres - produitsSorti - produitTransfere;

                    if (produit.Unite.IsBlock)
                        inv.StockInitialBlock = (long)(produitsInitial / produit.UniteParBlock);
                    else
                        inv.StockInitialBlock = produitsInitial / produit.UniteParBlock;

                    inv.StockInitialResteUnite = produitsInitial % produit.UniteParBlock;
                    #endregion

                    #region STOCK VENDU
                    //Facture client vendu
                    var facturesClientVendu = DataManager.GetListProduitsVenduParFactures(produit.Id, _caisse.DateOuverture);
                    var produitsVendus = CalculerQuantiteSortieDuStockProduits(produit, facturesClientVendu);

                    var listTransfereEffectue = DataManager.GetListProduitsTransferes(produit.Id, _caisse.DateOuverture);
                    var transfereEffectue = CalculerQuantiteTransfertParProduits(produit, listTransfereEffectue);

                    if (produitsVendus > 0)
                    {
                        inv.IsVendu = true;

                        if (produit.Unite.IsBlock)
                            inv.StockVenduBlock = (long)(produitsVendus / produit.UniteParBlock);
                        else
                            inv.StockVenduBlock = produitsVendus / produit.UniteParBlock;
                        inv.StockVenduResteUnite = produitsVendus % produit.UniteParBlock;
                    }

                    if (transfereEffectue > 0)
                    {
                        inv.IsVendu = true;

                        if (produit.Unite.IsBlock)
                            inv.StockTransfereBlock = (long)(transfereEffectue / produit.UniteParBlock);
                        else
                            inv.StockTransfereBlock = transfereEffectue / produit.UniteParBlock;
                        inv.StockTransfereResteUnite = transfereEffectue % produit.UniteParBlock;
                    }
                    #endregion

                    #region STOCK FINAL
                    var produitsFinaux = produitsInitial - produitsVendus - transfereEffectue;
                    if (produit.Unite.IsBlock)
                        inv.StockFinalBlock = (long)(produitsFinaux / produit.UniteParBlock);
                    else
                        inv.StockFinalBlock = produitsFinaux / produit.UniteParBlock;
                    inv.StockFinalResteUnite = produitsFinaux % produit.UniteParBlock;
                    #endregion

                    #region STOCK PHYSIQUE
                    decimal produitsStockPhysiques = 0;

                    var stockPhysique = DataManager.GetStockPhysique(_caisse.Id, produit.Id);
                    if (stockPhysique != null)
                    {
                        inv.StockPhysiqueBlock = stockPhysique.QuantiteStockBlock;
                        inv.StockPhysiqueResteUnite = stockPhysique.QuantiteStockResteEnUnite;

                        if (produit.Unite.IsBlock)
                        {
                            produitsStockPhysiques = (stockPhysique.QuantiteStockBlock * produit.UniteParBlock);
                            produitsStockPhysiques += stockPhysique.QuantiteStockResteEnUnite;
                        }
                        else
                        {
                            produitsStockPhysiques = (stockPhysique.QuantiteStockBlock * produit.UniteParBlock);
                        }
                    }

                    var diff = produitsStockPhysiques - produitsFinaux;
                    inv.StockDiffBlock = diff / produit.UniteParBlock;
                    if (produit.Unite.IsBlock)
                        inv.StockDiffBlock = (long)(inv.StockDiffBlock);
                    inv.StockDiffResteUnite = diff % produit.UniteParBlock;
                    #endregion

                    listInventaire.Add(inv);
                }

                responseData = ResponseData.GetSuccess(listInventaire, _caisse);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        private decimal CalculerQuantiteEntreeEnStockProduits(Produit p, List<DetailsBonReceptionFournisseur> detailsBonReception)
        {
            decimal produitsEntre = 0;
            foreach (var detailsBr in detailsBonReception)
            {
                if (detailsBr.TypeColisage == Constants.TYPE_BLOCK_KEY)
                {
                    produitsEntre += (detailsBr.Quantite * p.UniteParBlock);
                }
                else if (detailsBr.TypeColisage == Constants.TYPE_UNITE_KEY)
                {
                    produitsEntre += detailsBr.Quantite;
                }
            }

            return produitsEntre;
        }

        private decimal CalculerQuantiteSortieDuStockProduits(Produit p, List<DetailsFacturesClient> detailsFacturesClient)
        {
            decimal produitsSorti = 0;
            foreach (var factureCash in detailsFacturesClient)
            {
                //4. On calcul le nombre en unité : Quantité x UniteParBlock
                if (factureCash.TypeColisage == Constants.TYPE_BLOCK_KEY)
                {
                    produitsSorti += (factureCash.Quantite * p.UniteParBlock);
                }
                else if (factureCash.TypeColisage == Constants.TYPE_UNITE_KEY)
                {
                    produitsSorti += factureCash.Quantite;
                }
            }

            return produitsSorti;
        }

        private decimal CalculerQuantiteTransfertParProduits(Produit p, List<DetailsTransfert> detailsTransfert)
        {
            decimal produitsSorti = 0;
            foreach (var details in detailsTransfert)
            {
                //4. On calcul le nombre en unité : Quantité x UniteParBlock
                if (details.TypeColisage == Constants.TYPE_BLOCK_KEY)
                {
                    produitsSorti += (details.Quantite * p.UniteParBlock);
                }
                else if (details.TypeColisage == Constants.TYPE_UNITE_KEY)
                {
                    produitsSorti += details.Quantite;
                }
            }

            return produitsSorti;
        }

        private void CalculerQuantiteProduit(Produit p)
        {
            ///-------- ENTREE ---------
            //1. On recupère tous les produits des bon de reception du reçu
            var detailsBonReceptionParProduit = DataManager.GetListProduitsParBonRecu(p.Id);
            //2. On calcul le nombre en unité : Quantité x UniteParBlock
            decimal produitsEntre = CalculerQuantiteEntreeEnStockProduits(p, detailsBonReceptionParProduit);

            ///-------- SORTIE ---------
            //3. On recupère les produits vendu : factures (A revoir: Bon de livraison livré)
            var facturesClient = DataManager.GetList<DetailsFacturesClient>("Produit.Id", p.Id);
            decimal produitsSorti = CalculerQuantiteSortieDuStockProduits(p, facturesClient);

            ///-------- TRANSFERT INITIAL ---------
            //4. On recupère la liste des produit transferé
            var transfertInitialSorti = DataManager.GetListProduitsTransferes(p.Id);
            decimal produitTransfere = CalculerQuantiteTransfertParProduits(p, transfertInitialSorti);

            //5. On fait la difference
            decimal produitsRestant = produitsEntre - produitsSorti - produitTransfere;

            if(p.Unite.IsBlock)
                p.QuantiteStockBlock = (long)(produitsRestant / p.UniteParBlock);
            else
                p.QuantiteStockBlock = produitsRestant / p.UniteParBlock;

            p.QuantiteStockResteEnUnite = produitsRestant % p.UniteParBlock;
        }
    }
}
