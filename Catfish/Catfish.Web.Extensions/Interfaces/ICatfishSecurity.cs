using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Interfaces
{
    public interface ICatfishSecurity
    {
        void SetAuthApiRoot(string url);
        void SetJwtprocessor(IJwtProcessor processor);
    }
}
