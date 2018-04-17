using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Mocks
{
    class MockEntity : Entity
    {
        public static string TagName
        {
            get
            {
                return "mock-entity";
            }
        }

        public override string GetTagName() { return TagName; }
    }
}
