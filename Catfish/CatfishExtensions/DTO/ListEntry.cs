using CatfishExtensions.Constants;


namespace CatfishExtensions.DTO
{
    public class ListEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public eState State { get; set; }
    }
}
