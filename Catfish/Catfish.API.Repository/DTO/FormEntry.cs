namespace Catfish.API.Repository.DTO
{
    public class FormEntry 
    {
       public Guid Id { get; set; }
        public Guid FormId { get; set; }
         public string Name { get; set; }
        public bool IsRequired{ get; set; }
    }
}
