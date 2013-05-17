using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nwd.FrontOffice.Model;
using System.Web;

namespace Nwd.FrontOffice.Impl
{
    public class PlaylistManagement : IPlaylistManagement
    {
        public IEnumerable<Playlist> GetAllPlaylist()
        {
            using( var ctx = new NwdFrontOfficeContext() )
            {
                return ctx.UserInfo.Where(u => u.UserName == HttpContext.Current.User.Identity.Name).SelectMany(m => m.Playlists).ToList();
            }
        }

        public Playlist AddNewPlaylistToCurrentUser( string playlistName )
        {
            if( String.IsNullOrWhiteSpace( playlistName ) ) throw new ArgumentNullException();

            var httpContext = HttpContext.Current;
            if( !httpContext.User.Identity.IsAuthenticated )
            {
                throw new UnauthorizedAccessException( "You must be authenticated in order to create a new playlist" );
            }
            using( var ctx = new NwdFrontOfficeContext() )
            {
                var user = ctx.UserInfo.SingleOrDefault( u => u.UserName == httpContext.User.Identity.Name );
                if( user == null )
                {
                    user = ctx.UserInfo.Add( new User
                    {
                        UserName = httpContext.User.Identity.Name
                    } );
                }
                Playlist p = new Playlist
                {
                    Title = playlistName
                };
                user.Playlists.Add( p );

                ctx.SaveChanges();
                return ctx.Playlists.Where( m => m.Title == playlistName ).FirstOrDefault();
            }
        }

        public void AddTrackToPlaylist( string playlistName, string songName, string songUrl )
        {
            if( String.IsNullOrWhiteSpace( playlistName ) ) throw new ArgumentNullException();
            if( String.IsNullOrWhiteSpace( songName ) ) throw new ArgumentNullException();
            if( String.IsNullOrWhiteSpace( songUrl ) ) throw new ArgumentNullException();

            var httpContext = HttpContext.Current;
            using( var ctx = new NwdFrontOfficeContext() )
            {
                var user = ctx.UserInfo.SingleOrDefault( u => u.UserName == httpContext.User.Identity.Name );
                if( user == null )
                {
                    user = ctx.UserInfo.Add( new User
                    {
                        UserName = httpContext.User.Identity.Name
                    } );
                }

                var playList = user.Playlists.SingleOrDefault( p => p.Title == playlistName );
                if( playList == null )
                {
                    throw new ApplicationException( "The playlist does not exists. Please valid this before calling this method." );
                }
                playList.Tracks.Add( new PlaylistTrack
                {
                    SongName = songName,
                    SongUrl = songUrl
                } );

                ctx.SaveChanges();
            }
        }

        public void DeleteAPlaylist( string playlistName )
        {
            using( var ctx = new NwdFrontOfficeContext() )
            {
                Playlist p = ctx.Playlists.Where( m => m.Title == playlistName ).FirstOrDefault();
                ctx.Playlists.Remove( p );
                ctx.SaveChanges();
            }
        }
    }
}
