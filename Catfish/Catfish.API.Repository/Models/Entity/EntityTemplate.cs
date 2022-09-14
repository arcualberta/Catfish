namespace Catfish.API.Repository.Models.Entities
{
    public class EntityTemplate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Form> Forms { get; set; } = new List<Form>();
    }
}
