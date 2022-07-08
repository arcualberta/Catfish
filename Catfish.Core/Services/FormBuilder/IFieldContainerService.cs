using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.FormBuilder
{
    public interface IFieldContainerService
    {
        public FieldContainerListVM Get(int offset = 0, int? max = null);
        public FieldContainer Get(Guid id);
        
    }

    public interface IFormService : IFieldContainerService
    {
        public List<BaseField> GetFieldDefinitions();
    }

    public interface IMetadataSetService : IFieldContainerService { }

}

