using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Authorization.Requirements
{
    public class GroupOperations : CrudOperations
    {
        public static readonly OperationAuthorizationRequirement ReadSecurePost
            = new OperationAuthorizationRequirement() { Name = nameof(ReadSecurePost) };
        public static readonly OperationAuthorizationRequirement UpdateGroup
            = new OperationAuthorizationRequirement() { Name = nameof(UpdateGroup) };
        public static readonly OperationAuthorizationRequirement AdminDeleteControl
            = new OperationAuthorizationRequirement() { Name = nameof(AdminDeleteControl) };

    }
}
