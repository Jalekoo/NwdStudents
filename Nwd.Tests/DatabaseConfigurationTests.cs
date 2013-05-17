using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Data.Entity;
using Nwd.BackOffice.Model;
using Nwd.FrontOffice.Model;
using Nwd.Authentication.Model;
using System.Diagnostics;
using Nwd.Authentication.Security;
using System.Web.Security;

namespace Nwd.Tests
{
    [TestFixture]
    public class DatabaseConfigurationTests
    {
        [Test]
        public void Database_Should_Always_Be_Created()
        {
            Database.SetInitializer( new DropCreateDatabaseAlways<NwdBackOfficeContext>() );
            Database.SetInitializer( new DropCreateDatabaseAlways<NwdFrontOfficeContext>() );
            Database.SetInitializer( new DropCreateDatabaseAlways<NwdAuthContext>() );

            using( var b = new NwdBackOfficeContext() )
            {
                b.Database.Initialize( true );
                Debug.Assert( b.Database.Exists() );
                Console.WriteLine( b.Database.Connection.ConnectionString );
            }
            using( var f = new NwdFrontOfficeContext() )
            {
                f.Database.Initialize( true );
                Debug.Assert( f.Database.Exists() );
                Console.WriteLine( f.Database.Connection.ConnectionString );
            }
            using( var a = new NwdAuthContext() )
            {
                a.Database.Initialize( true );
                Debug.Assert( a.Database.Exists() );
                Console.WriteLine( a.Database.Connection.ConnectionString );
                a.Roles.Add( new Role { RoleName = "User" } );
                Role r = a.Roles.Add( new Role { RoleName = "Administrator" } );
                Nwd.Authentication.Model.User u = a.Users.Add( new Nwd.Authentication.Model.User { Username = "SuperAdmin", Name = "NwdProvider", Comment = "user admin", IsApproved = true, IsLockedOut = false, Password = AuthenticationUtils.HashPassword( "test" ), CreationDate = DateTime.UtcNow } );
                u.Roles.Add( r );

                a.SaveChanges();
            }
        }
    }
}
