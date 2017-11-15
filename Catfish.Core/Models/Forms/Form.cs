using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Forms
{
    public class Form : XmlModel
    {
        [NotMapped]
        public List<FormField> Fields
        {
            get
            {
                return GetChildModels("fields/field", Data).Select(c => c as FormField).ToList();
            }

            set
            {
                //Removing all children inside the metadata set element
                RemoveAllElements("fields/field", Data);

                foreach (FormField ms in value)
                    InsertChildElement("./fields", ms.Data);
            }
        }
    }
}
