namespace CatfishExtensions.Interfaces
{
    public interface IGoogleIdentity
    {
        public Task<LoginResult> GetUserLoginResult(string jwt);
      //  public string GenerateJSonWebToken(LoginResult userInfo);

        
    }
}
