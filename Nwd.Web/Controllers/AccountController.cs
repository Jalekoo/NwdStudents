using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Nwd.Authentication.ViewModels;

namespace Nwd.Web.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn( LogInViewModel model, string returnUrl )
        {
            if( ModelState.IsValid )
            {
                if( Membership.ValidateUser( model.Username, model.Password ) )
                {
                    FormsAuthentication.SetAuthCookie( model.Username, model.RememberMe );
                    if( Url.IsLocalUrl( returnUrl ) && returnUrl.Length > 1 && returnUrl.StartsWith( "/" )
                        && !returnUrl.StartsWith( "//" ) && !returnUrl.StartsWith( "/\\" ) )
                    {
                        return RedirectToAction( returnUrl );
                    }
                    else
                    {
                        return RedirectToAction( "Index", "Home", new { Area = "Backoffice" } );
                    }
                }
                else
                {
                    ModelState.AddModelError( "", "The username or password is incorrect." );
                }
            }

            return View( model );
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction( "LogIn" );
        }
    }
}
