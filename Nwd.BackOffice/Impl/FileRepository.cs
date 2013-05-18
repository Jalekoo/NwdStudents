using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace Nwd.BackOffice.Impl
{
    public class FileRepository
    {
        public void SaveFile(string tmstmp, string path)
        {
            string pPath = HttpContext.Current.Server.MapPath( "~/Tmp/" );
            string tmpPath = pPath + tmstmp;
            if( !File.Exists( tmpPath ) ) throw new FileNotFoundException();

            switch( MimeMapping.GetMimeMapping( tmpPath ) )
            {
                case "image/jpeg":
                    Image img = Image.FromFile( tmpPath );
                    img.Save( path, ImageFormat.Jpeg );
                    break;
                default :
                    File.WriteAllBytes(path, File.ReadAllBytes( tmpPath ));
                    break;
            }
        }
        public void SaveAlbumPicture(string tmstmp)
        {
        }
    }
}
