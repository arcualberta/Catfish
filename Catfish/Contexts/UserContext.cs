using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Contexts
{
    public class UserContext
    {
        [ThreadStatic]
        public static UserContext Current;

        public Piranha.Entities.User User { get; private set; }

        protected UserContext(Piranha.Entities.User User)
        {
            this.User = User;
        }

        public static UserContext GetContextForUser(string guid)
        {
            if(string.IsNullOrEmpty(guid))
            {
                return null;
            }

            if(UserContext.Current == null || UserContext.Current.User == null || UserContext.Current.User.Id.ToString() != guid)
            {
                using (var db = new Piranha.DataContext())
                {
                    Piranha.Entities.User user = db.Users.Where(u => u.Id.ToString() == guid).FirstOrDefault();
                    if (user != null)
                    {
                        UserContext.Current = new UserContext(user);
                    }
                    else
                    {
                        UserContext.Current = null;
                    }
                }
            }

            return UserContext.Current;
        }
    }
}