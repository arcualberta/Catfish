using Piranha;
using Piranha.Extend;
namespace Catfish.Models.Fields
{
    [FieldType(Name = "TextArea", Component = "textarea-field")]
    public class TextAreaField : IField
    {
        public string Value { get; set; }

        public string GetTitle()
        {
            return Value;
        }
    }
}
