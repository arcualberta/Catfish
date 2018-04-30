using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Mocks
{
    class MockEntity : CFEntity
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
