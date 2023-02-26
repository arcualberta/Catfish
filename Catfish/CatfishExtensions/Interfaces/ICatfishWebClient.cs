using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces
{
    public interface ICatfishWebClient
    {
        Task<HttpResponseMessage> Get(string url);
        Task<HttpResponseMessage> Get(string url, string jwtBearerToken);
        Task<HttpResponseMessage> PostJson(string url, object payload);

    }
}
