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
using System.ComponentModel;
using System.Globalization;

namespace Catfish.Models.Regions
{
    // Used to handle old entries which only contain an id string.
    public class AdvancedSearchContainerFieldConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return typeof(string).IsAssignableFrom(sourceType) || typeof(ListEntitiesPanelField).IsAssignableFrom(sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType().IsAssignableFrom(typeof(string)))
            {
                return new AdvancedSearchContainerField() { Id = int.Parse(value as string) };
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

    public class AdvancedSearchContainerFieldBinder : System.Web.Mvc.IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext)
        {
            var result = new AdvancedSearchContainerField();
            result.Id = int.Parse(bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".Id").AttemptedValue);

            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".IsMultiple");
            if (value != null)
            {
                result.IsMultiple = bool.Parse(((IEnumerable<string>)value.RawValue).FirstOrDefault());
            }

            value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".IsAutoComplete");
            if (value != null)
            {
                result.IsAutoComplete = bool.Parse(((IEnumerable<string>)value.RawValue).FirstOrDefault());
            }

            return result;
        }
    }

    [ModelBinder(typeof(AdvancedSearchContainerFieldBinder))]
    [TypeConverter(typeof(AdvancedSearchContainerFieldConverter))]
    public struct AdvancedSearchContainerField
    {
        public int Id { get; set; }

        [Display(Name = "Multiple Select")]
        public bool IsMultiple { get; set; }

        [Display(Name = "Autocomplete")]
        public bool IsAutoComplete { get; set; }
    }

    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "AdvanceSearchContainer")]
    [ExportMetadata("Name", "AdvanceSearchContainer")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class AdvanceSearchContainer : CatfishRegion
    {
        
       
        [Display(Name = "Include Fields")]
        public List<AdvancedSearchContainerField> Fields { get; set; }  //contain AttributeMapping Id
        
        [ScriptIgnore]
        public SelectList FieldsMapping { get; set; }

        [ScriptIgnore]
        public List<CFEntityTypeAttributeMapping> Mappings { get; set; }

        [ScriptIgnore]
        public List<CFItem> Items { get; set; }
        
        public AdvanceSearchContainer()
        {
           
            Fields = new List<AdvancedSearchContainerField>();
         
            Mappings = new List<CFEntityTypeAttributeMapping>();
        }
        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
          
            EntityTypeService entityTypeSrv = new EntityTypeService(db);

            FieldsMapping = new SelectList((entityTypeSrv.GetEntityTypeAttributeMappings()), "Id", "Name");
            
            if (Fields.Count > 0)
            {
                foreach(var field in Fields)
                {
                    CFEntityTypeAttributeMapping map = entityTypeService.GetEntityTypeAttributeMappingById(field.Id);
                    Mappings.Add(map);

                }
            }

            base.InitManager(model);
        }
        
        public override object GetContent(object model)
        {
            if (Fields.Count > 0)
            {
                //For testing -- go to the page that use this region and add ?entity=[entityId]
                HttpContext context = HttpContext.Current;


                //grab the columnHeaders
                foreach (var field in Fields)
                {
                    CFEntityTypeAttributeMapping map = entityTypeService.GetEntityTypeAttributeMappingById(field.Id);

                    Mappings.Add(map);
                }
            }

            return base.GetContent(model);
        }


    }
}