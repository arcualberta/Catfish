using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
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
    [ExportMetadata("InternalId", "BulletingBoardExtension")]
    [ExportMetadata("Name", "BulletinBoard")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class BulletinBoard: CatfishRegion
    {
        [Display(Name ="Collection Id")]
        public int CollectionId { get; set; }

        [Display(Name = "Entity Type Id")]
        public int EntityTypeId { get; set; }

        [Display(Name ="Item Count")]
        public int ItemCount { get; set; }

        [Display(Name = "Select Randomly")]
        public bool SelectRandomly { get; set; }

        [Display(Name = "Position Randomly")]
        public bool PositionRandomly { get; set; }
      
        [Display(Name = "Refresh Interval")]
        public int RefreshInterval { get; set; }
    }

    public class BulletinBoardItem
    {
        public int Id { get; set; }
        public string Thumbnail { get; set; }
        public string Image { get; set; }

        public List<MetadataFieldValue> Metadata { get; set; }

        public BulletinBoardItem(Item dataModel, RequestContext ctx, string fields)
        {
            DataFile file = dataModel.Files.FirstOrDefault();
            FileViewModel vm = new FileViewModel(file, dataModel.Id, ctx);
            Id = dataModel.Id;
            Thumbnail = vm.Thumbnail;
            Image = vm.Url;

            List<string> requiredFields = fields != null ? fields.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
            Metadata = new List<MetadataFieldValue>();
            foreach (MetadataSet ms in dataModel.MetadataSets)
            {
                foreach (FormField field in ms.Fields)
                {
                    if (requiredFields.Contains(field.GetName().ToLower()))
                    {
                        List<TextValue> vals = field.GetValues().Where(tv => !string.IsNullOrEmpty(tv.Value)).ToList();
                        if(vals.Count > 0)
                        {
                            Metadata.Add(new MetadataFieldValue()
                            {
                                FiledName = field.GetName(),
                                FieldValues = vals
                            });
                        }
                    }
                }
            }
        }
    }

    public class MetadataFieldValue
    {
        public string FiledName { get; set; }
        public List<TextValue> FieldValues { get; set; }
    }
}