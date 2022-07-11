using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Exceptions
{
    public  class CatfishException: SystemException
    {
        public CatfishException(string message)
            :base(message)
        {
        }
    }
}
