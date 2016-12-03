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
    public class SortieDeCaissesController : ApiController
    {
        // GET api/sortiedecaisses
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                //Recuperer la session en cours
                responseData = ResponseData.GetSuccess(DataManager.GetAll<SortieDeCaisse>("DateCreation", "DESC"));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/sortiedecaisses/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.Get<SortieDeCaisse>(id));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        public object GetSortieDeCaisseDuJour(long user)
        {
            ResponseData responseData;
            try
            {
                var _user = DataManager.Get<User>(user);
                if (_user == null)
                {
                    return responseData = ResponseData.GetError("Utilisateur introuvable !");
                }

                //Recherche de la caisse et de la session de caisse
                //Recuperer la caisse ouvert du jour
                var caisseDuJour = DataManager.GetCaisseOuverte();
                var desactiveModification = false;

                if (caisseDuJour == null)
                {
                    caisseDuJour = DataManager.GetCaisseOuverte(DateTime.UtcNow.Date);
                    if (caisseDuJour != null)
                    {
                        desactiveModification = true;
                        //return ResponseData.GetError("Impossible d'ouvrir 2 caisses le même jour.");
                    }
                    else
                    {
                        caisseDuJour = new Caisse();
                        caisseDuJour.DateOuverture = DateTime.UtcNow;
                        //TODO: Code à definir
                        //caisseDuJour.Code
                        DataManager.Save(caisseDuJour);
                    }
                }

                //Recuperer la session de la caisse de l'utilisateur
                var sessionCaisseUser = DataManager.GetSessionCaisseUser(caisseDuJour.Id, _user.Id);
                if (sessionCaisseUser == null)
                {
                    //Si la session n'existe pas, Ouverture de caisse
                    sessionCaisseUser = new SessionCaisse();
                    sessionCaisseUser.FondDeCaisse = _user.FondDeCaisse;
                    sessionCaisseUser.DateOuverture = DateTime.UtcNow;
                    sessionCaisseUser.Caisse = caisseDuJour;
                    sessionCaisseUser.User = _user;
                    sessionCaisseUser.IsClosed = false;
                    //TODO:A definir
                    //sessionCaisseUser.Code

                    DataManager.Save(sessionCaisseUser);
                }
                else
                {
                    if (sessionCaisseUser.IsClosed)
                    {
                        desactiveModification = true;
                        //return ResponseData.GetError("Opération refusée ! Votre session de caisse est clôturée.");
                    }
                }

                var sortiesDeCaisses = DataManager.GetList<SortieDeCaisse>("SessionCaisse.Id", sessionCaisseUser.Id, "DateCreation DESC");
                foreach (var sdc in sortiesDeCaisses)
                    sdc.DesactiveModification = desactiveModification;
                
                responseData = ResponseData.GetSuccess(sortiesDeCaisses);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/sortiedecaisses
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var _data = data.ToObject<SortieDeCaisse>();

                if (_data.Id > 0)
                {
                    _data.DateModification = DateTime.UtcNow;
                }
                else
                {
                    _data.DateCreation = DateTime.UtcNow;
                    _data.DateModification = DateTime.UtcNow;
                }

                //Recherche de la caisse et de la session de caisse
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
                var sessionCaisseUser = DataManager.GetSessionCaisseUser(caisseDuJour.Id, _data.User.Id);
                if (sessionCaisseUser == null)
                {
                    //Si la session n'existe pas, Ouverture de caisse
                    sessionCaisseUser = new SessionCaisse();
                    sessionCaisseUser.FondDeCaisse = _data.User.FondDeCaisse;
                    sessionCaisseUser.DateOuverture = DateTime.UtcNow;
                    sessionCaisseUser.Caisse = caisseDuJour;
                    sessionCaisseUser.User = _data.User;
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

                //Recuperation de la session de caisse
                _data.SessionCaisse = sessionCaisseUser;
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
