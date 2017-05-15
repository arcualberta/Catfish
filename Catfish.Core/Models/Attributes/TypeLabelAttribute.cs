using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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