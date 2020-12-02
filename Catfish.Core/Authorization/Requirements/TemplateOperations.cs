using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Requirements
{
    public class TemplateOperations : CrudOperations
    {
        public static readonly OperationAuthorizationRequirement Instantiate
            = new OperationAuthorizationRequirement() { Name = nameof(Instantiate) };

        public static readonly OperationAuthorizationRequirement ListInstances
         = new OperationAuthorizationRequirement() { Name = nameof(ListInstances) };
    }
}
