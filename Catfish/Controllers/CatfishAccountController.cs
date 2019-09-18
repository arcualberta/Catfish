using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Piranha.Models;

//using System.Diagnostics;
//using System.Reflection;
//using System.Web.Mvc;
using System.Web.Security;

namespace Catfish.Controllers
{
    public class CatfishAccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            Piranha.Models.LoginModel m = new LoginModel();

            return View();
        }
        /// <summary>
		/// Logs in the provided user.
		/// </summary>
		/// <param name="m">The model</param>
		[HttpPost]
        public ActionResult Login(Piranha.Models.LoginModel m)
        {
            // Authenticate the user
            if (ModelState.IsValid)
            {
                SysUser user = SysUser.Authenticate(m.Login, m.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), m.RememberMe);
                    HttpContext.Session[PiranhaApp.USER] = user;

                    // Redirect after logon
                    if (string.IsNullOrEmpty(m.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        return RedirectToAction("index", "managerstart");
                    }
                }
                else
                {
                    ViewBag.Message = @Piranha.Resources.Account.MessageLoginFailed;
                    ViewBag.MessageCss = "error";
                }
            }
            else
            {
                ViewBag.Message = @Piranha.Resources.Account.MessageLoginEmptyFields;
                ViewBag.MessageCss = "";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return Redirect("~/");
        }
    }
}