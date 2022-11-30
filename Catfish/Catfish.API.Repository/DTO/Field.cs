namespace Catfish.API.Repository.DTO
{
    public class Field
    {
        public  FieldType Type { get; set; }

        public Guid Id { get; set; }

        public TextCollection Title { get; set; }

        public TextCollection Description { get; set; }

        public bool IsRequired { get; set; }

        public bool IsMultiValued { get; set; }

        public int DecimalPoints { get; set; }

        public Option[] options { get; set; }

        public bool AllowCustomOptionValues { get; set; }

        public FileReference[] Files { get; set; }
    }
}
