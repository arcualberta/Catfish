using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Catfish.Areas.Manager.Models
{
    public class FieldDefinitionViewModel
    {
        public ICollection<FieldPropertyViewModel> Properties { get; set; }

        public FieldDefinitionViewModel(Type filedType)
        {
            Properties = new List<FieldPropertyViewModel>();

            PropertyInfo[] properties = filedType.GetProperties();
            foreach(PropertyInfo info in properties)
            {
                DataTypeAttribute dataTypeAtt = info.GetCustomAttribute<DataTypeAttribute>(true);

                FieldPropertyViewModel fi = new FieldPropertyViewModel()
                {
                    Name = info.Name,
                    DataType = info.PropertyType.ToString(),
                    DisplayType = dataTypeAtt == null ? "" : dataTypeAtt.DataType.ToString()
                };

                Properties.Add(fi);
            }
        }
    }
}