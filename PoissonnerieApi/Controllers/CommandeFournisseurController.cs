using BusinessEngine.DataModel;
using BusinessEngine.DataModels;
using BusinessEngine.Manager;
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
    public class CommandeFournisseurController : ApiController
    {
        // GET api/commandefournisseur
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                var commandesFournisseur = DataManager.GetAll<CommandesFournisseur>();

                foreach (var cmd in commandesFournisseur)
                {
                    var details = DataManager.GetDetailsCommandesFournisseur(cmd.Id);
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
                var commandesFournisseur = DataManager.Get<CommandesFournisseur>(id);
                var details = DataManager.GetDetailsCommandesFournisseur(commandesFournisseur.Id);
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

                    commandesFournisseur.TotalHt += item.Tht;
                    commandesFournisseur.TotalTva += item.MontantTva;
                    commandesFournisseur.TotalTtc += item.Ttc;

                    panier.Add(item);
                }

                commandesFournisseur.Panier = panier;
                responseData = ResponseData.GetSuccess(commandesFournisseur);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/commandefournisseur?transfertBonReception=5
        public object GetTransfertToBonReception(int transfertBonReception)
        {
            ResponseData responseData;
            try
            {
                //Recuperation de la commande
                var commandesFournisseur = DataManager.Get<CommandesFournisseur>(transfertBonReception);
                
                //Creation du bon de reception
                var bonReception = new BonReceptionFournisseur();
                bonReception.CommandesFournisseur = commandesFournisseur;
                bonReception.Fournisseur = commandesFournisseur.Fournisseur;
                bonReception.DateCreation = DateTime.UtcNow;
                bonReception.DateModification = bonReception.DateCreation;
                bonReception.Commentaire = bonReception.Commentaire;
                //Enregistrement du bon de reception
                DataManager.Save(bonReception);

                //Recuperation des details de la commandes
                var detailsCommande = DataManager.GetDetailsCommandesFournisseur(commandesFournisseur.Id);
                //Copie des details de la commande dans les details du bon de reception
                foreach (var dc in detailsCommande)
                {
                    var detailsBonReception = new DetailsBonReceptionFournisseur();
                    detailsBonReception.BonReceptionFournisseur = bonReception;
                    detailsBonReception.DateCreation = DateTime.UtcNow;
                    detailsBonReception.Produit = dc.Produit;
                    detailsBonReception.PrixUnitaire = dc.PrixUnitaire;
                    detailsBonReception.Quantite = dc.Quantite;
                    detailsBonReception.Tva = dc.Tva;
                    detailsBonReception.TypeColisage = dc.TypeColisage;

                    DataManager.Save(detailsBonReception);
                }

                commandesFournisseur.EstLieABonReception = true;
                DataManager.Save(commandesFournisseur);
                responseData = ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/fournisseurs
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {

                var dateCreationString = data["DateCreation"];//.ToString();
                Helper.RemoveFields(data, new[] { "DateCreation" });//Retirer le champ

                var commandeFournisseur = data.ToObject<CommandesFournisseur>();

                if (dateCreationString == null || string.IsNullOrWhiteSpace(dateCreationString.ToString()))
                {
                    if (commandeFournisseur.Id > 0)
                    {//Modification de commande
                        return ResponseData.GetError("Date commande invalide !");
                    }

                    commandeFournisseur.DateCreation = DateTime.UtcNow;
                }
                else
                {
                    commandeFournisseur.DateCreation = Helper.parseDateWithFrenchCulture(dateCreationString.ToString());
                }
                commandeFournisseur.DateModification = DateTime.UtcNow;

                if (commandeFournisseur.Id > 0)//Mise à jour commande
                {
                    DataManager.Save(commandeFournisseur);//Enregistrement des modifications

                    //Suppression des details
                    var detailsAsupprimer = DataManager.GetDetailsCommandesFournisseur(commandeFournisseur.Id);
                    foreach (var d in detailsAsupprimer)
                    {
                        DataManager.Delete(d);
                    }

                    //Ajout des nouveaux éléments
                    SaveCommandeElements(commandeFournisseur);
                }
                else//Nouvelle commande
                {
                    //Enregistrer la commande
                    DataManager.Save(commandeFournisseur);

                    //Enregistrer les details de la commande
                    SaveCommandeElements(commandeFournisseur);
                }

                responseData = ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        private void SaveCommandeElements(CommandesFournisseur commandeFournisseur)
        {
            //Enregistrer les details de la commande
            foreach (var panierItem in commandeFournisseur.Panier)
            {
                var detailsCmdFournisseur = new DetailsCommandesFournisseur();
                detailsCmdFournisseur.CommandesFournisseur = commandeFournisseur;
                detailsCmdFournisseur.Produit = panierItem.Produit;
                detailsCmdFournisseur.DateCreation = commandeFournisseur.DateCreation;

                //Recuperer la quantité
                if (panierItem.TypeVenteId == Constants.TYPE_BLOCK_KEY)
                {
                    detailsCmdFournisseur.Quantite = panierItem.QuantiteBlock;
                }
                else if (panierItem.TypeVenteId == Constants.TYPE_UNITE_KEY)
                {
                    detailsCmdFournisseur.Quantite = panierItem.QuantiteUnitaire;
                }
                //Etant donnée que c'est une commande, on utilise le prix d'achat
                detailsCmdFournisseur.PrixUnitaire = panierItem.PrixAchat;
                detailsCmdFournisseur.Tva = panierItem.Tva;
                detailsCmdFournisseur.TypeColisage = panierItem.TypeVenteId;

                DataManager.Save(detailsCmdFournisseur);
            }

            //Si c'est lier à un bon de reception
            if (commandeFournisseur.EstLieABonReception)
            {
                GetTransfertToBonReception(commandeFournisseur.Id);
            }
        }
    }
}
