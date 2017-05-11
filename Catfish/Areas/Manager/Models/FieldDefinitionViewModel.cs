using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Models
{
    public class FieldDefinitionViewModel
    {
        public string ModelType { get; set; }

        public ICollection<FieldPropertyViewModel> Properties { get; set; }

        public FieldDefinitionViewModel(Type filedType)
        {
            ModelType = filedType.ToString();

            Properties = new List<FieldPropertyViewModel>();
            PropertyInfo[] properties = filedType.GetProperties();
            foreach(PropertyInfo info in properties)
            {
                IgnoreDataMemberAttribute ignoreProperty = info.GetCustomAttribute<IgnoreDataMemberAttribute>(true);
                if (ignoreProperty != null)
                    continue;

                DataTypeAttribute dataTypeAtt = info.GetCustomAttribute<DataTypeAttribute>(true);
                HiddenInputAttribute hiddenInputAtt = info.GetCustomAttribute<HiddenInputAttribute>(true);
                string displayType = "";

                if (hiddenInputAtt != null)
                    displayType = "Hidden";
                else if (dataTypeAtt != null)
                    displayType = dataTypeAtt.DataType.ToString();


                FieldPropertyViewModel fi = new FieldPropertyViewModel()
                {
                    Name = info.Name,
                    DataType = info.PropertyType.ToString(),
                    DisplayType = displayType
                };

                Properties.Add(fi);
            }
        }
    }
}