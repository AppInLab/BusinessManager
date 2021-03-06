﻿using BusinessEngine.Manager;
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
    public class BlocksController : ApiController
    {

        // GET api/blocks
        public object GetAll()
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.GetAll<Block>());
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // GET api/blocks/5
        public object Get(int id)
        {
            ResponseData responseData;
            try
            {
                responseData = ResponseData.GetSuccess(DataManager.Get<Block>(id));
            }
            catch (Exception ex)
            {
                responseData = ResponseData.GetError(ex.Message);
            }

            return responseData;
        }

        // POST api/blocks
        public object Post([FromBody]JToken data)
        {
            ResponseData responseData;
            try
            {
                var _data = data.ToObject<Block>();
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
