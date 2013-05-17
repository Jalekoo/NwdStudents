using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using Nwd.BackOffice.Impl;
using Nwd.BackOffice.Model;
using Nwd.FrontOffice.ViewModels;
using Nwd.Web.Areas.Api.Models;

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
        public IEnumerable<Track> Get( int idAlbum )
        {
            return _repo.GetTrack( idAlbum );
        }


        // POST api/track
        public async Task Post( [FromBody]TrackRegisterViewModel model )
        {
            Stream buffer = null;
            try
            {
                if( Request.Content.IsMimeMultipartContent() )
                {
                    var provider = await Request.Content.ReadAsMultipartAsync();
                    buffer = await provider.Contents[0].ReadAsStreamAsync();
                }

                if( buffer == null ) throw new ArgumentNullException();

                using( var ctx = new NwdBackOfficeContext() )
                {
                    int number = ctx.Albums.Where( a => a.Id == model.AlbumId ).FirstOrDefault().Tracks.Max( a => a.Number );
                    Track t = new Track { AlbumId = model.AlbumId, Duration = new TimeSpan( model.Duration ), Song = new Song { Composed = model.Song.Composed, Name = model.Song.Name }, Number = number };
                    ctx.Albums.SingleOrDefault( a => a.Id == model.AlbumId ).Tracks.Add( t );
                    ctx.SaveChanges();

                    string path = String.Format( "~/Private/{0}/{1}", model.AlbumId, t.AlbumId, t.Number );
                    t.FileRelativePath = path;

                    FileStream f = File.Create( HostingEnvironment.MapPath( path ) );
                    buffer.Seek( 0, SeekOrigin.Begin );
                    buffer.CopyTo( f );

                    f.Close();
                    ctx.SaveChanges();
                }
            }
            finally
            {
                buffer.Dispose();
            }
        }

        // PUT api/track/5
        public void Put( int id, [FromBody]string value )
        {
        }

        // DELETE api/track/5
        public void Delete( int id )
        {
        }
    }
}
