using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Mocks
{
    class MockCFXmlModel : CFXmlModel
    {
        public static string TagName
        {
            get
            {
                return "mock-cfxmlmodel";
            }
        }

        public override string GetTagName() { return TagName; }
    }
}
