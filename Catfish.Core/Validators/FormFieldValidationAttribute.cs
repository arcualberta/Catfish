using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Validators
{
    public abstract class FormFieldValidationAttribute : ValidationAttribute
    {
        public static Func<string[]> CurrentLanguageCodes { get; set; }

        static FormFieldValidationAttribute()
        {
            CurrentLanguageCodes = () => new string[] { "en" };
        }
    }
}
