using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Piranha.AspNetCore.Identity.SQLServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    public class PiranhaDbContext : IdentitySQLServerDb
    {
        public IConfiguration Configuration { get; set; }
        public PiranhaDbContext(DbContextOptions<IdentitySQLServerDb> options, IConfiguration configuration)
        : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("catfish"));
            }
        }
    }
}
