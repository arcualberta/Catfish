using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class FileListViewModel : KoBaseViewModel
    {
        public List<FileViewModel> Files { get; set; }

        public FileListViewModel()
        {
            Files = new List<FileViewModel>();
        }

        public FileListViewModel(Item parent, RequestContext ctx)
        {
            Files = parent.Files.Select(f => new FileViewModel(f, parent.Id, ctx)).ToList();
            Id = parent.Id;
        }

        public override void UpdateDataModel(object dataModel, CatfishDbContext db)
        {
            Item item = dataModel as Item;

                
        }

    }
}