using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Metadata;

namespace Catfish.Core.Services
{
    public class MetadataService : ServiceBase
    {
        public MetadataService(CatfishDbContext db) : base(db) { }

        public IQueryable<MetadataSet> GetMetadataSets()
        {
            return Db.MetadataSets;
        }
    }
}
