using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nwd.Web.Areas.Api.Controllers
{
    public class TrackController : ApiController
    {

        // GET api/track
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/track/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/track
        public void Post([FromBody]string value)
        {
        }

        // PUT api/track/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/track/5
        public void Delete(int id)
        {
        }
    }
}
