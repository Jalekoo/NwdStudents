using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Nwd.BackOffice.Model;

namespace Nwd.BackOffice.Impl
{
    public class ArtistRepository
    {
        public List<Artist> GetAllArtists()
        {
            using( var ctx = new NwdBackOfficeContext() )
            {
                return ctx.Artists.ToList();
            }
        }

        public bool ArtistExists( Artist artist )
        {
            if( artist == null ) throw new ArgumentException("Artist can't be null");
            using( var ctx = new NwdBackOfficeContext() )
            {
                return ctx.Artists.Any( a => a.Name == artist.Name );
            }
        }

        public Artist CreateArtist( Artist artist, HttpServerUtilityBase server )
        {
            if( artist == null ) throw new ArgumentException( "Artist can't be null" );
            if( server == null ) throw new ArgumentException( "Server can't be null" );

            using( var ctx = new NwdBackOfficeContext() )
            {
                if( !ArtistExists( artist ) ) ctx.Artists.Add( artist );
                ctx.SaveChanges();
                return ctx.Artists.Single( a => a.Name == artist.Name );
            }
        }

        public Artist EditArtist( Artist artist, HttpServerUtilityBase server )
        {
            if( artist == null ) throw new ArgumentException( "Artist can't be null" );
            if( server == null ) throw new ArgumentException( "Server can't be null" );
            if( !ArtistExists( artist ) ) return null;

            return artist;
        }
    }
}
