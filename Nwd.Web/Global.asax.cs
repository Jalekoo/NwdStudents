using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using Nwd.BackOffice.Model;
using Nwd.FrontOffice.Model;
using System.Diagnostics;

namespace Nwd.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register( GlobalConfiguration.Configuration );
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );

            Database.SetInitializer( new DropCreateDatabaseAlways<NwdBackOfficeContext>() );
            Database.SetInitializer( new DropCreateDatabaseAlways<NwdFrontOfficeContext>() );

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
        }
    }
}