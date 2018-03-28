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
        public string ThumbnailUrl { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
        public string Label { get; set; }
        public DataFile.eThumbnailTypes ThumbnailType { get; set; }


        public FileViewModel() { }

        public FileViewModel(DataFile src, int? id, RequestContext ctx, string controller)
        {
            InitializeInstance(src, id, ctx, controller);
        }

        public FileViewModel(DataFile src, int? id)
        {
            InitializeInstance(src, id, HttpContext.Current.Request.RequestContext, "Items");
        }

        public FileViewModel(FileDescription fileDescription, int? id)
        {
            InitializeInstance(fileDescription, id);
        }

        public FileViewModel(DataFile src, int? itemId, RequestContext ctx)
        {
            InitializeInstance(src, itemId, ctx, "Items");
        }          

        public DataFile ToDataFile()
        {
            DataFile dataFile = new DataFile();
            dataFile.Id = Id;
            dataFile.FileName = FileName;
            dataFile.Guid = Guid;            
            dataFile.Path = Path;
            dataFile.Thumbnail = Thumbnail;
            dataFile.ThumbnailType = ThumbnailType;
            dataFile.ContentType = ContentType;
            return dataFile;
        }

        public FileDescription ToFileDescription()
        {
            FileDescription fileDescription = new FileDescription();
            fileDescription.DataFile = ToDataFile();
            fileDescription.Label = Label;
            return fileDescription;
        }

        private void InitializeInstance(FileDescription fileDescription, int? id)
        {
            Label = fileDescription.Label;
            InitializeInstance(fileDescription.DataFile, id, HttpContext.Current.Request.RequestContext, "Items"); ;
        }

        private void InitializeInstance(DataFile src, int? id, RequestContext ctx, string controller)
        {
            UrlHelper urlHelper = new UrlHelper(ctx);

            int idValue = id.HasValue ? id.Value : src.Id;

            Id = src.Id;
            FileName = src.FileName;
            Guid = src.Guid;
            Path = src.Path;
            Thumbnail = src.Thumbnail;
            ContentType = src.ContentType;
            ThumbnailType = src.ThumbnailType;
            ThumbnailUrl = "url('" + urlHelper.Action("Thumbnail", controller, new
            {
                id = idValue,
                name = src.Guid
            }) + "')";


            Url = urlHelper.Action("File", controller, new {
                id = idValue,
                guid = src.Guid
            });
        }
    }
}
