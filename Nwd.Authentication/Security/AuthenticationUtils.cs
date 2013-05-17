using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;

namespace Nwd.Authentication.Security
{
    public static class AuthenticationUtils
    {
        public static string HashPassword( string password )
        {
            Configuration configuration = WebConfigurationManager.OpenWebConfiguration( HostingEnvironment.ApplicationVirtualPath );
            MachineKeySection machineKey = (MachineKeySection)configuration.GetSection( "system.web/machineKey" );

            HMACSHA1 hash = new HMACSHA1 { Key = HexToByte( machineKey.ValidationKey ) };
            return Convert.ToBase64String( hash.ComputeHash( Encoding.Unicode.GetBytes( password ) ) );
        }

        private static byte[] HexToByte( string hexString )
        {
            return UTF8Encoding.UTF8.GetBytes( hexString );
        }
    }
}
