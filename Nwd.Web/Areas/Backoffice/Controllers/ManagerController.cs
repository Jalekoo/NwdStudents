using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nwd.Web.Areas.Backoffice.Controllers
{

    [Authorize(Roles="Administrator")]
    public class ManagerController : Controller
    {
        //
        // GET: /Backoffice/Album/

        public ActionResult Albums()
        {
            return View();
        }
    }
}