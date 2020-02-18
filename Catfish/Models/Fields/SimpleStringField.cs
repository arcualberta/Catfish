using Piranha;
using Piranha.Extend;
namespace Catfish.Models.Fields
{
    [FieldType(Name = "Simple String", Component = "simple-field")]
    public class SimpleStringField : IField
    {
        public string Value { get; set; }

        public string GetTitle()
        {
            throw new System.NotImplementedException();
        }
    }
}
