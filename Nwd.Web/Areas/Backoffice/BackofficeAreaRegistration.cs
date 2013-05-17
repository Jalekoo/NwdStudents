using System.Web.Mvc;

namespace Nwd.Web.Areas.Backoffice
{
    public class BackofficeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Backoffice";
            }
        }

        public override void RegisterArea( AreaRegistrationContext context )
        {
            context.MapRoute(
                "Backoffice_default",
                "Backoffice/{controller}/{action}/{id}",
                new { controller = "Manager", action = "Index", id = UrlParameter.Optional },
                new[] { "Nwd.Web.Areas.Backoffice.Controllers" }
            );
        }
    }
}
