using Catfish.API.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class MySqlMoviesDbContext : DbContext
    {
        public MySqlMoviesDbContext(DbContextOptions<MySqlMoviesDbContext> options)
          : base(options)
        {
        }

        //public DbSet<MySqlMovie> Movies { get; set; }
    }




    //We are not mapping this to the database since we are going to read
    //this directly from the SQL dump file.

}
