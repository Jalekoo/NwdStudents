using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nwd.Web.Areas.Backoffice.Controllers
{
    public class AlbumController : Controller
    {
        //
        // GET: /Backoffice/Album/

        [Authorize(Roles="test")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
