using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Catfish.Models.ViewModels;
using Piranha.AspNetCore.Identity.Models;
using Piranha.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity;
using Catfish.Helper;
using Piranha;

namespace Catfish
{
    public class CatfishUserModel : PageModel
    {
        private readonly Piranha.AspNetCore.Identity.IDb _db;
        private readonly UserManager<User> _userManager;
        private readonly ICatfishAppConfiguration _catfishConfig;
        private readonly ISecurity _security;

        public CatfishUserModel(UserManager<User> userManager, Piranha.AspNetCore.Identity.IDb db, ICatfishAppConfiguration catfishConfig,  ISecurity security) : base()
        {
            _db = db;
            _userManager = userManager;
            _catfishConfig = catfishConfig;
            _security = security;
        }
        /// <summary>
        /// Sign out login user from our system
        /// </summary>
        /// <returns></returns>
        public void OnGet()
        {
            _security.SignOut(_db);
        }
        /// <summary>
        /// Handle external login -- i.e Google account --
        /// Upon successful login, add this user to the databse if this user is not existing yet in our db based on the emailadress
        /// thelogin name (userName) for local account will be the email address
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(CatfishUserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                User _user = new Piranha.AspNetCore.Identity.Data.User();
               
                UserEditModel userEditModel = new UserEditModel();
                userEditModel.User = _user;
                userEditModel.Password = userModel.Password;
                userEditModel.PasswordConfirm = userModel.Password;
               // _user.
                
                if (await _userManager.FindByNameAsync(userModel.Login) == null)
                {
                     await _userManager.SetUserNameAsync(_user, userModel.Email);
                    await _userManager.SetEmailAsync(_user, userModel.Login);
                    await _userManager.CreateAsync(_user, userEditModel.Password);
                    string roleName = _catfishConfig.GetDefaultUserRole();
                    Role role = _db.Roles.Where(r => r.Name == roleName).FirstOrDefault();
                  
                    userEditModel.Roles.Add(role);
                     var result = userEditModel.Save(_userManager);

                    if(result.Result.Succeeded)
                    {
                       
                        //login user to the system
                        if(await _security.SignIn(HttpContext, userModel.Login, userModel.Password))
                            return new RedirectResult("/");
                    }

                }
                else
                {
                    //this user already in the system -- log him/her in
                    if (await _security.SignIn(HttpContext, userModel.Login, userModel.Password))
                        return new RedirectResult("/");
                }
            }
            return Page();
        }
    }
}