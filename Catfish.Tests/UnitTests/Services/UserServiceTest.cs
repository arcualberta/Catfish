using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catfish.Areas.Manager.Services;
using System.Configuration;
using Catfish.Services;

namespace Catfish.Tests.Services
{
    [TestFixture]
    public class UserServiceTest : BaseServiceTest
    {
        private DatabaseHelper mDh { get; set; }
        private UserService userService;

        protected override void OnSetup()
        {
            mDh = new DatabaseHelper(true);
            var pdb = mDh.PDb;
            userService = new UserService();
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanGetUserByLogin()
        {
            Piranha.Entities.User user = userService.GetUserByLogin("sys");
           
             Assert.AreEqual("sys", user.Login);
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanGetUserByEmail()
        {
            Piranha.Entities.User user = userService.GetUserByEmail("admin@ualberta.ca");
            Assert.AreEqual("admin@ualberta.ca", user.Email);
        }
    }
}
