using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nwd.FrontOffice.Model;

namespace Nwd.Web.Areas.Api.Controllers
{
    public class PlaylistTrackController : ApiController
    {
        public IEnumerable<PlaylistTrack> Get( int id )
        {
            using (var ctx = new NwdFrontOfficeContext())
            {
                var u = ctx.Playlists.Where( a => a.Id == id ).SelectMany( m => m.Tracks ).ToList();
                if( u != null ) return u;
                return new List<PlaylistTrack>();
            }
        }
    }
}
