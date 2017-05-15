using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InputTypeAttribute : Attribute
    {
        public enum eInputType { StringArray }

        public eInputType InputType { get;set; }

        public InputTypeAttribute(eInputType inputType)
        {
            InputType = inputType;
        }
    }
}
