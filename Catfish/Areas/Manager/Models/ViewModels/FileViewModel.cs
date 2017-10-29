using Catfish.Core.Models;
using System.Web.Mvc;
using System.Web.Routing;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Guid { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }

        public FileViewModel() { }

        public FileViewModel(DataFile src, int? itemId, RequestContext ctx)
        {
            UrlHelper u = new UrlHelper(ctx);
            Id = src.Id;
            FileName = src.FileName;
            Guid = src.GuidName;
            Thumbnail = u.Action("Thumbnail", "Items", new { id = itemId.HasValue ? itemId.Value : src.Id, name = src.GuidName });
            Url = u.Action("File", "Items", new { id = itemId.HasValue ? itemId.Value : src.Id, guidName = src.GuidName });
        }
    }
}
