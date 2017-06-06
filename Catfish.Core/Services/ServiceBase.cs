using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catfish.Core.Models;

namespace Catfish.Core.Services
{
    public class ServiceBase
    {
        protected CatfishDbContext Db { get; set; }
        public ServiceBase(CatfishDbContext db)
        {
            Db = db;
        }

        public class CustomComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> mExpression;

            public CustomComparer(Func<T, T, bool> lambda)
            {
                mExpression = lambda;
            }

            public bool Equals(T x, T y)
            {
                return mExpression(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }
    }
}
