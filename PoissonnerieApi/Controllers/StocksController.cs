using BusinessEngine.Manager;
using BusinessEngine.Models;
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
        //public object GetAll()
        //{
        //    ResponseData responseData;
        //    try
        //    {
        //        //Recuperer la session en cours
        //        responseData = ResponseData.GetSuccess(DataManager.GetAll<SessionCaisse>("DateOuverture", "DESC"));
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData = ResponseData.GetError(ex.Message);
        //    }

        //    return responseData;
        //}

        // GET api/stocks/5
        public object GetInventaire(long caisse)
        {
            ResponseData responseData;
            try
            {
                var _caisse = DataManager.Get<Caisse>(caisse);

                //Recuperer la liste des produits
                var produits = DataManager.GetAll<Produit>("Libelle");
                //Recuperer chaque produits et faire l'inventaire
                var inventaire = new List<Inventaire>();
                foreach (var produit in produits)
                {
                    var inv = new Inventaire();
                    inv.Produit = produit;

                    //Recuperer la liste des produits en stock avant date debut ouverture de caisse
                    //Recuperer la liste des produits vendu avant date debut ouverture de caisse
                    //Faire la différence pour avoir la liste du stock initial

                }

                responseData = ResponseData.GetSuccess(inventaire);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }
    }
}
