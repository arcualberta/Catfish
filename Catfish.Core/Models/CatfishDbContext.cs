using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    public class CatfishDbContext : DbContext
    {
        /// <summary>
        /// The application config.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        public CatfishDbContext(DbContextOptions<CatfishDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }


        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("piranha"));
            }
        }
    }
}
