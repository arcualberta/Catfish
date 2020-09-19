using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Authorization
{
    public class CatfishWorkflowAuthorizeAttribute : AuthorizeAttribute
    {
        public CatfishWorkflowAuthorizeAttribute()
            : base()
        {
        }
    }
}
