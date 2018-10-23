using Catfish.Core.Contexts;
using Catfish.Core.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Helpers
{
    public abstract class BaseUnitTest
    {
        [SetUp]
        public virtual void SetUp()
        {
            OnSetup();
        }

        [TearDown]
        public virtual void TearDown()
        {
            OnTearDown();
        }

        protected abstract void OnSetup();

        protected abstract void OnTearDown();
    }    
}
