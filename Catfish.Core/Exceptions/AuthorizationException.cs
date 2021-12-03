using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string message) : base(message)
        {
        }
    }
}
