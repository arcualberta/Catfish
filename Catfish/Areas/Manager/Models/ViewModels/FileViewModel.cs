using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using System.Web;
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



        public FileViewModel(DataFile src, int? id, RequestContext ctx, string controller)
        {
            InitializeInstance(src, id, ctx, controller);
        }

        public FileViewModel(DataFile src, int? id)
        {
            InitializeInstance(src, id, HttpContext.Current.Request.RequestContext, "Items");
        }

        public FileViewModel(DataFile src, int? itemId, RequestContext ctx)
        {
            InitializeInstance(src, itemId, ctx, "Items");
        }

        private void InitializeInstance(DataFile src, int? id, RequestContext ctx, string controller)
        {
            UrlHelper urlHelper = new UrlHelper(ctx);

            int idValue = id.HasValue ? id.Value : src.Id;

            Id = src.Id;
            FileName = src.FileName;
            Guid = src.Guid;
            
            Thumbnail = urlHelper.Action("Thumbnail", controller, new {
                id = idValue,
                name = src.Guid
            });

            Url = urlHelper.Action("File", controller, new {
                id = idValue,
                guid = src.Guid
            });
        }
    }
}
