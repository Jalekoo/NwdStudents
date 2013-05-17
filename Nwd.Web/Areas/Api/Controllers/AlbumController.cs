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
    public class AlbumController : ApiController
    {
        AlbumRepository _repo;

        public AlbumController()
        {
            _repo = new AlbumRepository();
        }

        // GET api/album
        public IEnumerable<Album> Get()
        {
            return _repo.GetAllAlbums();
        }

        // GET api/album/5
        public Album Get(int id)
        {
            return _repo.GetAlbumForEdit(id);
        }

        // POST api/album
        public void Post([FromBody]Album album)
        {
            _repo.CreateAlbum( album, new HttpServerUtilityWrapper(HttpContext.Current.Server) );
        }

        // PUT api/album/5
        public void Put([FromBody]Album album)
        {

            _repo.EditAlbum( album, new HttpServerUtilityWrapper( HttpContext.Current.Server ) );
        }

        // DELETE api/album/5
        public void Delete(int id)
        {

        }
    }
}
