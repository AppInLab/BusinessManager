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
    public class JournalController : ApiController
    {
        // GET api/journal/5
        public object Get(int sessionCaisseUser)
        {
            ResponseData responseData;
            try
            {
                var sessionsCaisse = DataManager.GetSessionCaisseUserOuvertes(sessionCaisseUser);
                if (sessionsCaisse == null)
                {
                    sessionsCaisse = DataManager.GetLastSessionCaisseUser(sessionCaisseUser);
                    //return responseData = ResponseData.GetError("Session introuvable !");
                }

                var listJournal = new List<Journal>();

                //Recuperation des ventes
                var ventes = DataManager.GetList<FacturesClient>("SessionCaisse", sessionsCaisse.Id);
                foreach (var v in ventes)
                {
                    var journal = new Journal();
                    journal.Date = v.DateCreation;

                    //var client = "un INCONNU";
                    //if (v.Client != null)
                    //    client = v.Client.NomComplet;

                    journal.Montant = DataManager.GetSumFactures(v.Id);
                    journal.Operation = "Vente";
                    journal.Owner = v.Client;

                    listJournal.Add(journal);
                }
                //Recuperation des sorties de caisses
                var sortieDeCaisses = DataManager.GetList<SortieDeCaisse>("SessionCaisse", sessionsCaisse.Id);
                //Recuperation des versements
                var paiements = DataManager.GetList<Paiement>("SessionCaisse", sessionsCaisse.Id);
                foreach (var p in paiements)
                {
                    var journal = new Journal();
                    journal.Date = p.DateCreation;

                    //var client = "INCONNU";
                    //if (p.Client != null)
                    //    client = p.Client.NomComplet;

                    journal.Operation = "Versement";
                    journal.Montant = p.Versement;
                    journal.Owner = p.Client;

                    listJournal.Add(journal);
                }

                listJournal.OrderByDescending(x => x.Date);
                
                responseData = ResponseData.GetSuccess(listJournal);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }
    }
}
