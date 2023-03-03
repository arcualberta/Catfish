using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces
{
    public interface ICatfishWebClient
    {
        Task<HttpResponseMessage> Get(string url, string? jwtBearerToken = null);
        Task<T> Get<T>(string url, string? jwtBearerToken = null);
        Task<HttpResponseMessage> PostJson(string url, object payload, string? jwtBearerToken = null);
        Task<T> PostJson<T>(string url, object payload, string? jwtBearerToken = null);
        Task<HttpResponseMessage> PatchJson(string url, object payload, string? jwtBearerToken = null);
        Task<T> PatchJson<T>(string url, object payload, string? jwtBearerToken = null);
        Task<T> PutJson<T>(string url, object payload, string? jwtBearerToken = null);
        Task<HttpResponseMessage> PutJson(string url, object payload, string? jwtBearerToken = null);
        Task<HttpResponseMessage> Delete(string url, string? jwtBearerToken = null);
    }
}
