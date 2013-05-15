using System;
using System.Web;
using System.Web.Optimization;
namespace Nwd.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles( BundleCollection bundles )
        {
            #region Scripts
            bundles.Add( new ScriptBundle("~/bundles/JS/bootstrap").Include("~/Scripts/bootstrap/bootstrap.min.js") );
            bundles.Add( new ScriptBundle( "~/bundles/JS/knockout" ).Include( "~/Scripts/knockout-2.2.1.js" ) );
            #endregion

            #region CSS
            #endregion
        }
    }
}