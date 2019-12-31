using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
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
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("catfish"));
            }
        }

        public ObjectContext ObjectContext
        {
            get
            {
                return ((IObjectContextAdapter)this).ObjectContext;
            }
        }

        public void Detach(object entity)
        {
            ObjectContext.Detach(entity);
        }





        public DbSet<CFXmlModel> XmlModels { get; set; }

        public DbSet<CFEntity> Entities { get; set; }

        public DbSet<CFEntityType> EntityTypes { get; set; }

        public DbSet<CFMetadataSet> MetadataSets { get; set; }

        public DbSet<CFUserList> UserLists { get; set; }
        public DbSet<CFUserListEntry> UserListEntries { get; set; }

        public DbSet<CFEntityTypeAttributeMapping> EntityTypeAttributeMappings { get; set; }


        //        public DbSet<CFAccessDefinition> AccessDefinitions { get; set; }



    }
}
