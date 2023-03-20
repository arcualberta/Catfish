

namespace CatfishExtensions.DTO.Entity
{
    public class EntityEntry : ListEntry
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
