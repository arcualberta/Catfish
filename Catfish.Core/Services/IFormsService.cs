using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services
{
    public interface IFormsService
    {
        public List<FormListEntry> List(int offset = 0, int max = 0);
        public Form Get(Guid id);
    }
}
