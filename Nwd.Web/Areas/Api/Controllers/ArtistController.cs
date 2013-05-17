using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Nwd.BackOffice.Impl;
using Nwd.BackOffice.Model;

namespace Nwd.Web.Areas.Api.Controllers
{
    public class ArtistController : ApiController
    {
        ArtistRepository _repo;

        public ArtistController()
        {
            _repo = new ArtistRepository();
        }

        // GET api/album
        public IEnumerable<Artist> Get()
        {
            return _repo.GetAllArtists();
        }

        // GET api/album/5
        public Artist Get( int id )
        {
            return _repo.GetArtistForEdit(id);
        }

        // POST api/album
        public void Post( [FromBody]Artist artist )
        {
            _repo.CreateArtist( artist, new HttpServerUtilityWrapper( HttpContext.Current.Server ) );
        }

        // PUT api/album/5
        public void Put( [FromBody]Artist artist )
        {

            _repo.EditArtist( artist, new HttpServerUtilityWrapper( HttpContext.Current.Server ) );
        }

        // DELETE api/album/5
        public void Delete(int id)
        {

        }
    }
}
