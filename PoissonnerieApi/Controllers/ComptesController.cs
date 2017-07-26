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
    public class ComptesController : ApiController
    {
        // GET api/comptes
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                var users = DataManager.GetAll<User>();
                foreach (var u in users)
                    u.Password = null;

                responseData = ResponseData.GetSuccess(users);
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/comptes/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                var user = DataManager.Get<User>(id);
                if (user != null)
                {
                    user.Password = null;
                    responseData = ResponseData.GetSuccess(DataManager.Get<User>(id));
                }
                else
                {
                    responseData = ResponseData.GetError("Compte invalide");
                }
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/comptes/5
        public object GetCheckHauth(int checkhauth)
        {
            ResponseData responseData;
            try
            {
                var user = DataManager.Get<User>(checkhauth);
                if (user != null)
                {
                    user.Password = null;
                    if (user.IsActif)
                    {
                        if (user.Privilege.Code == Constants.PRIVILEGE_SU_KEY)
                            user.IsSu = true;
                        else if (user.Privilege.Code == Constants.PRIVILEGE_ADMIN_KEY)
                            user.IsAdmin = true;
                        else if (user.Privilege.Code == Constants.PRIVILEGE_USTOCK_KEY)
                            user.IsStockUser = true;
                        else if (user.Privilege.Code == Constants.PRIVILEGE_USER_KEY)
                            user.IsUser = true;

                        var infosFacture = DataManager.Get<InfosFacture>(1);

                        responseData = ResponseData.GetSuccess(user, infosFacture);
                    }
                    else
                        responseData = ResponseData.GetError("Compte inactif", -2);
                }
                else
                    responseData = ResponseData.GetError("Comptes invalide");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/comptes?user=x&activer=true
        public object Get([FromUri] long user, [FromUri]bool activer)
        {
            ResponseData responseData;
            try
            {
                var userFound = DataManager.Get<User>(user);
                if (userFound != null)
                {
                    userFound.IsActif = activer;
                    DataManager.Save(userFound);
                    responseData = ResponseData.GetSuccess("OK");
                }
                else
                {
                    responseData = ResponseData.GetError("Utilisateur introuvable !");
                }                
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }


        // POST api/comptes
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var _data = data.ToObject<User>();

                if (_data.Id > 0)
                {
                    _data.DateModification = DateTime.UtcNow;
                }
                else
                {
                    _data.DateCreation = DateTime.UtcNow;
                    _data.DateModification = DateTime.UtcNow;
                }

                DataManager.Save(_data);
                responseData = ResponseData.GetSuccess("OK");
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }
    }
}
