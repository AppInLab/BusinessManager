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
    public class PaiementsController : ApiController
    {
        // GET api/paiements
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                //Recuperer la session en cours
                responseData = ResponseData.GetSuccess(DataManager.GetAll<Paiement>("DateCreation", "DESC"));
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
                var paiement = DataManager.Get<Paiement>(del);
                if (paiement == null)
                {
                    return ResponseData.GetError("L'information que vous essayez de supprimer l'existe pas.");
                }

                DataManager.Delete(paiement);
                //Recuperer la session en cours
                return ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                return ResponseData.GetError(ex.Message);
            }

        }

        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var paiement = data.ToObject<Paiement>();
                
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
                var sessionCaisseUser = DataManager.GetSessionCaisseUser(caisseDuJour.Id, paiement.User.Id);
                if (sessionCaisseUser == null)
                {
                    //Si la session n'existe pas, Ouverture de caisse
                    sessionCaisseUser = new SessionCaisse();
                    sessionCaisseUser.FondDeCaisse = paiement.User.FondDeCaisse;
                    sessionCaisseUser.DateOuverture = DateTime.UtcNow;
                    sessionCaisseUser.Caisse = caisseDuJour;
                    sessionCaisseUser.User = paiement.User;
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

                //On ajoute la session à la session
                paiement.SessionCaisse = sessionCaisseUser;
                paiement.DateCreation = DateTime.UtcNow;
                //Enregistrer de la facture
                DataManager.Save(paiement);
                
                responseData = ResponseData.GetSuccess("Ok");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

    }
}
