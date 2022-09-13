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
        public Guid Id { get; set; }
        public Guid SubjectEntityId { get; set; }
        public Entity SubjectEntity { get; set; }

        public string Predicate { get; set; }

        public Guid ObjectEntityId { get; set; }
        public Entity ObjectEntity { get; set; }

    }
}
