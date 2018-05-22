using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    [Serializable]
    public abstract class AbstractForm : CFXmlModel
    {
        public AbstractForm()
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

       

        [NotMapped]
        [DataType(DataType.MultilineText)]
        [IgnoreDataMember]
        public string Description { get { return GetDescription(); } set { SetDescription(value); } }


        public override void UpdateValues(CFXmlModel src)
        {
            base.UpdateValues(src);

            var src_item = src as AbstractForm;

            foreach (FormField field in this.Fields)
            { // checkhere type of 
                var src_field = src_item.Fields.Where(x => x.Guid == field.Guid).FirstOrDefault();
                if (src_field != null)
                    field.UpdateValues(src_field);
            }
        }
        public void SetFieldValue(string fieldName, string fieldValue, string language)
        {
            SetFieldValue(fieldName, new List<string> { fieldValue }, language);
        }

        public void SetFieldValue(string fieldName, IEnumerable<string> fieldValues, string language)
        {
            FormField field = Fields.Where(f => f.Name == fieldName).FirstOrDefault();
            field.SetValues(fieldValues, language);
        }

    }
}
