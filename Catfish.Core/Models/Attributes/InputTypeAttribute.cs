using System;

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
