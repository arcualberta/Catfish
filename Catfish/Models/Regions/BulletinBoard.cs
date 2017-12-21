using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "CarouselExtension")]
    [ExportMetadata("Name", "Carousel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class BulletinBoard: CatfishRegion
    {
        [Display(Name ="Collection Id")]
        public int CollectionId { get; set; }

        [Display(Name ="Item Count")]
        public int ItemCount { get; set; }

        [Display(Name = "Select Randomly")]
        public bool SelectAtRandom { get; set; }

        [Display(Name = "Position at Random")]
        public bool PositionAtRandom { get; set; }
      
        [Display(Name = "Refresh Interval")]
        public int RefreshInterval { get; set; }
    }

    public class BulletinBoardItem
    {
        public int Id { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }

        public BulletinBoardItem(Item dataModel, RequestContext ctx)
        {
            DataFile file = dataModel.Files.FirstOrDefault();
            FileViewModel vm = new FileViewModel(file, dataModel.Id, ctx);
            Id = dataModel.Id;
            Thumbnail = vm.Thumbnail;
            Image = vm.Url;
        }
    }
}