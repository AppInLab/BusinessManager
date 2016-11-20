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

        // GET api/facturesclient/5
        //public object Get(int id)
        //{
        //    ResponseData responseData;
        //    try
        //    {
        //        var bonReceptionFournisseur = DataManager.Get<BonReceptionFournisseur>(id);
        //        var details = DataManager.GetList<DetailsBonReceptionFournisseur>("BonReceptionFournisseur.Id", bonReceptionFournisseur.Id);
        //        var panier = new List<PanierItem>();

        //        foreach (var d in details)
        //        {
        //            var item = new PanierItem();
        //            item.Produit = d.Produit;
        //            item.PrixAchat = d.PrixUnitaire;

        //            if (d.TypeColisage == Constants.TYPE_BLOCK_KEY)
        //                item.QuantiteBlock = d.Quantite;
        //            else if (d.TypeColisage == Constants.TYPE_UNITE_KEY)
        //                item.QuantiteUnitaire = d.Quantite;

        //            item.TotalBlock = item.QuantiteBlock * item.PrixAchat;
        //            item.TotalUnitaire = item.QuantiteUnitaire * item.PrixAchat;

        //            item.TypeVenteId = d.TypeColisage;//Unite ou Carton
        //            item.Tva = d.Tva;
        //            item.Tht = d.PrixUnitaire * d.Quantite;
        //            item.MontantTva = (item.Tva / 100) * item.Tht;
        //            item.Ttc = item.Tht + item.MontantTva;

        //            bonReceptionFournisseur.TotalHt += item.Tht;
        //            bonReceptionFournisseur.TotalTva += item.MontantTva;
        //            bonReceptionFournisseur.TotalTtc += item.Ttc;

        //            panier.Add(item);
        //        }

        //        bonReceptionFournisseur.Panier = panier;
        //        responseData = ResponseData.GetSuccess(bonReceptionFournisseur);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData = ResponseData.GetError(ex.Message);
        //    }

        //    return responseData;
        //}

        //// GET api/facturesclient?marquerCommeRecu=5
        //public object GetMarquerCommeRecu(int marquerCommeRecu)
        //{
        //    ResponseData responseData;
        //    try
        //    {
        //        //Recuperation de la commande
        //        var bonReceptionFournisseur = DataManager.Get<BonReceptionFournisseur>(marquerCommeRecu);
        //        bonReceptionFournisseur.MarquerRecu = true;
        //        DataManager.Save(bonReceptionFournisseur);

        //        responseData = ResponseData.GetSuccess("OK");
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData = ResponseData.GetError(ex.Message);
        //    }

        //    return responseData;
        //}

        // POST api/facturesclient
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var facturesClient = data.ToObject<FacturesClient>();
                if (facturesClient.Id <= 0)//Ajout de la facture
                {
                    facturesClient.DateCreation = DateTime.UtcNow;
                    facturesClient.DateModification = facturesClient.DateCreation;
                    //Enregistrer de la facture
                    DataManager.Save(facturesClient);
                    //Enregistrer les details de la facture
                    SaveElements(facturesClient);
                }

                responseData = ResponseData.GetSuccess(facturesClient);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
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
