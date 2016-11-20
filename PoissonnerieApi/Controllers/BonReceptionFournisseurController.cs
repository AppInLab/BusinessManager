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
    public class BonReceptionFournisseurController : ApiController
    {
        // GET api/bonreceptionfournisseur
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                var commandesFournisseur = DataManager.GetAll<BonReceptionFournisseur>();

                foreach (var cmd in commandesFournisseur)
                {
                    var details = DataManager.GetList<DetailsBonReceptionFournisseur>("BonReceptionFournisseur.Id", cmd.Id);
                    foreach (var elem in details)
                    {
                        var montantHt = elem.PrixUnitaire * elem.Quantite;
                        var montantTva = (elem.Tva / 100) * montantHt;
                        var montantTtc = montantHt + montantTva;

                        cmd.TotalHt += montantHt;
                        cmd.TotalTva += montantTva;
                        cmd.TotalTtc += montantTtc;
                    }
                }

                responseData = ResponseData.GetSuccess(commandesFournisseur);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/commandefournisseur/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                var bonReceptionFournisseur = DataManager.Get<BonReceptionFournisseur>(id);
                var details = DataManager.GetList<DetailsBonReceptionFournisseur>("BonReceptionFournisseur.Id", bonReceptionFournisseur.Id);
                var panier = new List<PanierItem>();

                foreach (var d in details)
                {
                    var item = new PanierItem();
                    item.Produit = d.Produit;
                    item.PrixAchat = d.PrixUnitaire;

                    if (d.TypeColisage == Constants.TYPE_BLOCK_KEY)
                        item.QuantiteBlock = d.Quantite;
                    else if (d.TypeColisage == Constants.TYPE_UNITE_KEY)
                        item.QuantiteUnitaire = d.Quantite;

                    item.TotalBlock = item.QuantiteBlock * item.PrixAchat;
                    item.TotalUnitaire = item.QuantiteUnitaire * item.PrixAchat;

                    item.TypeVenteId = d.TypeColisage;//Unite ou Carton
                    item.Tva = d.Tva;
                    item.Tht = d.PrixUnitaire * d.Quantite;
                    item.MontantTva = (item.Tva / 100) * item.Tht;
                    item.Ttc = item.Tht + item.MontantTva;

                    bonReceptionFournisseur.TotalHt += item.Tht;
                    bonReceptionFournisseur.TotalTva += item.MontantTva;
                    bonReceptionFournisseur.TotalTtc += item.Ttc;

                    panier.Add(item);
                }

                bonReceptionFournisseur.Panier = panier;
                responseData = ResponseData.GetSuccess(bonReceptionFournisseur);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/bonreceptionfournisseur?marquerCommeRecu=5
        public object GetMarquerCommeRecu(int marquerCommeRecu)
        {
            ResponseData responseData;
            try
            {
                //Recuperation de la commande
                var bonReceptionFournisseur = DataManager.Get<BonReceptionFournisseur>(marquerCommeRecu);
                bonReceptionFournisseur.MarquerRecu = true;
                DataManager.Save(bonReceptionFournisseur);

                responseData = ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/bonreceptionfournisseur
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var bonReceptionFournisseur = data.ToObject<BonReceptionFournisseur>();
                if (bonReceptionFournisseur.Id > 0)//Mise à jour du bon de reception
                {
                    bonReceptionFournisseur.DateModification = DateTime.UtcNow;
                    DataManager.Save(bonReceptionFournisseur);//Enregistrement des modifications

                    //Suppression des details
                    var detailsAsupprimer = DataManager.GetList<DetailsBonReceptionFournisseur>("BonReceptionFournisseur.Id", bonReceptionFournisseur.Id);
                    foreach (var d in detailsAsupprimer)
                    {
                        DataManager.Delete(d);
                    }

                    //Ajout des nouveaux éléments
                    SaveCommandeElements(bonReceptionFournisseur);
                }
                else//Nouvelle commande
                {
                    bonReceptionFournisseur.DateCreation = DateTime.UtcNow;
                    bonReceptionFournisseur.DateModification = bonReceptionFournisseur.DateCreation;
                    //Enregistrer le bon
                    DataManager.Save(bonReceptionFournisseur);
                    //Enregistrer les details du bon
                    SaveCommandeElements(bonReceptionFournisseur);
                }

                responseData = ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        private void SaveCommandeElements(BonReceptionFournisseur bonReceptionFournisseur)
        {
            //Enregistrer les details de la commande
            foreach (var panierItem in bonReceptionFournisseur.Panier)
            {
                var details = new DetailsBonReceptionFournisseur();
                details.BonReceptionFournisseur = bonReceptionFournisseur;
                details.Produit = panierItem.Produit;
                details.DateCreation = DateTime.UtcNow;

                //Recuperer la quantité
                if (panierItem.TypeVenteId == Constants.TYPE_BLOCK_KEY)
                {
                    details.Quantite = panierItem.QuantiteBlock;
                }
                else if (panierItem.TypeVenteId == Constants.TYPE_UNITE_KEY)
                {
                    details.Quantite = panierItem.QuantiteUnitaire;
                }
                //Etant donnée que c'est un bon de reception, on utilise le prix d'achat
                details.PrixUnitaire = panierItem.PrixAchat;
                details.Tva = panierItem.Tva;
                details.TypeColisage = panierItem.TypeVenteId;

                DataManager.Save(details);
            }
        }
    }
}
