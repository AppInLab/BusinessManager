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
                responseData = ResponseData.GetSuccess(sessionsCaisse);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/sessioncaisses?cloturer=idSessionCaisse
        public object GetCloturer(long cloturer)
        {
            ResponseData responseData;
            try
            {
                var sessionsCaisse = DataManager.Get<SessionCaisse>(cloturer);
                if (sessionsCaisse == null)
                {
                    responseData = ResponseData.GetError("Session introuvable !");
                }
                else
                {
                    sessionsCaisse.IsClosed = true;
                    sessionsCaisse.DateCloture = DateTime.UtcNow;
                    DataManager.Save(sessionsCaisse);
                    responseData = ResponseData.GetSuccess(sessionsCaisse);
                }
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }
    }
}
