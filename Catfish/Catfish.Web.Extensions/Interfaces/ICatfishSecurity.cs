using ARC.Security.Lib.Interfaces;


namespace CatfishWebExtensions.Interfaces
{
    public interface ICatfishSecurity
    {
        void SetAuthApiRoot(string url);
        void SetJwtprocessor(IJwtProcessor processor);
    }
}
