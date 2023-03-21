using CatfishExtensions.DTO;

namespace Catfish.API.Repository.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Email email);
    }
    
}
