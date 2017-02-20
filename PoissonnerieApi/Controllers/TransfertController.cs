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
    public class TransfertController : ApiController
    {
        // GET api/transfert
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                var commandesFournisseur = DataManager.GetAll<Transfert>("DateCreation", "DESC");
                responseData = ResponseData.GetSuccess(commandesFournisseur);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/transfert/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                var transfert = DataManager.Get<Transfert>(id);
                var details = DataManager.GetList<DetailsTransfert>("Transfert", transfert.Id);
                var panier = new List<PanierItem>();

                foreach (var d in details)
                {
                    var item = new PanierItem();
                    item.Produit = d.Produit;

                    if (d.TypeColisage == Constants.TYPE_BLOCK_KEY)
                        item.QuantiteBlock = d.Quantite;
                    else if (d.TypeColisage == Constants.TYPE_UNITE_KEY)
                        item.QuantiteUnitaire = d.Quantite;

                    item.TypeVenteId = d.TypeColisage;//Unite ou Carton

                    panier.Add(item);
                }

                transfert.Panier = panier;
                responseData = ResponseData.GetSuccess(transfert);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/transfert?marquerCommeTransfere=5
        public object GetMarquerCommeTransfere([FromUri]int marquerCommeTransfere, [FromUri]string date)
        {
            ResponseData responseData;
            try
            {
                //Recuperation de la commande
                var transfert = DataManager.Get<Transfert>(marquerCommeTransfere);
                transfert.MarquerTransfere = true;

                if (string.IsNullOrWhiteSpace(date) || date == "undefined")
                {
                    transfert.DateTransfertEffectue = DateTime.UtcNow;
                }
                else
                {
                    var stringToDate = Helper.parseDateWithFrenchCulture(date);
                    if (DateTime.UtcNow.Date == stringToDate.Date)
                    {
                        transfert.DateTransfertEffectue = DateTime.UtcNow;
                    }
                    else
                    {
                        transfert.DateTransfertEffectue = Helper.parseDateWithFrenchCulture(date);
                    }
                }
                
                DataManager.Save(transfert);

                responseData = ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/transfert
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var dateCreationString = data["DateCreation"];
                Helper.RemoveFields(data, new[] { "DateCreation" });//Supprimer DateCreation du token

                var transfert = data.ToObject<Transfert>();

                if (dateCreationString == null || string.IsNullOrWhiteSpace(dateCreationString.ToString()))
                {
                    if (transfert.Id > 0)
                    {//Modification du transfert
                        return ResponseData.GetError("Date transfert invalide !");
                    }

                    transfert.DateCreation = DateTime.UtcNow;
                }
                else
                {
                    transfert.DateCreation = Helper.parseDateWithFrenchCulture(dateCreationString.ToString());
                    if (transfert.DateCreation.Date == DateTime.UtcNow.Date)
                    {
                        transfert.DateCreation = DateTime.UtcNow;
                    }
                }

                if (transfert.Id > 0)//Mise à jour du bon de reception
                {
                    DataManager.Save(transfert);//Enregistrement des modifications

                    //Suppression des details
                    var detailsAsupprimer = DataManager.GetList<DetailsTransfert>("Transfert", transfert.Id);
                    foreach (var d in detailsAsupprimer)
                    {
                        DataManager.Delete(d);
                    }

                    //Ajout des nouveaux éléments
                    SaveTransfertElements(transfert);
                }
                else//Nouvelle commande
                {
                    //Enregistrer le bon
                    DataManager.Save(transfert);
                    //Enregistrer les details du bon
                    SaveTransfertElements(transfert);
                }

                responseData = ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        private void SaveTransfertElements(Transfert transfert)
        {
            //Enregistrer les details de la commande
            foreach (var panierItem in transfert.Panier)
            {
                var details = new DetailsTransfert();
                details.Transfert = transfert;
                details.Produit = panierItem.Produit;
                details.DateCreation = transfert.DateCreation;

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
                details.TypeColisage = panierItem.TypeVenteId;

                DataManager.Save(details);
            }
        }
    }
}
