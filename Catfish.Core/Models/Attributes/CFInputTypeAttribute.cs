using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CFInputTypeAttribute : Attribute
    {
        public enum eInputType { StringArray }

        public eInputType InputType { get;set; }

        public CFInputTypeAttribute(eInputType inputType)
        {
            InputType = inputType;
        }
    }
}
