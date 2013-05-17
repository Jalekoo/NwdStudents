using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nwd.Web.Controllers
{
    [Authorize( Roles = "User" )]
    public class PlaylistController : Controller
    {
        //
        // GET: /Playlist/

        public ActionResult Index()
        {
            return View();
        }

    }
}
