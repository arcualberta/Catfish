using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Catfish.Areas.Manager.Services;
using System.Configuration;

namespace Catfish.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private DatabaseHelper mDh { get; set; }
        private UserService userService;

        [TestInitialize]
        public void InitializeTesting()
        {
            mDh = new DatabaseHelper(true);
            var pdb = mDh.PDb;
            userService = new UserService();
        }

        [TestMethod]
        public void CanGetUserByLogin()
        {
            Piranha.Entities.User user = userService.GetUserByLogin("sys");
           
             Assert.AreEqual("sys", user.Login);
        }

        [TestMethod]
        public void CanGetUserByEmail()
        {
            Piranha.Entities.User user = userService.GetUserByEmail("admin@ualberta.ca");
            Assert.AreEqual("admin@ualberta.ca", user.Email);
        }
        
        [TestMethod]
        public void CanGetListUsers()
        {
           var users = userService.GetAllUsers();
            Assert.AreEqual(2, users.Count());
        }
    }
}
