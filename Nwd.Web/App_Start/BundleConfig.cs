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
            bundles.IgnoreList.Ignore( "*.map", OptimizationMode.Always );
            bundles.Add( new ScriptBundle( "~/bundles/JS/jquery" ).Include( "~/Scripts/jquery-{version}.js" ).Include("~/Scripts/jquery-ui-1.10.2.js" ));
            bundles.Add( new ScriptBundle( "~/bundles/JS/bootstrap").Include("~/Scripts/bootstrap/js/bootstrap.js") );
            bundles.Add( new ScriptBundle( "~/bundles/JS/knockout" ).Include( "~/Scripts/knockout-2.2.1.js" ) );
            
            #endregion

            #region CSS
            bundles.Add( new StyleBundle( "~/bundles/CSS/Layout" ).Include("~/Content/Layout.css") );
            bundles.Add( new StyleBundle( "~/bundles/CSS/bootstrap" ).Include( "~/Scripts/bootstrap/css/bootstrap.css",
                "~/Scripts/bootstrap/css/bootstrap-responsive.css" ) );
            #endregion
        }
    }
}