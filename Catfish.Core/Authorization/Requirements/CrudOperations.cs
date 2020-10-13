using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.Authorization.Requirements
{
    public class CrudOperations
    {
        public static readonly OperationAuthorizationRequirement Create
           = new OperationAuthorizationRequirement() { Name = nameof(Create) };

        public static readonly OperationAuthorizationRequirement Read
            = new OperationAuthorizationRequirement() { Name = nameof(Read) };

        public static readonly OperationAuthorizationRequirement Update
            = new OperationAuthorizationRequirement() { Name = nameof(Update) };

        public static readonly OperationAuthorizationRequirement Delete
            = new OperationAuthorizationRequirement() { Name = nameof(Delete) };
    }
}
