using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Validators
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FormFieldRequiredAttribute : FormFieldValidationAttribute
    {

        public override bool IsValid(object value)
        {
            FormField field = value as FormField;

            if (field.IsRequired)
            {
                foreach (string code in CurrentLanguageCodes())
                {
                    var fieldValue = field.Values.Where(f => f.LanguageCode == code).FirstOrDefault();
                    if (fieldValue == null || string.IsNullOrWhiteSpace(fieldValue.Value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            // return string.Format(Catfish.Core.Resources.Validators.FormFieldRequiredAttribute, name);
            return Catfish.Core.Resources.Validators.FormFieldRequiredAttribute;
        }
    }
}
