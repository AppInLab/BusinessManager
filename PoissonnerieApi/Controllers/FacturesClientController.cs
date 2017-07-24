using BusinessEngine.DataModel;
using BusinessEngine.DataModels;
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
    public class FacturesClientController : ApiController
    {
        // GET api/facturesclient
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                var facturesClient = DataManager.GetAll<FacturesClient>();

                foreach (var facture in facturesClient)
                {
                    var details = DataManager.GetList<DetailsFacturesClient>("FacturesClient.Id", facture.Id);
                    foreach (var elem in details)
                    {
                        var montantHt = elem.PrixUnitaire * elem.Quantite;
                        var montantTva = (elem.Tva / 100) * montantHt;
                        var montantTtc = montantHt + montantTva;

                        facture.TotalHt += montantHt;
                        facture.TotalTva += montantTva;
                        facture.TotalTtc += montantTtc;
                    }
                }

                responseData = ResponseData.GetSuccess(facturesClient);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        [HttpGet]
        public object SupprimerData(long del)
        {
            try
            {
                var data = DataManager.Get<FacturesClient>(del);
                if (data == null)
                {
                    return ResponseData.GetError("L'information que vous essayez de supprimer l'existe pas.");
                }

                //On recupere tous les versments effectué sur cette facture
                var paiements = DataManager.GetList<Paiement>("FacturesClient", data.Id);
                foreach (var p in paiements)
                {
                    DataManager.Delete(p);
                }

                //On recupere tous les details de la facture
                var detailsFacturesClient = DataManager.GetList<DetailsFacturesClient>("FacturesClient", data.Id);
                foreach (var d in detailsFacturesClient)
                {
                    DataManager.Delete(d);
                }

                DataManager.Delete(data);
                //Recuperer la session en cours
                return ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                return ResponseData.GetError(ex.Message);
            }
        }

        // GET api/facturesclient
        public object GetDetailsFacture(long details)
        {
            ResponseData responseData;
            try
            {
                var detailsFacture = DataManager.GetList<DetailsFacturesClient>("FacturesClient", details);
                foreach (var facture in detailsFacture)
                {
                    var montantHt = facture.PrixUnitaire * facture.Quantite;
                    var montantTva = (facture.Tva / 100) * montantHt;
                    var montantTtc = montantHt + montantTva;

                    //facture.TotalHt += montantHt;
                    //facture.TotalTva += montantTva;
                    facture.MontantTtc = montantTtc;
                }

                responseData = ResponseData.GetSuccess(detailsFacture);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/facturesclient
        public object GetDetailsRetourFacture(long detailsRetourFacture)
        {
            ResponseData responseData;
            try
            {
                var listDetailsRetourFacture = DataManager.GetList<DetailsRetourFacturesClient>("RetourFacturesClient", detailsRetourFacture);
                string commentaire = "";
                foreach (var facture in listDetailsRetourFacture)
                {
                    var montantHt = facture.PrixUnitaire * facture.Quantite;
                    var montantTva = (facture.Tva / 100) * montantHt;
                    var montantTtc = montantHt + montantTva;

                    //facture.TotalHt += montantHt;
                    //facture.TotalTva += montantTva;
                    facture.MontantTtc = montantTtc;

                    if (facture.RetourFacturesClient != null)
                    {
                        commentaire = facture.RetourFacturesClient.Commentaire;
                    }
                }

                var data = new Dictionary<string, object>();
                data["List"] = listDetailsRetourFacture;
                data["Commentaire"] = commentaire;

                responseData = ResponseData.GetSuccess(data);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var facturesClient = data.ToObject<FacturesClient>();
                if (facturesClient.Id <= 0)//Nouvelle facture
                {
                    //Recuperer la caisse ouvert du jour
                    var caisseDuJour = DataManager.GetCaisseOuverte();

                    if (caisseDuJour != null)
                    {
                        //Si ok, on continue
                        //Si nok, on verifie que la caisse j-n est fermé.
                        //Si oui, on ouvre une nouvelle caisse
                        //Si non, on informe qu'ils doivent fermer la session precedente
                        if (caisseDuJour.DateOuverture.Date < DateTime.UtcNow.Date)
                        {
                            return ResponseData.GetError("Vous ne pouvez pas effectuer cette opération car la caisse du " + caisseDuJour.DateOuverture.Date.ToString("dd/MM/yyy")
                                + " n'est pas encore cloturée");
                        }

                        if (caisseDuJour.DateOuverture.Date > DateTime.UtcNow.Date)
                        {
                            return ResponseData.GetError("La date d'ouverture de la caisse actuelle est superieure à la date système. Merci de contacter le developpeur.");
                        }
                    }
                    else
                    {

                        caisseDuJour = DataManager.GetCaisseOuverte(DateTime.UtcNow.Date);
                        if (caisseDuJour != null)
                        {
                            return ResponseData.GetError("Impossible d'ouvrir 2 caisses le même jour.");
                        }

                        caisseDuJour = new Caisse();
                        caisseDuJour.DateOuverture = DateTime.UtcNow;
                        //TODO: Code à definir
                        //caisseDuJour.Code
                        DataManager.Save(caisseDuJour);
                    }

                    //Recuperer la session de la caisse de l'utilisateur
                    var sessionCaisseUser = DataManager.GetSessionCaisseUser(caisseDuJour.Id, facturesClient.User.Id);
                    if (sessionCaisseUser == null)
                    {
                        //Si la session n'existe pas, Ouverture de caisse
                        sessionCaisseUser = new SessionCaisse();
                        sessionCaisseUser.FondDeCaisse = facturesClient.User.FondDeCaisse;
                        sessionCaisseUser.DateOuverture = DateTime.UtcNow;
                        sessionCaisseUser.Caisse = caisseDuJour;
                        sessionCaisseUser.User = facturesClient.User;
                        sessionCaisseUser.IsClosed = false;
                        //TODO:A definir
                        //sessionCaisseUser.Code

                        DataManager.Save(sessionCaisseUser);
                    }
                    else
                    {
                        if (sessionCaisseUser.IsClosed)
                        {
                            return 
                                ResponseData.GetError("Opération refusée ! Votre session de caisse est clôturée.");
                        }
                    }


                    //On ajoute la session à la facture
                    facturesClient.SessionCaisse = sessionCaisseUser;
                    facturesClient.DateCreation = DateTime.UtcNow;
                    facturesClient.DateModification = facturesClient.DateCreation;
                    //Enregistrer de la facture
                    DataManager.Save(facturesClient);
                    //Enregistrer les details de la facture
                    SaveElements(facturesClient);

                    //Enregistrer le paiement
                    if (facturesClient.MontantPercu > 0)//Versement
                    {
                        //On calculer la facture (Montant HT, TVA, TTC)
                        CalculerDetailsFacture(facturesClient);

                        var paiement = new Paiement();
                        paiement.DateCreation = DateTime.UtcNow;
                        paiement.Client = facturesClient.Client;
                        paiement.SessionCaisse = sessionCaisseUser;
                        paiement.NetAPayer = facturesClient.TotalTtc;
                        paiement.MontantPercu = facturesClient.MontantPercu;

                        //if (facturesClient.MontantPercu > facturesClient.TotalTtc)
                        //{
                        //    paiement.Versement = facturesClient.TotalTtc;
                        //}
                        //else
                        //{
                        //    paiement.Versement = facturesClient.MontantPercu;
                        //}

                        paiement.Versement = facturesClient.MontantPercu;
                        paiement.FacturesClient = facturesClient;
                        DataManager.Save(paiement);
                    }

                    if (facturesClient.Client != null)
                    {
                        //Recuperer tous les achats du client
                        var sommeFacturesClient = DataManager.GetSumFacturesParClients(facturesClient.Client.Id);
                        //Recuperer tous les versements du client
                        var sommeVersementClient = DataManager.GetSumPaiementParClient(facturesClient.Client.Id);
                        //Faire la difference pour obetenir la somme dû
                        facturesClient.SommeDue = sommeFacturesClient - sommeVersementClient;
                    }
                }

                responseData = ResponseData.GetSuccess(facturesClient);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        private void CalculerDetailsFacture(FacturesClient facturesClient)
        {
            var details = DataManager.GetList<DetailsFacturesClient>("FacturesClient.Id", facturesClient.Id);
            foreach (var elem in details)
            {
                var montantHt = elem.PrixUnitaire * elem.Quantite;
                var montantTva = (elem.Tva / 100) * montantHt;
                var montantTtc = montantHt + montantTva;

                facturesClient.TotalHt += montantHt;
                facturesClient.TotalTva += montantTva;
                facturesClient.TotalTtc += montantTtc;
            }
        }

        private void SaveElements(FacturesClient facturesClient)
        {
            //Enregistrer les details de la commande
            foreach (var panierItem in facturesClient.Panier)
            {
                var details = new DetailsFacturesClient();
                details.FacturesClient = facturesClient;
                details.Produit = panierItem.Produit;
                details.DateCreation = DateTime.UtcNow;

                //Recuperer la quantité
                if (panierItem.TypeVenteId == Constants.TYPE_BLOCK_KEY)
                {
                    details.Quantite = panierItem.QuantiteBlock;
                    details.PrixUnitaire = panierItem.PrixBlock;
                }
                else if (panierItem.TypeVenteId == Constants.TYPE_UNITE_KEY)
                {
                    details.Quantite = panierItem.QuantiteUnitaire;
                    details.PrixUnitaire = panierItem.PrixUnitaire;
                }
                //Etant donnée que c'est une facture, on utilise le prix d'achat
                
                details.Tva = panierItem.Tva;
                details.TypeColisage = panierItem.TypeVenteId;

                DataManager.Save(details);
            }
        }
    }
}
