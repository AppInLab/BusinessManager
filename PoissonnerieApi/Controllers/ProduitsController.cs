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
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                var produits = DataManager.GetAll<Produit>();
                foreach (var produit in produits)
                {
                    CalculerQuantiteProduit(produit);
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
                responseData = ResponseData.GetSuccess(DataManager.GetList<Produit>("Categorie.Id", categorie));
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
                var produits = DataManager.GetList<Produit>("Categorie.Id", categorieAvecQuantite);
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

        private void CalculerQuantiteProduit(Produit p)
        {
            ///-------- ENTREE ---------
            //1. On recupère tous les produits des bon de reception du reçu
            var detailsBonReceptionParProduit = DataManager.GetListProduitsParBonRecu(p.Id);
            //2. On calcul le nombre en unité : Quantité x UniteParBlock

            decimal produitsEntre = 0;
            foreach (var detailsBr in detailsBonReceptionParProduit)
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

            ///-------- SORTIE ---------
            //3. On recupère les produits vendu : Bon de livraison livré & factures
            var facturesClientCash = DataManager.GetListProduitsParFacturesCash(p.Id);
            decimal produitsSorti = 0;
            foreach (var factureCash in facturesClientCash)
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

            //5. On fait la difference
            decimal produitsRestant = produitsEntre - produitsSorti;

            if(p.Unite.IsBlock)
                p.QuantiteStockBlock = (long)(produitsRestant / p.UniteParBlock);
            else
                p.QuantiteStockBlock = produitsRestant / p.UniteParBlock;

            p.QuantiteStockResteEnUnite = produitsRestant % p.UniteParBlock;
        }
    }
}
