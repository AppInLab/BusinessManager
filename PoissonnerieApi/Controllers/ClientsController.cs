﻿using BusinessEngine.Manager;
using BusinessEngine.Models;
using Newtonsoft.Json.Linq;
using PoissonnerieApi.Models;
using PoissonnerieApi.Utils;
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
                var clients = DataManager.GetAll<Client>();
                foreach (var c in clients)
                {
                    var soldeInitial = DataManager.GetVersementInitial(c.Id);
                    if (soldeInitial == null)
                    {
                        c.SoldeInitial = 0;
                        continue;
                    }

                    c.SoldeInitial = -soldeInitial.Versement;
                }

                return ResponseData.GetSuccess(clients);
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
                return ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        public object GetSoldeAnterieurClient(int client, string dateAnterieur)
        {
            ResponseData responseData;
            try
            {
                var stringToDate = Helper.parseDateWithFrenchCulture(dateAnterieur);
                stringToDate.AddDays(1);//Prendre en compte la date d'aujoud'hui

                var _client = DataManager.Get<Client>(client);

                //Recuperer tous les achats du client
                var sommeFacturesClient = DataManager.GetSumFacturesParClients(_client.Id, stringToDate);

                //Recuperer tous les retours du client
                var sommeRetourFacturesClient = DataManager.GetSumRetourFacturesParClients(_client.Id, stringToDate);

                //Recuperer tous les versements du client
                var sommeVersementClient = DataManager.GetSumPaiementParClient(_client.Id, stringToDate);

                //Faire la difference pour obetenir la somme dû
                var sommeDue = sommeFacturesClient - sommeVersementClient - sommeRetourFacturesClient;

                responseData = ResponseData.GetSuccess(sommeDue);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/clients/5
        public object GetFicheClient(int client, bool anterieure = false)
        {
            ResponseData responseData;
            try
            {
                var _client = DataManager.Get<Client>(client);

                var ficheClient = new FicheClient();
                ficheClient.Client = _client;
                //Recuperer tous les achats du client
                var sommeFacturesClient = DataManager.GetSumFacturesParClients(_client.Id);

                //Recuperer tous les retours du client
                var sommeRetourFacturesClient = DataManager.GetSumRetourFacturesParClients(_client.Id);
                
                //Recuperer tous les versements du client
                var sommeVersementClient = DataManager.GetSumPaiementParClient(_client.Id);
                
                //Faire la difference pour obetenir la somme dû
                ficheClient.SommeDue = sommeFacturesClient - sommeVersementClient - sommeRetourFacturesClient;

                //Recuperer la liste des 100 derniers versements
                int max = 0;//Toute la liste
                if (!anterieure)
                    max = 100;

                ficheClient.Paiements = DataManager.GetListPaiementsClientSansSoldeInitial(_client.Id, max);
                foreach (var p in ficheClient.Paiements)
                    p.Monnaie = p.MontantPercu - p.Versement;

                ficheClient.Factures = DataManager.GetListFacturesClient(_client.Id, max);
                foreach (var facture in ficheClient.Factures)
                {
                    facture.TotalTtc = DataManager.GetSumFactures(facture.Id);
                }

                ficheClient.RetourFactures = DataManager.GetListRetourFacturesClient(_client.Id);
                foreach (var retour in ficheClient.RetourFactures)
                {
                    retour.TotalTtc = DataManager.GetSumRetourFactures(retour.Id);
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

                    //Recherche le solde initial
                    var ancienSolde = DataManager.GetVersementInitial(_data.Id);
                    if (ancienSolde == null)
                    {
                        var paiement = new Paiement();
                        paiement.Client = _data;
                        paiement.IsSoldeInitial = true;
                        paiement.Versement = _data.SoldeInitial * (-1);
                        paiement.DateCreation = DateTime.UtcNow;

                        DataManager.Save(paiement);
                    }
                    else
                    {
                        if (ancienSolde.Versement != -_data.SoldeInitial)
                        {
                            ancienSolde.Versement = _data.SoldeInitial * (-1);
                            DataManager.Save(ancienSolde);
                        }
                    }                    
                }else{
                    _data.DateCreation = DateTime.UtcNow;
                    _data.DateModification = DateTime.UtcNow;

                    if(_data.SoldeInitial != 0){
                        //Si le client doit 1000; On fais un versement de -1000
                        //Si on doit au client, (-1000) on fait un versement positive (1000)
                        _data.SoldeInitial = _data.SoldeInitial * (-1);
                        isSoldeInitial = true;
                    }

                    //Si c'est le solde initial, on effectue un paiement inverse
                    if (isSoldeInitial)
                    {
                        var paiement = new Paiement();
                        paiement.Client = _data;
                        paiement.IsSoldeInitial = true;
                        paiement.Versement = _data.SoldeInitial;
                        paiement.DateCreation = DateTime.UtcNow;

                        DataManager.Save(paiement);
                    }
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
    }
}
