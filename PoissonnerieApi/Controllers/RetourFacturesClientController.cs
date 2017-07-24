using BusinessEngine.DataModel;
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
    public class RetourFacturesClientController : ApiController
    {
        // GET api/stocks
        public object GetRetourFacturesClientProduits(long facture)
        {
            try
            {
                var detailsFacture = DataManager.GetList<DetailsFacturesClient>("FacturesClient", facture);
                var listRetourFacture = new List<DetailsRetourFacturesClient>();
                foreach (var details in detailsFacture)
                {
                    var detailsRetourFactureClient = new DetailsRetourFacturesClient();
                    detailsRetourFactureClient.DateCreation = details.DateCreation;
                    detailsRetourFactureClient.Produit = details.Produit;
                    detailsRetourFactureClient.Quantite = 0;
                    detailsRetourFactureClient.QuantiteDisponible = details.Quantite;
                    detailsRetourFactureClient.PrixUnitaire = details.PrixUnitaire;
                    detailsRetourFactureClient.Tva = details.Tva;
                    detailsRetourFactureClient.TypeColisage = details.TypeColisage;

                    listRetourFacture.Add(detailsRetourFactureClient);
                }

                return ResponseData.GetSuccess(listRetourFacture);
            }
            catch (Exception ex)
            {
                return ResponseData.GetError(ex.Message);
            }
        }

        public object Post([FromBody] JToken data)
        {
            try
            {
                var retourFacturesClientData = data.ToObject<RetourFacturesClientData>();

                var _user = DataManager.Get<User>(retourFacturesClientData.UserId);
                if (_user == null)
                {
                    return ResponseData.GetError("Utilisateur invalide !");
                }

                var _facture = DataManager.Get<FacturesClient>(retourFacturesClientData.FactureClientId);
                if (_facture == null)
                {
                    return ResponseData.GetError("Facture client introuvable !");
                }

                var details = new List<DetailsRetourFacturesClient>();

                foreach (var retourClient in retourFacturesClientData.DetailsRetourFacturesClient)
                {
                    if (retourClient.Quantite == 0)
                        continue;

                    retourClient.DateCreation = DateTime.UtcNow;
                    details.Add(retourClient);
                }

                if (!details.Any())
                {
                    return ResponseData.GetError("Aucun produit a retourner");
                }

                var retour = new RetourFacturesClient();
                retour.Commentaire = retourFacturesClientData.Commentaire;
                retour.DateCreation = DateTime.UtcNow;
                retour.User = _user;
                retour.FacturesClient = _facture;
                DataManager.Save(retour);

                foreach (var d in details)
                {
                    d.RetourFacturesClient = retour;
                    DataManager.Save(d);
                }

                //Entrer en stock des produits retournés
                var fournisseur = DataManager.Get<Fournisseur>("RaisonSociale", Constants.FOURNISSEUR_RETOUR_FACTURE_CLIENT);
                if (fournisseur == null)
                {
                    fournisseur = new Fournisseur();
                    fournisseur.RaisonSociale = Constants.FOURNISSEUR_RETOUR_FACTURE_CLIENT;
                    fournisseur.DateCreation = DateTime.UtcNow;
                    fournisseur.DateModification = fournisseur.DateCreation;

                    DataManager.Save(fournisseur);
                }

                var br = new BonReceptionFournisseur();
                br.Commentaire = retour.Commentaire;
                br.DateCreation = DateTime.UtcNow;
                br.DateModification = br.DateCreation;
                br.DateValidation = br.DateCreation;
                br.Fournisseur = fournisseur;
                br.MarquerRecu = true;
                br.RetourFacturesClient = retour;
                DataManager.Save(br);

                var listDetailsBr = new List<DetailsBonReceptionFournisseur>();
                foreach (var l in details)
                {
                    var detailsBr = new DetailsBonReceptionFournisseur();
                    detailsBr.BonReceptionFournisseur = br;
                    detailsBr.DateCreation = DateTime.UtcNow;
                    detailsBr.PrixUnitaire = l.PrixUnitaire;
                    detailsBr.Quantite = l.Quantite;
                    detailsBr.TypeColisage = l.TypeColisage;
                    detailsBr.Tva = l.Tva;
                    detailsBr.Produit = l.Produit;

                    DataManager.Save(detailsBr);
                }
                
                return ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                return ResponseData.GetError(ex.Message);
            }
        }
    }

    public class RetourFacturesClientData
    {
        public List<DetailsRetourFacturesClient> DetailsRetourFacturesClient { set; get; }
        public string Commentaire { set; get; }
        public long UserId { set; get; }
        public long FactureClientId { set; get; }
    }
}
