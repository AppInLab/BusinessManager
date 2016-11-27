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
    public class PrivilegesController : ApiController
    {
        // GET api/privileges
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.GetAll<Privilege>());
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/privileges/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.Get<Privilege>(id));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/privileges
        //public object Post([FromBody]JToken data)
        //{
        //    ResponseData responseData;
        //    try
        //    {
        //        var _data = data.ToObject<Privilege>();

        //        DataManager.Save(_data);
        //        responseData = ResponseData.GetSuccess(_data);
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData = ResponseData.GetError(ex.Message);
        //    }

        //    return responseData;
        //}
    }
}
