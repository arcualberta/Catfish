using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Catfish.Core.Models.EntityType;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityTypeViewModel : KoBaseViewModel
    {
       // public enum eMappingType { NameMapping = 1, DescriptionMapping }

        public string TypeLabel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public string TargetType { get; set; }
        public IList<bool> TargetType { get; set; } //MR: jan 15 change to list<> so it could be apply to more than 1 entity (ie: collection, item, etc)
        public List<MetadataSetListItem> AvailableMetadataSets { get; set; }
        public MetadataSetListItem SelectedMetadataSets { get; set; }
        public List<MetadataSetListItem> AssociatedMetadataSets { get; set; }

        public Dictionary<string, List<string>> MetadataSetFields { get; set; }
      

        public List<AttributeMapping> AttributeMappings { get; set; }
       // public List<string> AvailableMappingFields { get; set; }

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
        }

        public void UpdateViewModel(object dataModel, CatfishDbContext db)
        {
            EntityType model = dataModel as EntityType;

            Id = model.Id;
            Name = model.Name;
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


                    AttributeMappings.Add(new AttributeMapping
                    {
                        Name = map.Name,
                        Field = map.FieldName,
                        MetadataSetFieldId = map.MetadataSetId
                    });
                }
            }
            else
            {
                AttributeMappings.Clear();
                //add default Name Mapping and DescriptionMapping
                //EntityTypeAttributeMapping entityTypeAttributeMapping = new EntityTypeAttributeMapping();
                //entityTypeAttributeMapping.Name = "Name Mapping";
               // entityTypeAttributeMapping.FieldName = "Not Defined";
                AttributeMappings.Add(new AttributeMapping { Name = "Name Mapping" });

                //  AttributeMappings.Add(new AttributeMapping {EntityTypeAttributeMapping = entityTypeAttributeMapping, Name = entityTypeAttributeMapping.Name});

               // EntityTypeAttributeMapping entityTypeAttributeMapping2 = new EntityTypeAttributeMapping();
               // entityTypeAttributeMapping2.Name = "Description Mapping";
               // entityTypeAttributeMapping2.FieldName = "Not Defined";
                AttributeMappings.Add(new AttributeMapping { Name = "Description Mapping"});

               // AttributeMappings.Add(new AttributeMapping { EntityTypeAttributeMapping = entityTypeAttributeMapping2, Name = entityTypeAttributeMapping2.Name });

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
                                                        MetadataSetId = map.MetadataSetFieldId
                                                    }                       
                                            );
            }
        }

       
    }

    public class MetadataSetListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

      //  public List<string> Fields { get; set; }

        public MetadataSetListItem()
        {
            Id = 0;
            Name = "";
           // Fields = new List<string>();
        }
        public MetadataSetListItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
        //public MetadataSetListItem(int id, string name, List<string> fields)
        //{
        //    Id = id;
        //    Name = name;
        //    Fields = fields;
        //}
    }

    //public class MetadataFieldMapping
    //{
    //    public int MetadataSetId { get; set; }

    //    public string MetadataSet { get; set; }

    //    public string Field { get; set; }

    //    public MetadataFieldMapping()
    //    {
    //        MetadataSet = "Not specified";
    //        Field = "Not specified";
    //    }
    //}

    //mr
    public class AttributeMapping
    {
        //public EntityTypeAttributeMapping EntityTypeAttributeMapping { get; set; }
        //public List<MetadataSetListItem> MetadataFields { get; set; }

        public int MetadataSetFieldId { get; set; }
        public string Name { get; set; }
        public string Field { get; set; }

       // public List<string> Fields { get; set; }

        public bool Deletable { get; set; }

        public AttributeMapping(){
            Deletable = false;
         }
      
    }

}