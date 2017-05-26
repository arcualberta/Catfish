namespace Catfish.Core.Models.Metadata
{
    public class FieldValue
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int FieldId { get; set; }
        public SimpleField Field { get; set; }

        public int EntityId { get; set; }
        public Entity Entity { get; set; }
    }
}
