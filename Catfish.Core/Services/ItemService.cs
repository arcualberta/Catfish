using Catfish.Core.Models;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ElmahCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Catfish.Core.Helpers;
using Catfish.Core.Models.Contents.Fields.ViewModels;

namespace Catfish.Core.Services
{
    public class ItemService : DbEntityService
    {
        private readonly IConfiguration _configuration;

        public ItemService(AppDbContext db, ErrorLog errorLog, IConfiguration configuration)
            : base(db, errorLog)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns list of items grouped by their primary collection containers
        /// </summary>
        /// <returns></returns>
        public ItemListVM GetItems(int offset = 0, int max = 0)
        {
            try
            {
                ItemListVM ret = new ItemListVM() { OffSet = offset, Max = max };

                if (max == 0)
                    max = Int32.MaxValue;

                var items = Db.Items
                    .Skip(offset)
                    .Take(max)
                    .Include(it => it.PrimaryCollection)
                    .Select(it => new EntityListEntry(it))
                    .ToList();

                //List of items that does not have a primary collection
                CollectionContent root = null;
                if (items.Where(it => it.PrimaryCollectionId.HasValue == false).Any())
                {
                    root = new CollectionContent()
                    {
                        Name = new MultilingualName(),
                        Description = new MultilingualDescription()
                    };
                    root.Name.SetContent("Root");
                    root.Description.SetContent("Items with no primary collection");
                    root.Items.AddRange(items.Where(it => it.PrimaryCollectionId == null));
                }


                // List of collections which acts as primary collections for the items
                var primaryCollectionIds = items
                    .Where(it => it.PrimaryCollectionId.HasValue)
                    .Select(it => it.PrimaryCollectionId)
                    .Distinct()
                    .ToList();
                foreach (var id in primaryCollectionIds)
                {
                    try
                    {
                        var itemSubSet = items.Where(it => it.PrimaryCollectionId == id);
                        CollectionContent c = new CollectionContent()
                        {
                            Id = itemSubSet.First().PrimaryCollectionId,
                            Name = itemSubSet.First().PrimaryCollectionName
                        };
                        c.Items.AddRange(itemSubSet);
                        ret.Collections.Add(c);
                    }
                    catch (Exception ex)
                    {
                        _errorLog.Log(new Error(ex));
                    }
                }

                //TODO: order the collection array by the name of the collections in the default language

                if (root != null)
                {
                    ret.Collections.Insert(0, root);
                }


                //Adding the Item type templates
                var itemTemplates = Db.ItemTemplates
                    .Where(t => t.TargetType == new Item().GetType().FullName)
                    .Select(t => new EntityTemplateListEntry(t))
                    .ToList();
                ret.ItemTypes.AddRange(itemTemplates);

                //Adding the Collection type templates
                var collectionTemplates = Db.CollectionTemplates
                    .Where(t => t.TargetType == new Collection().GetType().FullName)
                    .Select(t => new EntityTemplateListEntry(t))
                    .ToList();
                ret.CollectionTypes.AddRange(collectionTemplates);

                return ret;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        
        public void UpdateItemlDataModel(ItemVM viewModel)
        {
            try
            {
                Item dataModel = Db.Items.Where(i => i.Id == viewModel.Id).FirstOrDefault();

                //Delegating the responsibility of updating the data model to the view model
                viewModel.UpdateDataModel(dataModel);

                dataModel.Updated = DateTime.Now;

                Db.SaveChanges();

                //solrIndexService.AddUpdate(new SolrItemModel(model));
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                throw ex;
            }
        }

        public List<FileReference> UploadFiles(ICollection<IFormFile> files, string uploadRoot = null)
        {
            if (string.IsNullOrEmpty(uploadRoot))
                uploadRoot = ConfigHelper.GetUploadTempFolder(true);

            List<FileReference> fileReferences = new List<FileReference>();
            foreach (IFormFile file in files)
            {
                FileReference fileRef = new FileReference();
                fileRef.Size = file.Length;
                fileRef.OriginalFileName = file.FileName;
                fileRef.FileName = fileRef.Id + "_" + file.FileName.Replace(" ", "_");
                fileRef.ContentType = file.ContentType;
                fileRef.Thumbnail = GetThumbnail(file.ContentType);

                //Destination absolute path name
                string pathName = Path.Combine(uploadRoot, fileRef.FileName);
                using (var stream = new FileStream(pathName, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                fileReferences.Add(fileRef);
            }

            return fileReferences;
        }

        public string GetThumbnail(string contentType)
        {
            string icon_path = "/assets/images/icons/";

            if (contentType == "application/pdf")
                return Path.Combine(icon_path, "pdf.png");

            if (contentType == "application/msword")
                return Path.Combine(icon_path, "doc.png");

            if (contentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                return Path.Combine(icon_path, "docx.png");

            if (contentType == "application/vnd.ms-excel")
                return Path.Combine(icon_path, "xls.png");

            if (contentType == "application/x-zip-compressed")
                return Path.Combine(icon_path, "zip.png");

            if (contentType == "image/jpeg")
                return Path.Combine(icon_path, "jpg.png");

            if (contentType == "image/png")
                return Path.Combine(icon_path, "png.png");

            if (contentType == "image/tiff")
                return Path.Combine(icon_path, "tiff.png");

            if (contentType == "text/html")
                return Path.Combine(icon_path, "html.png");

            if (contentType == "text/xml")
                return Path.Combine(icon_path, "xml.png");

            return Path.Combine(icon_path, "other.png");
        }
    }
}
