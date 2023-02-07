namespace Catfish.API.Repository.Models.Workflow
{
    public class Recipient
    {
        public Guid Id { get; set; }
        public eEmailType EmailType { get; set; }
        public eRecipientType RecipientType { get; set; }
        public Guid? RoleId { get; set; }
        public string Email { get; set; }
        public Guid? FormId { get; set; }
        public Guid? FeildId { get; set; }
        public Guid? MetadataFormId { get; set; }
        public Guid? MetadataFeildId { get; set; }
    }
}
