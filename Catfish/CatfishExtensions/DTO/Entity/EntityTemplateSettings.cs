using CatfishExtensions.DTO.Forms;


namespace CatfishExtensions.DTO.Entity
{
    public class EntityTemplateSettings
    {
        public FormEntry[] MetadataForms { get; set; }

        public FormEntry[] DataForms { get; set; }

        public FieldEntry TitleField { get; set; }
        public FieldEntry DescriptionField { get; set; }
        public FieldEntry MediaField { get; set; }

        public Guid PrimaryFormId { get; set; }
    }
}
