using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class AuthorizationPurpose
    {
    }

    public class View: AuthorizationPurpose { }
    public class Edit : AuthorizationPurpose { }
    public class Comment : AuthorizationPurpose { }
    public class StateChange : AuthorizationPurpose 
    {

    }

}
