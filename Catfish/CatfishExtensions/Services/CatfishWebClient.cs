


using Newtonsoft.Json;
using System.Net.Http;

namespace CatfishExtensions.Services
{
    public class CatfishWebClient : ICatfishWebClient
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<HttpResponseMessage> Get(string url, string? jwtBearerToken = null)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrEmpty(jwtBearerToken))
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtBearerToken}");

            var response = await _httpClient.GetAsync(url);
            return response;
        }

        public async Task<T> Get<T>(string url, string? jwtBearerToken = null)
        {
            var response = await Get(url, jwtBearerToken);
            string responseString = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "";
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public async Task<HttpResponseMessage> PostJson(string url, object payload, string? jwtBearerToken = null)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrEmpty(jwtBearerToken))
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtBearerToken}");

            var payloadString = JsonConvert.SerializeObject(payload);
            HttpContent content = new StringContent(payloadString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);

            return response;
        }

        public async Task<T> PostJson<T>(string url, object payload, string? jwtBearerToken = null)
        {
            var response = await PostJson(url, payload, jwtBearerToken);
            string responseString = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "";
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        public async Task<HttpResponseMessage> PatchJson(string url, object payload, string? jwtBearerToken = null)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrEmpty(jwtBearerToken))
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtBearerToken}");

            var payloadString = JsonConvert.SerializeObject(payload);
            HttpContent content = new StringContent(payloadString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, content);
            return response;
        }
        public async Task<T> PatchJson<T>(string url, object payload, string? jwtBearerToken = null)
        {
            var response = await PatchJson(url, payload, jwtBearerToken);
            string responseString = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "";
            return JsonConvert.DeserializeObject<T>(responseString);
        }

    }
}
