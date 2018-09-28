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
            if(Current == null || Current.User == null || Current.User.Id.ToString() != guid)
            {
                using (var db = new Piranha.DataContext())
                {
                    Piranha.Entities.User user = db.Users.Where(u => u.Id.ToString() == guid).FirstOrDefault();
                    if (user != null)
                    {
                        user = db.Users.Where(u => u.Id.ToString() == guid).FirstOrDefault();
                        Current = new UserContext(user);
                    }
                }
            }

            return Current;
        }
    }
}