using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces
{
    public interface ICatfishWebClient
    {
        public Task<HttpResponseMessage> Get(string url);
    }
}
