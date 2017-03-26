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
        static private string OP_FACTURE = "OP_FACTURE";
        static private string OP_VERSEMENT = "OP_VERSEMENT";

        // GET api/journal/5
        public object Get(int user)
        {
            try
            {
                var sessionsCaisse = DataManager.GetSessionCaisseUserOuvertes(user);
                if (sessionsCaisse == null)
                {
                    sessionsCaisse = DataManager.GetLastSessionCaisseUser(user);
                }

                //Factures client avec versement
                //Factures client sans versement
                //Vente cash
                //Versement client sans facture

                var listJournal = new List<Journal>();
                //Recuperation des ventes
                var ventes = DataManager.GetList<FacturesClient>("SessionCaisse", sessionsCaisse.Id);
                foreach (var v in ventes)
                {
                    var journal = new Journal();
                    journal.Date = v.DateCreation;
                    journal.Montant = DataManager.GetSumFactures(v.Id);
                    journal.Owner = v.Client;
                    journal.IdOperation = v.Id;

                    var versement = DataManager.Get<Paiement>("FacturesClient", v.Id);
                    if (versement == null)//Facture sans versement
                    {
                        journal.Operation = "Facture client SANS versement";
                    }
                    else
                    {
                        if (v.Client == null)
                        {
                            journal.Operation = "Vente cash";
                        }
                        else
                        {
                            journal.MontantFactureAvecVersement = versement.Versement;
                            journal.Operation = "Facture client AVEC versement";
                        }
                    }
                    
                    listJournal.Add(journal);
                }
                //Recuperation des sorties de caisses
                //var sortieDeCaisses = DataManager.GetList<SortieDeCaisse>("SessionCaisse", sessionsCaisse.Id);
                
                //Recuperation des versements
                var paiements = DataManager.GetListVersementsClientParSessionCaisse(sessionsCaisse.Id);
                foreach (var p in paiements)
                {
                    var journal = new Journal();
                    journal.Date = p.DateCreation;
                    journal.Operation = "Versement";
                    journal.Montant = p.Versement;
                    journal.Owner = p.Client;

                    listJournal.Add(journal);
                }

                return ResponseData.GetSuccess(listJournal.OrderByDescending(x => x.Date));
            }
            catch (Exception ex)
            {
                return ResponseData.GetError(ex.Message);
            }
        }

        public object GetParSessionCaisse(int sessionCaisse)
        {
            try
            {
                var sessionsCaisse = DataManager.Get<SessionCaisse>(sessionCaisse);
                if (sessionsCaisse == null)
                {
                    return ResponseData.GetError("Session introuvable !");
                }

                //Factures client avec versement
                //Factures client sans versement
                //Vente cash
                //Versement client sans facture

                var listJournal = new List<Journal>();
                //Recuperation des ventes
                var ventes = DataManager.GetList<FacturesClient>("SessionCaisse", sessionsCaisse.Id);
                foreach (var v in ventes)
                {
                    var journal = new Journal();
                    journal.Date = v.DateCreation;
                    journal.Montant = DataManager.GetSumFactures(v.Id);
                    journal.Owner = v.Client;
                    journal.IdOperation = v.Id;
                    journal.OpCode = OP_FACTURE;

                    var versement = DataManager.Get<Paiement>("FacturesClient", v.Id);
                    if (versement == null)//Facture sans versement
                    {
                        journal.Operation = "Facture client SANS versement";
                    }
                    else
                    {
                        if (v.Client == null)
                        {
                            journal.Operation = "Vente cash";
                        }
                        else
                        {
                            journal.MontantFactureAvecVersement = versement.Versement;
                            journal.Operation = "Facture client AVEC versement";
                        }
                    }

                    listJournal.Add(journal);
                }
                //Recuperation des sorties de caisses
                //var sortieDeCaisses = DataManager.GetList<SortieDeCaisse>("SessionCaisse", sessionsCaisse.Id);

                //Recuperation des versements
                var paiements = DataManager.GetListVersementsClientParSessionCaisse(sessionsCaisse.Id);
                foreach (var p in paiements)
                {
                    var journal = new Journal();
                    journal.Date = p.DateCreation;
                    journal.Operation = "Versement";
                    journal.Montant = p.Versement;
                    journal.Owner = p.Client;
                    journal.OpCode = OP_VERSEMENT;
                    journal.IdOperation = p.Id;

                    listJournal.Add(journal);
                }

                return ResponseData.GetSuccess(listJournal.OrderByDescending(x => x.Date));
            }
            catch (Exception ex)
            {
                return ResponseData.GetError(ex.Message);
            }
        }
    }
}
