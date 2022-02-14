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

        public static new readonly OperationAuthorizationRequirement Read
         = new OperationAuthorizationRequirement() { Name = nameof(Read) };

        public static new readonly OperationAuthorizationRequirement Update
         = new OperationAuthorizationRequirement() { Name = nameof(Update) };

        public static new readonly OperationAuthorizationRequirement ChangeState
         = new OperationAuthorizationRequirement() { Name = nameof(ChangeState) };

        public static new readonly OperationAuthorizationRequirement ChildFormView
         = new OperationAuthorizationRequirement() { Name = nameof(ChildFormView) };

        public static new readonly OperationAuthorizationRequirement DeleteComment
         = new OperationAuthorizationRequirement() { Name = nameof(DeleteComment) };

        public static new readonly OperationAuthorizationRequirement Review
         = new OperationAuthorizationRequirement() { Name = nameof(Review) };
    }
}
