namespace Catfish.API.Repository.Models.Form
{
    public class Field
    {
        public  Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public Guid FormId { get; set; }
        public Form? Form { get; set; }
    }
}
