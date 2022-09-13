namespace Catfish.API.Repository.Models.Entities
{
    public class ItemTemplate : EntityTemplate
    {
        public ICollection<Form> DataTemplates { get; set; } = new List<Form>();
    }
}
