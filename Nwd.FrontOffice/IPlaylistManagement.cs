using System;
using System.Collections.Generic;
using Nwd.FrontOffice.Model;
namespace Nwd.FrontOffice
{
    public interface IPlaylistManagement
    {
        IEnumerable<Playlist> GetAllPlaylist();
        Playlist AddNewPlaylistToCurrentUser( string playlistName );
        void AddTrackToPlaylist( string playlistName, string songName, string songUrl );
        void DeleteAPlaylist( string playlistName );
    }
}
