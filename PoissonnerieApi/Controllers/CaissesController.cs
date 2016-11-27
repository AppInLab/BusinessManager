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
    public class CaissesController : ApiController
    {
        // GET api/caisses
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.GetAll<Caisse>());
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/caisses/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.Get<Caisse>(id));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/caisses
        //public object Post([FromBody]JToken data)
        //{
        //    ResponseData responseData;
        //    try
        //    {
        //        var _data = data.ToObject<Caisse>();
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
