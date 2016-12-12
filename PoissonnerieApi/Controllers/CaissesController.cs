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
    public class CaissesController : ApiController
    {
        // GET api/caisses
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                var caisses = DataManager.GetAll<Caisse>("DateOuverture", "DESC");
                foreach (var caisse in caisses)
                {
                    caisse.SortieDeCaisseTotal = DataManager.GetSumSortieDeCaisse(caisse.Id);
                    caisse.EntreeDeCaisseTotal = DataManager.GetSumPaiementParCaisse(caisse.Id);
                    caisse.MontantTotalEnCaisse = caisse.EntreeDeCaisseTotal - caisse.SortieDeCaisseTotal;
                    //Recuperer les sessions de caisses
                    //var sessionDeCaisse = DataManager.GetList<SessionCaisse>("Caisse.Id", caisse.Id);
                }
                responseData = ResponseData.GetSuccess(caisses);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/caisses/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.Get<Caisse>(id));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/caisses?cloturer=idCaisse
        public object GetCloturer([FromUri]long cloturer, [FromUri]long user)
        {
            ResponseData responseData;
            try
            {
                var _user = DataManager.Get<User>(user);

                if (_user == null)
                {
                    return ResponseData.GetError("Utilisateur invalide !");
                }

                var caisse = DataManager.Get<Caisse>(cloturer);
                
                if (caisse == null)
                {
                    responseData = ResponseData.GetError("Caisse introuvable !");
                }
                else
                {
                    if (caisse.IsClosed)
                    {
                        return ResponseData.GetError("La caisse a déjà été fermer !");
                    }

                    //Verifier que toutes les sessions de caisse sont cloturées
                    var sessionsDeCaisse = DataManager.GetSessionCaisseOuvertes(caisse.Id);
                    if(sessionsDeCaisse.Any())
                        return ResponseData.GetError("Impossible de cloturer la caisse. Fermer les sessions de caisse en cours puis réessayer.");

                    caisse.IsClosed = true;
                    caisse.DateCloture = DateTime.UtcNow;
                    caisse.ClotureePar = _user;
                    DataManager.Save(caisse);
                    responseData = ResponseData.GetSuccess(caisse);
                }
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/caisses
        //public object Post([FromBody]JToken data)
        //{
        //    ResponseData responseData;
        //    try
        //    {
        //        var _data = data.ToObject<Caisse>();
        //        DataManager.Save(_data);
        //        responseData = ResponseData.GetSuccess(_data);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData = ResponseData.GetError(ex.Message);
        //    }

        //    return responseData;
        //}
    }
}
