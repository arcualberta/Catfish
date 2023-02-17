using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Exceptions
{
    public  class CatfishException: SystemException
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public int ErrorCode { get; set; }
        public CatfishException(string message)
            :base(message)
        {
        }

        public CatfishException(string message, HttpStatusCode code)
           : base(message)
        {
            HttpStatusCode = code;
        }
    }
}
