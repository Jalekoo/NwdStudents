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

            using( var ctx = new NwdBackOfficeContext() )
            {
                ctx.Database.Initialize( true );
                Debug.Assert( ctx.Database.Exists() );
                Console.WriteLine( ctx.Database.Connection.ConnectionString );
            }
            using( var ctx = new NwdFrontOfficeContext() )
            {
                ctx.Database.Initialize( true );
                Debug.Assert( ctx.Database.Exists() );
                Console.WriteLine( ctx.Database.Connection.ConnectionString );
            }
            using( var ctx = new NwdAuthContext() )
            {
                ctx.Database.Initialize( true );
                Debug.Assert( ctx.Database.Exists() );
                Console.WriteLine( ctx.Database.Connection.ConnectionString );
            }
        }
    }
}
