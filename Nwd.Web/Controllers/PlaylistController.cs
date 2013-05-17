using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nwd.Web.Controllers
{
    public class PlaylistController : Controller
    {
        //
        // GET: /Playlist/

        [Authorize( Roles = "User" )]
        public ActionResult Index()
        {
            return View();
        }

    }
}
