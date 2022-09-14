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

        public string Name { get; set; }

        //public ICollection<Item> ObjectItems { get; set; } = new List<Item>();

        public Guid SubjectEntityId { get; set; }
        public Entity SubjectEntity { get; set; }
        public int Order { get; set; }
        public Guid ObjectEntityId { get; set; }
        public Entity ObjectEntity { get; set; }

        ////public ICollection<Collection> Collections { get; set; } = new List<Collection>();

        ////[Column(Order = 0)]
        ////public int SubjectItemId { get; set; }

        ////public virtual Item SubjectItem { get; set; }


        ////[Column(Order = 1)]
        ////public int ObjectItemId { get; set; }
        ////public virtual Item ObjectItem { get; set; }


        ////[Column(Order = 2)]
        ////public int SubjectCollectionId { get; set; }
        ////public virtual Collection SubjectCollection { get; set; }


        ////[Column(Order = 3)]
        ////public int ObjectCollectionId { get; set; }
        ////public virtual Collection ObjectCollection { get; set; }



        ////[Column(Order = 4)]
        ////public string Name { get; set; }
    }

    public class SubjectRelationship : Relationship { }
    public class ObjectRelationship : Relationship { }
}
