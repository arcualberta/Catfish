using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catfish.API.Repository;

namespace Catfish.API.Repository.Tests.UnitTests
{
    public class EntityServiceTests
    {
        public readonly Mock<RepoDbContext> _dbContextMock = new();
    }
}
