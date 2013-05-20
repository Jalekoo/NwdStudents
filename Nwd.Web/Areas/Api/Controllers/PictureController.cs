using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Nwd.BackOffice.Impl;
using Nwd.BackOffice.Model;


namespace Nwd.Web.Areas.Api.Controllers
{
    public class PictureController : ApiController
    {
        private async Task<Stream> ReadFileContent()
        {
            Stream buffer = null;

            if( Request.Content.IsMimeMultipartContent() )
            {
                var provider = await Request.Content.ReadAsMultipartAsync();
                buffer = await provider.Contents[2].ReadAsStreamAsync();
            }
            else
                buffer = await Request.Content.ReadAsStreamAsync();
            return buffer;
        }
        public class FileRsult
        {
            public string Id { get; set; }
        }
        private FileRsult SaveImage( Stream s )
        {
            using( Image img = Image.FromStream( s ) )
            {
                string tmstmp = DateTime.UtcNow.Ticks.ToString() + ".jpg";
                string path = HttpContext.Current.Server.MapPath("~/Tmp/" + tmstmp);
                img.Save(path, ImageFormat.Jpeg);

                return new FileRsult
                {
                    Id = tmstmp
                };
            }
        }

        public async Task<FileRsult> Post()
        {
            using( var s = await ReadFileContent() )
            {
                return SaveImage( s );
            }
        }

    }
}