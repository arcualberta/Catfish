using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Services
{
    public interface IItemTemplateAppletService
    {
        public ItemTemplate GetItemTemplate(Guid id, ClaimsPrincipal user);
        public List<Group> GetTemplateGroups(Guid? id);
        public DataItem GetDataItem(Guid itemTemplate, Guid ChildFormId);

        public List<DataItem> GetDataItems(Guid itemTemplate, bool isRoot=false);
        public List<DataItem> GetAllDataItems(Guid itemTemplate);
    }
}
