using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Services
{
    public class UserService
    {
        /// <summary>
        /// This service is retrieveing user related information using piranha context
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> GetUserIdAndLoginName()
        {
            Dictionary<string, string> users = new Dictionary<string, string>();
            using (var db = new DataContext())
            {
                users = db.Users.Select(u => new { u.Id, u.Login }).ToDictionary(u => u.Id.ToString(), u => u.Login);
            }

            return users;
        }
    }
}