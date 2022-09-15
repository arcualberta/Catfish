namespace Catfish.API.Repository.Models.Entities
{
    public static class RelationshipType
    {
        public static readonly string Child = "Child";
        public static readonly string RelatedItem = "RelatedItem";
        public static readonly string RelatedCollection = "RelatedCollection";
    }

    public class Relationship
    {
        [Key]
        public Guid SubjectEntityId { get; set; }
        public virtual Entity SubjectEntity { get; set; }

        public Guid ObjectEntityId { get; set; }
        public virtual Entity ObjectEntity { get; set; }

        public string Name { get; set; }
        public int Order { get; set; }
    }


}
