namespace Catfish.API.Repository.DTO
{
    public class EntitySearchResult
    {
        public IEnumerable<EntityEntry> Result { get; set; } = new List<EntityEntry>();
        public int Offset { get; set; }
        public int  Total { get; set; }
    }
}
