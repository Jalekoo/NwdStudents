using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nwd.BackOffice.Impl;
using Nwd.BackOffice.Model;

namespace Nwd.Web.Areas.Api.Controllers
{
    public class TrackController : ApiController
    {
        AlbumRepository _repo;

        public TrackController()
        {
            _repo = new AlbumRepository();
        }

        // GET api/track
        public IEnumerable<Track> Get(int idAlbum)
        {
            return _repo.GetTrack( idAlbum );
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
