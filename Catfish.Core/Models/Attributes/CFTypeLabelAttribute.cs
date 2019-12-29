using System;

namespace Catfish.Core.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class CFTypeLabelAttribute : Attribute
    {
        public string Name { get; set; }

        public CFTypeLabelAttribute(string name)
        {
            Name = name;
        }
    }
}