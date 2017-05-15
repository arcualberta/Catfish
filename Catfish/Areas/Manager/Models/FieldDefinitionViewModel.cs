using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models.Attributes;

namespace Catfish.Areas.Manager.Models
{
    public class FieldDefinitionViewModel
    {
        public string ModelType { get; set; }
        public string TypeLabel { get; set; }

        public ICollection<FieldPropertyViewModel> Properties { get; set; }

        public FieldDefinitionViewModel(Type filedType)
        {
            ModelType = filedType.ToString();

            Properties = new List<FieldPropertyViewModel>();
            IEnumerable<PropertyInfo> properties = filedType.GetProperties()
                .Where(p => p.GetCustomAttribute<IgnoreAttribute>() == null);

            TypeLabelAttribute typeLabel = filedType.GetCustomAttribute<TypeLabelAttribute>(true);
            TypeLabel = typeLabel == null ? filedType.ToString() : typeLabel.Name;

            foreach (PropertyInfo info in properties)
            {

                FieldPropertyViewModel fi = new FieldPropertyViewModel()
                {
                    Name = info.Name,
                    DataType = info.PropertyType.ToString(),
                    DisplayType = GetDisplayType(info),
                    IsRequired = info.GetCustomAttribute<RequiredAttribute>(true) != null
                };

                Properties.Add(fi);
            }
        }

        protected string GetDisplayType(PropertyInfo info)
        {
            HiddenInputAttribute hiddenInputAtt = info.GetCustomAttribute<HiddenInputAttribute>(true);
            InputTypeAttribute inputTypeAtt = info.GetCustomAttribute<InputTypeAttribute>(true);
            DataTypeAttribute dataTypeAtt = info.GetCustomAttribute<DataTypeAttribute>(true);

            string displayType = "";

            if (hiddenInputAtt != null)
                displayType = "Hidden";
            else if (inputTypeAtt != null)
                displayType = inputTypeAtt.InputType.ToString();
            else if (dataTypeAtt != null)
                displayType = dataTypeAtt.DataType.ToString();

            return displayType;
        }
    }
}