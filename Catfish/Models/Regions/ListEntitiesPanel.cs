using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piranha.Extend;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Core.Models.Forms;
using Piranha.Models.Manager.PageModels;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;
using System.Web.ModelBinding;

namespace Catfish.Models.Regions
{

    // Used to handle old entries which only contain an id string.
    public class ListEntitiesPanelFieldConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return typeof(string).IsAssignableFrom(sourceType) || typeof(ListEntitiesPanelField).IsAssignableFrom(sourceType);
            
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType().IsAssignableFrom(typeof(string)))
            {
                return new ListEntitiesPanelField() { Id = int.Parse(value as string) };
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return base.IsValid(context, value);
        }
    }

    public class ListEntitiesPanelFieldBinder : System.Web.Mvc.IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            var result = new ListEntitiesPanelField();
            result.Id = int.Parse(bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".Id").AttemptedValue);

            var link = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".Link");
            if(link != null)
            {
                result.Link = link.AttemptedValue;
            }

            return result;
        }
    }

    [ModelBinder(typeof(ListEntitiesPanelFieldBinder))]
    [TypeConverter(typeof(ListEntitiesPanelFieldConverter))]
    public struct ListEntitiesPanelField
    {
        public int Id { get; set; }  // AttributeMapping Id
        public string Link { get; set; } // The guid of the page to link to.
    }

    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "ListEntitiesPanel")]
    [ExportMetadata("Name", "ListEntitiesPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class ListEntitiesPanel : CatfishRegion
    {
        [Display(Name = "Item Per Page")]
        public int ItemPerPage { get; set; }
        
        [Display(Name = "Include Fields")]
        public List<ListEntitiesPanelField> Fields { get; set; }  //contain AttributeMapping Id

        [Display(Name = "Sort By Field")]
        public int SortByField { get; set; }

        [Display(Name = "Entity type filter")]
        public string EntityTypeFilter { get; set; }

        [ScriptIgnore]
        public SelectList EntityTypes { get; set; }

        [ScriptIgnore]
        public SelectList FieldsMapping { get; set; }

        [ScriptIgnore]
        public List<CFEntityTypeAttributeMapping> Mappings { get; set; }

        [ScriptIgnore]
        public SelectList Pages { get; set; }

        public ListEntitiesPanel()
        {
            Fields = new List<ListEntitiesPanelField>();
            Mappings = new List<CFEntityTypeAttributeMapping>();
        }

        public override void InitManager(object model)
        {
            var internalId = Piranha.Config.SiteTree;
            var listModel = ListModel.Get(internalId);

            Pages = new SelectList(listModel.Pages, "Id", "Title");

            EntityTypeService entityTypeSrv = new EntityTypeService(db);
       
            FieldsMapping = new SelectList((entityTypeSrv.GetEntityTypeAttributeMappings()), "Id", "Name");
            if (Fields.Count > 0)
            {
                foreach (var field in Fields)
                {
                    CFEntityTypeAttributeMapping map = entityTypeService.GetEntityTypeAttributeMappingById(field.Id);

                    Mappings.Add(map);
                }
            }

            EntityTypes = new SelectList(entityTypeService.GetEntityTypes(), "Name", "Name");

            base.InitManager(model);
        }

        public override object GetContent(object model)
        {
            foreach (var field in Fields)
            {
                CFEntityTypeAttributeMapping map = entityTypeService.GetEntityTypeAttributeMappingById(field.Id);
                Mappings.Add(map);
            }

            return base.GetContent(model);
        }

        public override void OnManagerSave(object model)
        {
            base.OnManagerSave(model);
        }
    }
}