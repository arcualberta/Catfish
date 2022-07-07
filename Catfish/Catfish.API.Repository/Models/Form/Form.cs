

namespace Catfish.API.Repository.Models.Form
{
    public class Form
    {
        public Guid Id { get; set; }
        public eStatus Status { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public IList<Field> Fields { get; set; } = new List<Field>();
    }
}
