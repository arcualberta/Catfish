using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Piranha.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Services
{
    [TestClass]
    public class SecurityServiceBaseTest
    {
        private DatabaseHelper mDh { get; set; }
        private CatfishDbContext Db {
            get
            {
                return mDh.Db;
            }
        }

        private List<TestUser> Users { get; set; }

        private TestUser CreateUser(SecurityServiceBase srv, string username, bool isAdmin = false)
        {
            return ((TestSecurityService)srv).AddNewUser(username, isAdmin);
        }

        private AccessMode GetDefaultPermissions()
        {
            int permissions;

            if (int.TryParse(ConfigHelper.GlobalAccessModes, out permissions))
            {
                return (AccessMode)permissions;
            }

            return AccessMode.None;
        }

        [TestInitialize]
        public void Initialize()
        {
            mDh = new DatabaseHelper(true);
            SecurityServiceBase srv = new TestSecurityService(Db);

            Users = new List<TestUser>();

            // Create the users
            Users.Add(CreateUser(srv, "U1"));
            Users.Add(CreateUser(srv, "U2"));
            Users.Add(CreateUser(srv, "U3"));
            Users.Add(CreateUser(srv, "U4"));

            // Create the groups

        }

        [TestMethod]
        public void TestUserIsAdmin()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);
            TestUser admin = CreateUser(srv, "A1", true);
            TestUser user = CreateUser(srv, "U1");
            string collectionName = "TestUserIsAdminCollection";
            string collectionDesciption = "Test collection description";
            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";

            AccessMode defaultAccess = GetDefaultPermissions();

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c1 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);

            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);

            Db.SaveChanges();

            AccessMode modes = srv.GetPermissions(admin.Guid, c1);
            Assert.AreNotEqual(defaultAccess, modes);
            Assert.AreEqual(AccessMode.All, modes);

            modes = srv.GetPermissions(user.Guid, c1);
            Assert.AreEqual(defaultAccess, modes);
            Assert.AreNotEqual(AccessMode.All, modes);

            modes = srv.GetPermissions(admin.Guid, i1);
            Assert.AreNotEqual(defaultAccess, modes);
            Assert.AreEqual(AccessMode.All, modes);

            modes = srv.GetPermissions(user.Guid, i1);
            Assert.AreEqual(defaultAccess, modes);
            Assert.AreNotEqual(AccessMode.All, modes);
        }

        [TestMethod]
        public void TestNoAccessDefinitionNoParents()
        {

        }

        [TestMethod]
        public void TestNoAccessDefinitionWithParents()
        {

        }

        [TestMethod]
        public void TestHasAccessDefinitionNoParents()
        {

        }

        [TestMethod]
        public void TestHasAccessDefinitionHasParent()
        {

        }

        [TestMethod]
        public void TestHasAccessDefinitionHasParents()
        {

        }

        [TestMethod]
        public void TestDenyInheritanceFlag()
        {

        }

        [TestMethod]
        public void TestDenyInheritanceFlagOnSingleParent()
        {

        }

        [TestMethod]
        public void TestDenyInheritanceFlagOnMultipleParents()
        {

        }
    }

    struct TestUser
    {
        public string Guid;
        public string UserName;
        public bool IsAdmin;
    }

    class TestSecurityService : SecurityServiceBase
    {
        private Dictionary<string, TestUser> Users { get; set; }

        public TestSecurityService(CatfishDbContext db) : base(db)
        {
            Users = new Dictionary<string, TestUser>();
        }

        public TestUser AddNewUser(string name, bool isAdmin)
        {
            TestUser user = new TestUser() { Guid = Guid.NewGuid().ToString(), UserName = name, IsAdmin = isAdmin };
            AddUser(user);

            return user;
        }

        public void AddUser(TestUser user)
        {
            Users.Add(user.Guid, user);
        }

        protected override bool IsAdmin(string userGuid)
        {
            if (Users.ContainsKey(userGuid))
            {
                return Users[userGuid].IsAdmin;
            }

            return false;
        }
    }
}
