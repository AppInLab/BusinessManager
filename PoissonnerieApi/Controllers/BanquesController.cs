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
    public class BanquesController : ApiController
    {
        // GET api/banques
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                var sessionCaisses = DataManager.GetAll<SessionCaisse>();
                decimal totalEnCaisse = 0;

                //Recuperer toutes les sommes en caisse
                foreach (var session in sessionCaisses)
                {
                    totalEnCaisse += session.TotalEspeceFermeture;
                }

                //Deduire les versement déjà effectués en Banque
                var sommeVersementEnBaque = DataManager.GetSumVersementEnBanque();
                totalEnCaisse = totalEnCaisse - sommeVersementEnBaque;

                var infos = new InfosVersementBanque();
                infos.TotalEnBanque = sommeVersementEnBaque;
                infos.TotalEnCaisse = totalEnCaisse;

                var listVersement = DataManager.GetAll<VersementBanque>("DateCreation", "DESC");
                foreach (var versement in listVersement)
                {
                    //Calculer la transaction
                    var totalAncienSommeSessionCaisse = DataManager.GetSessionCaisseAvantDateVersementBanque(versement.DateCreation);
                    var totalAncienVersement = DataManager.GetSumVersementEnBanqueAvantVersementBaque(versement.DateCreation);

                    versement.MontantInitial = totalAncienSommeSessionCaisse - totalAncienVersement;
                    versement.MontantFinal = versement.MontantInitial - versement.Versement;
                }
                infos.ListVersement = listVersement;
                responseData = ResponseData.GetSuccess(infos);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/banques/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.Get<Block>(id));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/banques
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var _data = data.ToObject<VersementBanque>();
                _data.DateCreation = DateTime.UtcNow;
                DataManager.Save(_data);
                responseData = ResponseData.GetSuccess(_data);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }
    }

    class InfosVersementBanque
    {
        public List<VersementBanque> ListVersement { get; set; }
        public decimal TotalEnCaisse { get; set; }
        public decimal TotalEnBanque { get; set; }
    }
}
