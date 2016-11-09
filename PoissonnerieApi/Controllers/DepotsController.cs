using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PoissonnerieApi.Controllers
{
    public class DepotsController : ApiController
    {
        // GET api/depots
        public object Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/depots/5
        public object Get(int id)
        {
            return "value";
        }

        // POST api/depots
        public object Post([FromBody]string value)
        {
            return null;
        }

        // PUT api/depots/5
        public object Put(int id, [FromBody]string value)
        {
            return null;
        }

        // DELETE api/depots/5
        public object Delete(int id)
        {
            return null;
        }
    }
}
