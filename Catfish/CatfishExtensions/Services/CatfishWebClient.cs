


using Newtonsoft.Json;
using System.Net.Http;

namespace CatfishExtensions.Services
{
    public class CatfishWebClient : ICatfishWebClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public async Task<HttpResponseMessage> Get(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> Get(string url, string jwtBearerToken)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtBearerToken}");
            var response = await _httpClient.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> PostJson(string url, object payload)
        {
            var payloadString = JsonConvert.SerializeObject(payload);
            HttpContent content = new StringContent(payloadString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            return response;
        }

    }
}
