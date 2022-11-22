using Catfish.API.Repository.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.API.Repository.Tests.TestHelpers
{
    public static class MoqDbHelper
    {
        //public static Mock<RepoDbContext>
        private static Mock<RepoDbContext> CreateEmptyContext()
        {
            Mock<RepoDbContext> context = new Mock<RepoDbContext>(MockBehavior.Strict);


            return context;
        }

    }
}
