using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nwd.Web.Areas.Backoffice.Controllers
{
    [Authorize(Roles="Administrator")]
    public class AlbumController : Controller
    {
        //
        // GET: /Backoffice/Album/

        public ActionResult Index()
        {
            return View();
        }
    }
}
