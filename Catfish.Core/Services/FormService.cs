using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services
{
    public class FormService : DbEntityService, IFormsService
    {
        public FormService(AppDbContext db)
          : base(db)
        {

        }
        public Form Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<FormListEntry> List(int offset = 0, int max = 0)
        {
            throw new NotImplementedException();
        }
    }
}
