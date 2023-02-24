namespace Catfish.API.Repository.Models.Workflow
{
    public class Authorization
    {
        public Guid Id { get; set; }
        public Guid CurrentStateId { get; set; }
        public eAuthorizedBy AuthorizedBy { get; set; }
        public Guid? AuthorizedRoleId { get; set; }
        public List<Guid> Users { get; set; }
        public string AuthorizedDomain { get; set; }
        public Guid? AuthorizedFormId { get; set; }
        public Guid? AuthorizedFeildId { get; set; }
        public Guid? AuthorizedMetadataFormId { get; set; }
        public Guid? AuthorizedMetadataFeildId { get; set; }
    }
}
