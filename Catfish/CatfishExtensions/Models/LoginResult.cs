namespace CatfishExtensions.Models
{
    public class LoginResult
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public IList<string> GlobalRoles { get; set; } = new List<string>();
        public bool Success { get; set; } = false;
    }
}
