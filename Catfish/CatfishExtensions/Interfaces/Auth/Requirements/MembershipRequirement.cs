﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Interfaces.Auth.Requirements
{
    public class MembershipRequirement : IAuthorizationRequirement
    {
        public MembershipRequirement()
        {
        }
    }
}
