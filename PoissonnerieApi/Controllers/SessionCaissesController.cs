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
    public class SessionCaissesController : ApiController
    {
        // GET api/sessioncaisses
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                //Recuperer la session en cours
                responseData = ResponseData.GetSuccess(DataManager.GetAll<SessionCaisse>("DateOuverture", "DESC"));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/sessioncaisses/5
        public object GetSessionsCaisse(long caisse)
        {
            ResponseData responseData;
            try
            {
                var sessionsCaisse = DataManager.GetList<SessionCaisse>("Caisse", caisse);

                foreach (var session in sessionsCaisse)
                {
                    //Recuperer la somme des sorties de caisse
                    session.SortiesDeCaisse = DataManager.GetSumSortieDeCaisseSession(session.Id);
                    //Recupurer la somme des paiements effectués
                    session.TotalPaiement = DataManager.GetSumPaiementCaisseSession(session.Id);

                    //Calculer l'espece théorique
                    session.TotalEspeceTheorique = session.FondDeCaisse +
                        session.TotalPaiement - session.SortiesDeCaisse;

                    if(session.IsClosed)
                        session.DifferenceTheoriqueEtFermeture = session.TotalEspeceFermeture - session.TotalEspeceTheorique;
                }

                responseData = ResponseData.GetSuccess(sessionsCaisse);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/sessioncaisses?cloturer=idSessionCaisse
        public object GetFermerSession([FromUri]long user, [FromUri]long especeSaisi, [FromUri]string observation)
        {
            ResponseData responseData;
            try
            {
                if (especeSaisi < 0) {
                    return ResponseData.GetError("Montant invalide !!!");
                }

                var sessionsCaisse = DataManager.GetSessionCaisseUserOuvertes(user);
                if (sessionsCaisse == null)
                    return ResponseData.GetError("Session introuvable !");

                sessionsCaisse.IsClosed = true;
                sessionsCaisse.DateCloture = DateTime.UtcNow;

                sessionsCaisse.TotalEspeceFermeture = especeSaisi;
                sessionsCaisse.Observation = observation;

                DataManager.Save(sessionsCaisse);
                responseData = ResponseData.GetSuccess(sessionsCaisse);

            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/sessioncaisses?user=idUser
        public object GetSessionOuverte(long user)
        {
            ResponseData responseData;
            try
            {
                var sessionsCaisse = DataManager.GetSessionCaisseUserOuvertes(user);
                if (sessionsCaisse == null)
                {
                    sessionsCaisse = DataManager.GetLastSessionCaisseUser(user);
                    //return responseData = ResponseData.GetError("Session introuvable !");
                }
                    
                //Recuperer la somme des sorties de caisse
                sessionsCaisse.SortiesDeCaisse = DataManager.GetSumSortieDeCaisseSession(sessionsCaisse.Id);
                //Recupurer la somme des paiements effectués
                sessionsCaisse.TotalPaiement = DataManager.GetSumPaiementCaisseSession(sessionsCaisse.Id);

                //Calculer l'espece théorique
                sessionsCaisse.TotalEspeceTheorique = sessionsCaisse.FondDeCaisse +
                    sessionsCaisse.TotalPaiement - sessionsCaisse.SortiesDeCaisse;
                
                responseData = ResponseData.GetSuccess(sessionsCaisse);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }
    }
}
