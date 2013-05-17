using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nwd.FrontOffice.Impl;
using Nwd.FrontOffice.Model;

namespace Nwd.Web.Areas.Api.Controllers
{
    public class PlaylistController : ApiController
    {
        readonly PlaylistManagement _playlistManager;

        public PlaylistController()
        {
            _playlistManager = new PlaylistManagement();
        }

        public IEnumerable<Playlist> Get()
        {
            return _playlistManager.GetAllPlaylist();
        }

        public Playlist Post( [FromBody]PlaylistRequest p )
        {
            return _playlistManager.AddNewPlaylistToCurrentUser( p.PlaylistName );
        }

        public void Put( string playlistName, string songName, string songUrl )
        {
            _playlistManager.AddTrackToPlaylist( playlistName, songName, songUrl );
        }

        public bool Delete( [FromBody]PlaylistRequest model )
        {
            using (var ctx = new NwdFrontOfficeContext())
            {
                Playlist p = ctx.Playlists.FirstOrDefault( a => a.Title == model.PlaylistName );
                ctx.Playlists.Remove( p );
                ctx.SaveChanges();
                return true;
            }
        }

        public class PlaylistRequest
        {
            public string PlaylistName { get; set; }
        }
    }
}
