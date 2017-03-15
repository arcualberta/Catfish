using Catfish.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Catfish.Models
{
    public class CatfishDbContext : DbContext
    {
        public CatfishDbContext()
            :base("piranha")
        {

        }

      //  public DbSet<Collection> Collections { get; set; }

    }
}