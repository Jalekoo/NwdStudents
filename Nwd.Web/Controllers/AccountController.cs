using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Nwd.Authentication.Model;
using Nwd.Authentication.Security;
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

                    return RedirectToAction( "Redirect" );
                }
                else
                {
                    ModelState.AddModelError( "", "Nom d'utilisateur ou mot de passe incorrect." );
                }
            }

            return View( model );
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register( RegisterViewModel model )
        {
            if( ModelState.IsValid )
            {
                using( var ctx = new NwdAuthContext() )
                {
                    if( ctx.Users.Any( m => m.Username == model.Username ) )
                        ModelState.AddModelError( "Username", "Ce nom d'utilisateur existe déjà !" );
                    else
                    {
                        try
                        {
                            User u = ctx.Users.Add( new User { Username = model.Username, Name = "NwdProvider", Password = AuthenticationUtils.HashPassword( model.Password ), Email = model.Email, CreationDate = DateTime.UtcNow } );
                            u.Roles.Add( ctx.Roles.Where( r => r.RoleName == "User" ).FirstOrDefault() );
                            ctx.SaveChanges();
                            return View( "RegisterSuccess" );
                        }
                        catch( MembershipCreateUserException ex )
                        {
                            ModelState.AddModelError( "", ex.Message );
                        }
                    }
                }
            }
            return View( model );
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction( "LogIn" );
        }

        public ActionResult Redirect()
        {
            if( HttpContext.User.Identity.IsAuthenticated == true )
            {
                if( HttpContext.User.IsInRole( "Administrator" ) )
                    return RedirectToAction( "Albums", "Manager", new { Area = "Backoffice" } );

                return RedirectToAction( "Index", "Playlist", new { Area = "" } );
            }
            return RedirectToAction( "Index", "Home", new { Area = "" } );
        }
    }
}
