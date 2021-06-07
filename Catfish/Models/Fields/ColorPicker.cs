using Piranha;
using Piranha.Extend;

namespace Catfish.Models.Fields
{
    [FieldType(Name = "ColorPicker", Component = "color-picker")]
    public class ColorPicker : IField
    {
        public string Value { get; set; }

        public string GetTitle()
        {
            return Value;
        }
    }
}
