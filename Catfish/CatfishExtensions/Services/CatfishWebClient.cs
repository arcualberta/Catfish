


namespace CatfishExtensions.Services
{
    public class CatfishWebClient : ICatfishWebClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public Task<HttpResponseMessage> Get(string url)
        {
            var response = _httpClient.GetAsync(url);
            return response;
        }
    }
}
