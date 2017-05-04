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
    }
}
