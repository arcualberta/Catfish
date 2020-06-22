using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Core.Services.FormBuilder
{
    public class MetadataSetService : DbEntityService, IMetadataSetService
    {
        public MetadataSetService(AppDbContext db)
         : base(db)
        {

        }

        public FieldContainer Get(Guid id)
        {
            return Db.MetadataSets.Where(x => x.Id == id).FirstOrDefault();
        }

        public FieldContainerListVM Get(int offset = 0, int? max = null)
        {
            IQueryable<MetadataSet> query = Db.MetadataSets.Skip(offset);
            if (max.HasValue)
                query = query.Take(max.Value);

            var forms = query.ToList();
            FieldContainerListVM result = new FieldContainerListVM()
            {
                OffSet = offset,
                Max = max,
                Entries = forms.Select(x => new FieldContainerListEntry(x)).ToList()
            };
            return result;
        }
    }
}
