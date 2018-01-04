using Catfish.Core.Models;
using Catfish.Core.Models.Data;
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
            Guid = src.Guid;
            Thumbnail = u.Action("Thumbnail", "Items", new { id = itemId.HasValue ? itemId.Value : src.Id, name = src.Guid });
            Url = u.Action("File", "Items", new { id = itemId.HasValue ? itemId.Value : src.Id, guid = src.Guid });
        }

        public FileViewModel(DataFile src, int? id, RequestContext ctx, string controller)
        {
            UrlHelper u = new UrlHelper(ctx);
            Id = src.Id;
            FileName = src.FileName;
            Guid = src.Guid;
            Thumbnail = u.Action("Thumbnail", controller, new { id = id.HasValue ? id.Value : src.Id, name = src.Guid });
            Url = u.Action("File", controller, new { id = id.HasValue ? id.Value : src.Id, guid = src.Guid });
        }
    }
}
