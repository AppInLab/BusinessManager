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
    public class ClientsController : ApiController
    {

        // GET api/clients
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.GetAll<Client>());
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/clients/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.Get<Client>(id));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/clients/5
        public object GetFicheClient(int client)
        {
            ResponseData responseData;
            try
            {
                var _client = DataManager.Get<Client>(client);

                var ficheClient = new FicheClient();
                ficheClient.Client = _client;
                //Recuperer tous les achats du client
                var sommeFacturesClient = DataManager.GetSumFacturesParClients(_client.Id);
                //Recuperer tous les versements du client
                var sommeVersementClient = DataManager.GetSumPaiementParCLient(_client.Id);
                //Faire la difference pour obetenir la somme dû
                ficheClient.SommeDue = sommeFacturesClient - sommeVersementClient;

                //Recuperer la liste des 100 derniers versements
                ficheClient.Paiements = DataManager.GetListPaiementsClientSansSoldeInitial(_client.Id);
                foreach (var p in ficheClient.Paiements)
                    p.Monnaie = p.MontantPercu - p.Versement;

                ficheClient.Factures = DataManager.GetListFacturesClient(_client.Id);
                foreach (var facture in ficheClient.Factures)
                {
                    facture.TotalTtc = DataManager.GetSumFactures(facture.Id);
                }

                responseData = ResponseData.GetSuccess(ficheClient);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/clients
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var _data = data.ToObject<Client>();
                var isSoldeInitial = false;
                if (_data.Id > 0)
                {
                    _data.DateModification = DateTime.UtcNow;
                }else{
                    _data.DateCreation = DateTime.UtcNow;
                    _data.DateModification = DateTime.UtcNow;

                    if(_data.SoldeInitial != 0){
                        //Si le client doit 1000; On fais un versement de -1000
                        //Si on doit au client, (-1000) on fait un versement positive (1000)
                        _data.SoldeInitial = _data.SoldeInitial * (-1);
                        isSoldeInitial = true;
                    }
                }

                DataManager.Save(_data);

                //Si c'est le solde initial, on effectue un paiement inverse
                if(isSoldeInitial){
                    var paiement = new Paiement();
                    paiement.Client = _data;
                    paiement.IsSoldeInitial = true;
                    paiement.Versement = _data.SoldeInitial;
                    paiement.DateCreation = DateTime.UtcNow;

                    DataManager.Save(paiement);
                }

                responseData = ResponseData.GetSuccess(_data);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }
    }
}
