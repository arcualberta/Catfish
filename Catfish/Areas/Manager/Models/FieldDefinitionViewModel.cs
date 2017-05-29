using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Metadata;

namespace Catfish.Areas.Manager.Models
{
    public class FieldDefinitionViewModel
    {
        public string ModelType { get; set; }
        public string Label { get; set; }
        public string Template { get; set; }
        //public ICollection<FieldPropertyViewModel> Properties { get; set; }

        public FieldDefinitionViewModel(Type fieldType)
        {
            ModelType = fieldType.ToString();

            //Properties = new List<FieldPropertyViewModel>();
            //IEnumerable<PropertyInfo> properties = filedType.GetProperties()
            //    .Where(p => p.GetCustomAttribute<IgnoreAttribute>() == null);

            TypeLabelAttribute typeLabel = fieldType.GetCustomAttribute<TypeLabelAttribute>(true);
            Label = typeLabel == null ? fieldType.ToString() : typeLabel.Name;
            Template = typeof(OptionsField).IsAssignableFrom(fieldType) 
                ? typeof(OptionsField).ToString() 
                : typeof(SimpleField).ToString();

            //foreach (PropertyInfo info in properties)
            //{

            //    FieldPropertyViewModel fi = new FieldPropertyViewModel()
            //    {
            //        Name = info.Name,
            //        DataType = info.PropertyType.ToString(),
            //        DisplayType = GetDisplayType(info),
            //        IsRequired = info.GetCustomAttribute<RequiredAttribute>(true) != null
            //    };

            //    Properties.Add(fi);
            //}
        }

        //protected string GetDisplayType(PropertyInfo info)
        //{
        //    HiddenInputAttribute hiddenInputAtt = info.GetCustomAttribute<HiddenInputAttribute>(true);
        //    InputTypeAttribute inputTypeAtt = info.GetCustomAttribute<InputTypeAttribute>(true);
        //    DataTypeAttribute dataTypeAtt = info.GetCustomAttribute<DataTypeAttribute>(true);

        //    string displayType = "";

        //    if (hiddenInputAtt != null)
        //        displayType = "Hidden";
        //    else if (inputTypeAtt != null)
        //        displayType = inputTypeAtt.InputType.ToString();
        //    else if (dataTypeAtt != null)
        //        displayType = dataTypeAtt.DataType.ToString();

        //    return displayType;
        //}
    }
}