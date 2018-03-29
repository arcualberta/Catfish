using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using static Catfish.Core.Models.EntityType;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityTypeViewModel : KoBaseViewModel
    {
       // public enum eMappingType { NameMapping = 1, DescriptionMapping }

        public string TypeLabel { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        //public string TargetType { get; set; }
        public IList<bool> TargetType { get; set; } //MR: jan 15 change to list<> so it could be apply to more than 1 entity (ie: collection, item, etc)
        public List<MetadataSetListItem> AvailableMetadataSets { get; set; }
        public MetadataSetListItem SelectedMetadataSets { get; set; }
        public List<MetadataSetListItem> AssociatedMetadataSets { get; set; }

        public Dictionary<string, List<string>> MetadataSetFields { get; set; }
      

        public List<AttributeMapping> AttributeMappings { get; set; }
      
        public string ErrorMessage { get; set; }
        public EntityTypeViewModel()
        {
            AvailableMetadataSets = new List<MetadataSetListItem>();
            AssociatedMetadataSets = new List<MetadataSetListItem>();
            AssociatedMetadataSets.Add(new MetadataSetListItem(0, ""));

            SelectedMetadataSets = null;
            MetadataSetFields = new Dictionary<string, List<string>>();

            TargetType = new List<bool>();

            foreach (var key in System.Enum.GetValues(typeof(EntityType.eTarget)))
            {
                TargetType.Add(false);
            }

            AttributeMappings = new List<AttributeMapping>(); //MR
            Name = "";
            ErrorMessage = "*";
        }

        public void UpdateViewModel(object dataModel, CatfishDbContext db)
        {
            EntityType model = dataModel as EntityType;

            Id = model.Id;
            Name = model.Name;
            if(!string.IsNullOrEmpty(Name))
            { ErrorMessage = ""; }
            Description = model.Description;
            // TargetType = model.TargetType.ToString();

            foreach (var tt in model.TargetTypesList)  //MR jan 15 2018
            {
                TargetType[(int)tt] = true;
            }

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


            //melania

            if (model.AttributeMappings.Count > 0)
            {

                foreach (EntityTypeAttributeMapping map in model.AttributeMappings)
                {
                    List<string> addList = new List<string>();
                    addList.Add("");
                    addList = addList.Concat((map.MetadataSet.Fields.Select(f => f.Name).ToList())).ToList();
                    if (!MetadataSetFields.ContainsKey(map.MetadataSetId.ToString()))
                    {
                        MetadataSetFields.Add(map.MetadataSetId.ToString(), addList);
                    }
                    
                    if(map.Name.Equals("Name Mapping") || map.Name.Equals("Description Mapping"))
                    {
                        map.Deletable = false;
                    }

                    AttributeMappings.Add(new AttributeMapping
                    {
                        Name = map.Name,
                        Field = map.FieldName,
                        MetadataSetFieldId = map.MetadataSetId,
                        Label = map.Label,
                       Deletable = map.Deletable
                    });
                }
            }
            else
            {
                AttributeMappings.Clear();
                
                AttributeMappings.Add(new AttributeMapping { Name = "Name Mapping", Deletable=false });     
                AttributeMappings.Add(new AttributeMapping { Name = "Description Mapping", Deletable=false});
            }
            
        }

        public override void UpdateDataModel(object dataModel, CatfishDbContext db)
        {
            EntityType model = dataModel as EntityType;

            model.Name = Name;
            model.Description = Description;
           
            //Mr jan 15 2018

            var TargetTypesList = new List<EntityType.eTarget>();
            for (int i = 0; i < TargetType.Count; ++i)
            {
                if (TargetType[i])
                {
                    TargetTypesList.Add((EntityType.eTarget)i);
                }
            }
            model.TargetTypesList = TargetTypesList;

            List<int> dataModelMetadataSetIds = model.MetadataSets.Select(m => m.Id).ToList();
            List<int> viewModelMetadataSetIds = AssociatedMetadataSets.Select(m => m.Id).ToList();

            //Removing metadata sets that are already associated with the data model but not with the view model
            foreach (int id in dataModelMetadataSetIds)
            {
                if (!viewModelMetadataSetIds.Contains(id))
                    model.MetadataSets.Remove(model.MetadataSets.Where(m => m.Id == id).FirstOrDefault());
            }

            //Adding metadata sets that are in the view model but not in the data model to the data model.
            foreach (int id in viewModelMetadataSetIds)
            {
                if (!dataModelMetadataSetIds.Contains(id))
                {
                    MetadataSet ms = db.MetadataSets.Where(s => s.Id == id).FirstOrDefault();
                    if(ms != null)
                        model.MetadataSets.Add(ms);
                }
            }

            //mr March 20 2018
            model.AttributeMappings.Clear();
            MetadataService mService = new MetadataService(db);
            foreach(var map in AttributeMappings)
            {
                model.AttributeMappings.Add( new EntityTypeAttributeMapping
                                                    {
                                                        Name = map.Name,
                                                        FieldName = map.Field,
                                                        MetadataSetId = map.MetadataSetFieldId,
                                                        Label = map.Label,
                                                        Deletable = map.Deletable
                                                    }                       
                                            );
            }
        }

       
    }

    public class MetadataSetListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

      
        public MetadataSetListItem()
        {
            Id = 0;
            Name = "";
          
        }
        public MetadataSetListItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
       
    }

  
    //mr
    public class AttributeMapping
    {
        [Required]
        public int MetadataSetFieldId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Field { get; set; }

        public string Label { get; set; }

        public bool Deletable { get; set; }

        public string ErrorMessage { get; set; }
        public AttributeMapping(){
            Deletable = false;
            Name = "";
            Field = "";
            ErrorMessage = "*";
         }
      
    }

}