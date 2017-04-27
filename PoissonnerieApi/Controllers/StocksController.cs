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
    public class StocksController : ApiController
    {
        // GET api/stocks
        public object GetStockPhysiqueEmptyProduits()
        {
            ResponseData responseData;
            try
            {
                var produits = DataManager.GetAll<Produit>();
                var listStock = new List<StockPhysique>();
                foreach (var prod in produits)
                {
                    var stockPhysique = new StockPhysique();
                    stockPhysique.Produit = prod;

                    listStock.Add(stockPhysique);
                }

                responseData = ResponseData.GetSuccess(listStock);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/stocks/5
        //public object GetInventaire(long caisse)
        //{
        //    ResponseData responseData;
        //    try
        //    {
        //        var _caisse = DataManager.Get<Caisse>(caisse);

        //        //Recuperer la liste des produits
        //        var produits = DataManager.GetAll<Produit>("Libelle");
        //        //Recuperer chaque produits et faire l'inventaire
        //        var inventaire = new List<Inventaire>();
        //        foreach (var produit in produits)
        //        {
        //            var inv = new Inventaire();
        //            inv.Produit = produit;

        //            //Recuperer la liste des produits en stock avant date debut ouverture de caisse
        //            //Recuperer la liste des produits vendu avant date debut ouverture de caisse
        //            //Faire la différence pour avoir la liste du stock initial

        //        }

        //        responseData = ResponseData.GetSuccess(inventaire);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData = ResponseData.GetError(ex.Message);
        //    }

        //    return responseData;
        //}

        public object Post([FromBody] JToken data)
        {
            try
            {
                var stockPhysiqueData = data.ToObject<StockPhysiqueData>();

                var _user = DataManager.Get<User>(stockPhysiqueData.UserId);
                if (_user == null)
                {
                    return ResponseData.GetError("Utilisateur invalide !");
                }

                var caisse = DataManager.Get<Caisse>(stockPhysiqueData.CaisseId);
                if (caisse == null)
                {
                    return ResponseData.GetError("Caisse introuvable !");
                }

                if (caisse.IsClosed)
                {
                    return ResponseData.GetError("La caisse a déjà été fermer !");
                }

                //Verifier que toutes les sessions de caisse sont cloturées
                var sessionsDeCaisse = DataManager.GetSessionCaisseOuvertes(caisse.Id);
                if (sessionsDeCaisse.Any())
                    return ResponseData.GetError("Impossible de cloturer la caisse. Fermer les sessions de caisse en cours puis réessayer.");

                foreach (var sp in stockPhysiqueData.ListStockPhysique)
                {
                    //if (sp.QuantiteStockBlock == 0 && sp.QuantiteStockResteEnUnite == 0)
                    //    continue;
                    sp.Caisse = caisse;
                    sp.DateCreation = DateTime.UtcNow;
                    DataManager.Save(sp);
                }

                //Fermer la caisse
                caisse.IsClosed = true;
                caisse.DateCloture = DateTime.UtcNow;
                caisse.ClotureePar = _user;
                DataManager.Save(caisse);

                return ResponseData.GetSuccess(caisse);
            }
            catch (Exception ex)
            {
                return ResponseData.GetError(ex.Message);
            }
        }
    }

    public class StockPhysiqueData
    {
        public List<StockPhysique> ListStockPhysique { set; get; }
        public long CaisseId { set; get; }
        public long UserId { set; get; }
    }
}
