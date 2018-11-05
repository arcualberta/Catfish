using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Composite form field")]
    public class CompositeFormField : FormField
    {
        public CompositeFormField()
        {
            Data.Add(new XElement("fields"));
        }

        [NotMapped]
        [IgnoreDataMember]
        public IReadOnlyList<FormField> Fields
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
