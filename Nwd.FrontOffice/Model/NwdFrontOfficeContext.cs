﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Nwd.FrontOffice.Model
{
    public class NwdFrontOfficeContext : DbContext
    {
        public NwdFrontOfficeContext()
            : base( "NwdMusikFront" )
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<User> UserInfo { get; set; }

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }
    }
}
