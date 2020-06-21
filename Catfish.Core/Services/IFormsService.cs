using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services
{
    public interface IFormsService
    {
        public FieldContainerListVM GetForms(int offset = 0, int max = 0);
        public FieldContainerListVM GetMetadataSets(int offset = 0, int max = 0);
        public FieldContainer Get(Guid id);
    }
}
