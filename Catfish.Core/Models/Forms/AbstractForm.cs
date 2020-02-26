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
    public abstract class AbstractForm : XmlModel
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

       

        ////[NotMapped]
        ////[DataType(DataType.MultilineText)]
        ////[IgnoreDataMember]
        ////override public string Description { get { return GetDescription(); } set { SetDescription(value); } }


        public override void UpdateValues(XmlModel src)
        {
            base.UpdateValues(src);

            var src_item = src as AbstractForm;
            UpdateValueFields(src_item.Fields.ToList());
        }

        /// <summary>
        /// Helper method that assigns the abstract form's fields directly from a list.
        /// The inpuit parameter formFields Guid's need to match to the abstract form's
        /// Guid in order to assign them.
        /// </summary>
        /// <param name="formFields"></param>

        public void UpdateValueFields(List<FormField> formFields)
        {
            foreach (FormField field in this.Fields)
            { // checkhere type of 
                var src_field = formFields.Where(x => x.Guid == field.Guid).FirstOrDefault();
                if (src_field != null)
                    field.UpdateValues(src_field);
            }
        }
        public void SetFieldValue(string fieldName, string fieldValue, string language, bool removePrevious=false)
        {
            SetFieldValue(fieldName, new List<string> { fieldValue }, language, removePrevious);
        }

        public void SetFieldValue(string fieldName, IEnumerable<string> fieldValues, string language, bool removePrevious=false)
        {
            FormField field = Fields.Where(f => f.GetName() == fieldName).FirstOrDefault();
            field.SetValues(fieldValues, language, removePrevious);
        }

        [NotMapped]
        [TypeLabel("String")]
        [IgnoreDataMember]
        public string ReferenceCode { get { return GetAttribute("reference-code", null); } set { SetAttribute("reference-code", value); } }


    }
}
