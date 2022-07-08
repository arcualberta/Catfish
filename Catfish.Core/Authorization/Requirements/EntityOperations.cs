using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Requirements
{
    public class EntityOperations : CrudOperations
    {
        public static readonly OperationAuthorizationRequirement AppendChild
            = new OperationAuthorizationRequirement() { Name = nameof(AppendChild) };

        public static readonly OperationAuthorizationRequirement RemoveChild
            = new OperationAuthorizationRequirement() { Name = nameof(RemoveChild) };
    }
}
