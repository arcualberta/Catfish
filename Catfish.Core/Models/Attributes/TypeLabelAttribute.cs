using System;

namespace Catfish.Core.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class TypeLabelAttribute : Attribute
    {
        public string Name { get; set; }

        public TypeLabelAttribute(string name)
        {
            Name = name;
        }
    }
}