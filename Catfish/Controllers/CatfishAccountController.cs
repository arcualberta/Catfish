using System;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using Piranha.Models;

using System.Web.Security;
using Piranha.Entities;
using Catfish.Models;

using System.Security.Claims;
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
                        return Redirect(m.ReturnUrl);
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
        /// <summary>
        /// Login with Google account
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public ActionResult ExternalLogin(Piranha.Entities.User userModel)
        {
            if (ModelState.IsValid)
            {

                Piranha.Models.LoginModel loginModel = new LoginModel();
                User user = GetUserByName(userModel.Login);
                loginModel.Login = userModel.Login;
                loginModel.Password = userModel.Password;
                if (user == null)
                {
                    if (AllowGoogleLogin())
                    {
                        user = CreateNewUser(userModel);
                        using (var db = new Piranha.DataContext())
                        {
                            // Login sysuser into the current context.
                            db.LoginSys();
                            db.Users.Add(user);
                            if (db.SaveChanges() > 0)
                            {
                                return Login(loginModel);
                            }

                        }
                    }

                }
                else
                {
                    //log he in using his gmail account -- user existed in our system

                    return Login(loginModel);
                }

            }
            else {
                return RedirectToAction("Login");
            }
            return Redirect("~/");
        }
      
        // GET: UserAccount
        public ActionResult AuthenticationFailure()
        {
            return View();
        }
        /// <summary>
        /// get piranha User by Login name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 
        public User GetUserByName(string name)
        {
            User usr = null;
            using (var db = new Piranha.DataContext())
            {
                usr = db.Users.Where(u => u.Login == name).FirstOrDefault();
            }

            return usr;
        }
        /// <summary>
        /// get piranha User by Login name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public User GetUserByEmail(string email)
        {
            User usr = null;
            using (var db = new Piranha.DataContext())
            {
                usr = db.Users.Where(u => u.Email == email).FirstOrDefault();
            }

            return usr;
        }
      
        public bool AllowGoogleLogin()
        {
            return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AllowGoogleLogin"]);
        }
     
      
        /// <summary>
        /// Create piranha User when user is login from external mean like Google
        /// </summary>
        /// <param name="user">Piranha.Entities.User</param>
        /// <returns></returns>
        private User CreateNewUser(Piranha.Entities.User user)
        {
            //get piranha Group
            Group group = null;
            using (var db = new Piranha.DataContext())
            {
                string groupName = GetInitialGroupName();
                group = db.Groups.Where(g => g.Name == groupName).FirstOrDefault();
            }
            user.GroupId = group.Id;
            if (!String.IsNullOrEmpty(user.Password))
            {
                user.Password = Piranha.Models.SysUserPassword.Encrypt(user.Password);
            }
            return user;
        }
      
        public string GetInitialGroupName()
        { 
            return System.Configuration.ConfigurationManager.AppSettings["InitialGroupName"];
        }
    }
}