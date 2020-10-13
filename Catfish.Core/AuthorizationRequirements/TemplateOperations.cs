﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Core.AuthorizationRequirements
{
    public class TemplateOperations : CrudOperations
    {
        public static readonly OperationAuthorizationRequirement Instantiate
            = new OperationAuthorizationRequirement() { Name = nameof(Instantiate) };
    }
}
