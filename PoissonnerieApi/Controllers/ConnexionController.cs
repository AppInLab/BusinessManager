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
    public class ConnexionController : ApiController
    {
        // POST api/comptes?login=x&mdp=y
        public object Post([FromBody] JToken data)
        {
            ResponseData responseData;
            try
            {
                var login = data["Login"].ToString();
                var mdp = data["Password"].ToString();

                var userFound = DataManager.GetUser(login, mdp);
                if (userFound != null)
                {
                    if (userFound.IsActif)
                    {
                        userFound.Password = null;

                        if (userFound.Privilege.Code == Constants.PRIVILEGE_SU_KEY)
                            userFound.IsSu = true;
                        else if (userFound.Privilege.Code == Constants.PRIVILEGE_ADMIN_KEY)
                            userFound.IsAdmin = true;
                        else if (userFound.Privilege.Code == Constants.PRIVILEGE_USTOCK_KEY)
                            userFound.IsStockUser = true;
                        else if (userFound.Privilege.Code == Constants.PRIVILEGE_USER_KEY)
                            userFound.IsUser = true;

                        var infosFacture = DataManager.Get<InfosFacture>(1);

                        responseData = ResponseData.GetSuccess(userFound, infosFacture);
                    }
                    else
                    {
                        responseData = ResponseData.GetError("Compte invalide. Contactez l'administrateur");
                    }
                }
                else
                {
                    responseData = ResponseData.GetError("Login ou mot de passe incorrect !");
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
