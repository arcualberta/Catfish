namespace Catfish.API.Authorization.Models
{
    public class LoginResult
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool Success { get; set; } = false;
    }
}
