using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Helpers
{
    abstract class BaseIntegrationTest
    {
        [SetUp]
        public void SetUp()
        {
            // Remove Database

            // Create Admin Through Login
            
            OnSetup();
        }

        [TearDown]
        public void TearDown()
        {
            // Delete the database
            OnTearDown();
        }

        protected abstract void OnSetup();

        protected abstract void OnTearDown();

        private void DeleteDatabase()
        {

        }

        private void SetupPiranha()
        {

        }
    }
}
