using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Metadata;
using Catfish.Core.Services;
using Catfish.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityTypeViewModel : KoBaseViewModel
    {
        public string TypeLabel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MetadataSetListItem> AvailableMetadataSets { get; set; }
        public List<MetadataSetListItem> SelectedMetadataSets { get; set; }
        public List<MetadataSetListItem> AssociatedMetadataSets { get; set; }
        //public MetadataFieldMapping NameMapping { get; set; }
        //public MetadataFieldMapping DescriptionMapping { get; set; }

        public EntityTypeViewModel()
        {
            AvailableMetadataSets = new List<MetadataSetListItem>();
            AssociatedMetadataSets = new List<MetadataSetListItem>();
            SelectedMetadataSets = new List<MetadataSetListItem>();
        }
 
        public void UpdateViewModel(object dataModel, CatfishDbContext db)
        {
            EntityType model = dataModel as EntityType;

            Id = model.Id;
            Name = model.Name;
            Description = model.Description;

            TypeLabelAttribute att = Attribute.GetCustomAttribute(model.GetType(), typeof(TypeLabelAttribute)) as TypeLabelAttribute;
            TypeLabel = att == null ? model.GetType().ToString() : att.Name;

            //populating the available metadata sets array
            MetadataService srv = new MetadataService(db);
            var metadataSets = srv.GetMetadataSets();
            AvailableMetadataSets.Add(new MetadataSetListItem(0, ""));
            foreach (var ms in metadataSets)
            {
                if (!string.IsNullOrEmpty(ms.Name))
                    AvailableMetadataSets.Add(new MetadataSetListItem(ms.Id, ms.Name));
            }

            //populating the associated metadata sets array
            foreach (var ms in model.MetadataSets)
                AssociatedMetadataSets.Add(new MetadataSetListItem(ms.Id, ms.Name));
        }

        public override void UpdateDataModel(object dataModel, CatfishDbContext db)
        {
            EntityType model = dataModel as EntityType;

            model.Name = Name;
            model.Description = Description;

            List<int> dataModelMetadataSetIds = model.MetadataSets.Select(m => m.Id).ToList();
            List<int> viewModelMetadataSetIds = AssociatedMetadataSets.Select(m => m.Id).ToList();

            //Removing metadata sets that are already associated with the data model but not with the view model
            foreach (int id in dataModelMetadataSetIds)
            {
                if (!viewModelMetadataSetIds.Contains(id))
                    model.MetadataSets.Remove(model.MetadataSets.Where(m => m.Id == id).FirstOrDefault());
            }

            //Adding metadata sets that are in the view model but not in the data model to the data model.
            foreach(int id in viewModelMetadataSetIds)
            {
                if(!dataModelMetadataSetIds.Contains(id))
                {
                    MetadataSet ms = db.MetadataSets.Where(s => s.Id == id).FirstOrDefault();
                    model.MetadataSets.Add(ms);
                }
            }
        }
    }

    public class MetadataSetListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MetadataSetListItem()
        {

        }

        public MetadataSetListItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class MetadataFieldMapping
    {
        public int MetadataSetId { get; set; }

        public string MetadataSetName { get; set; }

        public int FieldName { get; set; }
    }

}