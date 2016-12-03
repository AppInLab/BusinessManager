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
    }
}
