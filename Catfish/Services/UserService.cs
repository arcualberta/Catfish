using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Services
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

        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Piranha.Entities.User GetUserById(string id)
        {
            Piranha.Entities.User user = new Piranha.Entities.User();
            using (var db = new DataContext())
            {
                user = db.Users.Where(u => u.Id.ToString() == id).FirstOrDefault();
            }

            return user;
        }

        /// <summary>
        /// Get user by Login Name
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Piranha.Entities.User GetUserByLogin(string login)
        {
            Piranha.Entities.User user = new Piranha.Entities.User();
            try
            {
                using (var db = new DataContext())
                {
                    user = db.Users.Where(u => u.Login == login).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return user;
        }

        public Piranha.Entities.User GetUserByEmail(string email)
        {
            Piranha.Entities.User user = new Piranha.Entities.User();
            using (var db = new DataContext())
            {
                user = db.Users.Where(u => u.Email == email).FirstOrDefault();
            }

            return user;
        }

        public IEnumerable<Piranha.Entities.User> GetAllUsers()
        {
           // IEnumerable<Piranha.Entities.User> users = null;
            try
            {
                using (var db = new DataContext())
                {
                    return db.Users.ToList();
                   // return users;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
           // return users;
        }
    }
}