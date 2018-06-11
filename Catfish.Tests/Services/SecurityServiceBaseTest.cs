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

/*
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
        private List<string> Groups { get; set; }

        private TestUser CreateUser(SecurityServiceBase srv, string username, bool isAdmin = false)
        {
            return ((TestSecurityService)srv).AddNewUser(username, isAdmin);
        }

        private AccessMode GetDefaultPermissions()
        {
            int permissions;
            string permString = ConfigHelper.GlobalAccessModes;

            if (int.TryParse(permString, out permissions))
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
            UserListService ulSrv = new UserListService(Db);

            Users = new List<TestUser>();
            Groups = new List<string>();

            // Create the users
            Users.Add(CreateUser(srv, "U1"));
            Users.Add(CreateUser(srv, "U2"));
            Users.Add(CreateUser(srv, "U3"));
            Users.Add(CreateUser(srv, "U4"));
            Db.SaveChanges();

            // Create the groups
            CFUserList ul = new CFUserList() { Name = "G1", Id = Guid.NewGuid() };
            ul.CFUserListEntries.Add(new CFUserListEntry() { UserId = Guid.Parse(Users[0].Guid) });
            ul.CFUserListEntries.Add(new CFUserListEntry() { UserId = Guid.Parse(Users[1].Guid) });
            ulSrv.EditEntityGroup(ul);
            Groups.Add(ul.Id.ToString());

            ul = new CFUserList() { Name = "G2", Id = Guid.NewGuid() };
            ul.CFUserListEntries.Add(new CFUserListEntry() { UserId = Guid.Parse(Users[1].Guid) });
            ul.CFUserListEntries.Add(new CFUserListEntry() { UserId = Guid.Parse(Users[2].Guid) });
            ul.CFUserListEntries.Add(new CFUserListEntry() { UserId = Guid.Parse(Users[3].Guid) });
            ulSrv.EditEntityGroup(ul);
            Groups.Add(ul.Id.ToString());

            ul = new CFUserList() { Name = "G3", Id = Guid.NewGuid() };
            ul.CFUserListEntries.Add(new CFUserListEntry() { UserId = Guid.Parse(Users[0].Guid) });
            ul.CFUserListEntries.Add(new CFUserListEntry() { UserId = Guid.Parse(Users[3].Guid) });
            ulSrv.EditEntityGroup(ul);
            Groups.Add(ul.Id.ToString());

            Db.SaveChanges();

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
            SecurityServiceBase srv = new TestSecurityService(Db);

            string collectionName = "TestUserIsAdminCollection";
            string collectionDesciption = "Test collection description";
            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";

            AccessMode defaultAccess = GetDefaultPermissions();
            AccessMode modes;

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c1 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);

            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName + "1", itemDesciption, true);
            CFItem i2 = mDh.CreateItem(mDh.Is, entityType, itemName + "2", itemDesciption, true);
            
            Db.SaveChanges();

            c1.ChildMembers.Add(i1);
            c1.Serialize();
            Db.Entry(c1).State = System.Data.Entity.EntityState.Modified;

            i2.ChildMembers.Add(i1);
            i2.Serialize();
            Db.Entry(i2).State = System.Data.Entity.EntityState.Modified;

            Db.SaveChanges();

            foreach (TestUser user in Users)
            {
                modes = srv.GetPermissions(user.Guid, c1);

                Assert.AreEqual(defaultAccess, modes);
                Assert.AreNotEqual(AccessMode.All, modes);

                modes = srv.GetPermissions(user.Guid, i2);

                Assert.AreEqual(defaultAccess, modes);
                Assert.AreNotEqual(AccessMode.All, modes);

                modes = srv.GetPermissions(user.Guid, i1);

                Assert.AreEqual(defaultAccess, modes);
                Assert.AreNotEqual(AccessMode.All, modes);
            }
        }

        [TestMethod]
        public void TestNoAccessDefinitionWithParents()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);

            string collectionName = "TestUserIsAdminCollection";
            string collectionDesciption = "Test collection description";
            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";

            AccessMode defaultAccess = GetDefaultPermissions();
            AccessMode modes;

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c1 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);

            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);

            c1.ChildMembers.Add(i1);

            Db.SaveChanges();

            foreach (TestUser user in Users)
            {
                modes = srv.GetPermissions(user.Guid, i1);

                Assert.AreEqual(defaultAccess, modes);
                Assert.AreNotEqual(AccessMode.All, modes);
            }
        }

        [TestMethod]
        public void TestHasAccessDefinitionNoParents()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);

            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";


            AccessMode defaultAccess = GetDefaultPermissions();

            CFAccessDefinition ad1 = new CFAccessDefinition()
            {
                Name = "Test 1",
                AccessModes = AccessMode.Read | AccessMode.Write
            };

            CFAccessDefinition ad2 = new CFAccessDefinition()
            {
                Name = "Test 2",
                AccessModes = AccessMode.Control | AccessMode.Append
            };

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);

            List<CFAccessGroup> groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[0]), AccessDefinition = ad1 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Users[1].Guid), AccessDefinition = ad2 }
            };

            i1.AccessGroups = groups;
            i1.Serialize();
            Db.SaveChanges();
            
            AccessMode modes1 = srv.GetPermissions(Users[0].Guid, i1);
            AccessMode modes2 = srv.GetPermissions(Users[1].Guid, i1);
            AccessMode modes3 = srv.GetPermissions(Users[2].Guid, i1);
            AccessMode modes4 = srv.GetPermissions(Users[3].Guid, i1);

            Assert.AreEqual(defaultAccess | ad1.AccessModes, modes1);
            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad2.AccessModes, modes2);
            Assert.AreEqual(defaultAccess, modes3);
            Assert.AreEqual(defaultAccess, modes4);
        }

        [TestMethod]
        public void TestHasAccessDefinitionHasParent()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);

            string collectionName = "TestUserIsAdminCollection";
            string collectionDesciption = "Test collection description";
            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";

            AccessMode defaultAccess = GetDefaultPermissions();

            CFAccessDefinition ad1 = new CFAccessDefinition()
            {
                Name = "Test 1",
                AccessModes = AccessMode.Write
            };

            CFAccessDefinition ad2 = new CFAccessDefinition()
            {
                Name = "Test 2",
                AccessModes = AccessMode.Control | AccessMode.Append
            };

            CFAccessDefinition ad3 = new CFAccessDefinition()
            {
                Name = "Test 3",
                AccessModes = AccessMode.Read
            };

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c1 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);

            List<CFAccessGroup> groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[0]), AccessDefinition = ad1 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[1]), AccessDefinition = ad2 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[2]), AccessDefinition = ad3 }
            };

            c1.AccessGroups = groups;
            c1.Serialize();
            i1.Serialize();

            c1.ChildMembers.Add(i1);
            Db.SaveChanges();

            AccessMode modes1 = srv.GetPermissions(Users[0].Guid, i1);
            AccessMode modes2 = srv.GetPermissions(Users[1].Guid, i1);
            AccessMode modes3 = srv.GetPermissions(Users[2].Guid, i1);
            AccessMode modes4 = srv.GetPermissions(Users[3].Guid, i1);

            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad3.AccessModes, modes1);
            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad2.AccessModes, modes2);
            Assert.AreEqual(defaultAccess | ad2.AccessModes, modes3);
            Assert.AreEqual(defaultAccess | ad2.AccessModes | ad3.AccessModes, modes4);
        }

        [TestMethod]
        public void TestHasAccessDefinitionHasParents()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);

            string collectionName = "TestUserIsAdminCollection";
            string collectionDesciption = "Test collection description";
            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";

            AccessMode defaultAccess = GetDefaultPermissions();

            CFAccessDefinition ad1 = new CFAccessDefinition()
            {
                Name = "Test 1",
                AccessModes = AccessMode.Write
            };

            CFAccessDefinition ad2 = new CFAccessDefinition()
            {
                Name = "Test 2",
                AccessModes = AccessMode.Control | AccessMode.Append
            };

            CFAccessDefinition ad3 = new CFAccessDefinition()
            {
                Name = "Test 3",
                AccessModes = AccessMode.Read
            };

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c1 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i2 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);

            List<CFAccessGroup> groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[0]), AccessDefinition = ad1 },
            };

            c1.AccessGroups = groups;
            c1.Serialize();

            groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[1]), AccessDefinition = ad2 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[2]), AccessDefinition = ad3 }
            };

            i2.AccessGroups = groups;
            i2.Serialize();

            i1.Serialize();

            c1.ChildMembers.Add(i1);
            i2.ChildMembers.Add(i1);
            Db.SaveChanges();

            AccessMode modes1 = srv.GetPermissions(Users[0].Guid, i1);
            AccessMode modes2 = srv.GetPermissions(Users[1].Guid, i1);
            AccessMode modes3 = srv.GetPermissions(Users[2].Guid, i1);
            AccessMode modes4 = srv.GetPermissions(Users[3].Guid, i1);

            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad3.AccessModes, modes1);
            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad2.AccessModes, modes2);
            Assert.AreEqual(defaultAccess | ad2.AccessModes, modes3);
            Assert.AreEqual(defaultAccess | ad2.AccessModes | ad3.AccessModes, modes4);

            modes1 = srv.GetPermissions(Users[0].Guid, i2);
            modes2 = srv.GetPermissions(Users[1].Guid, i2);
            modes3 = srv.GetPermissions(Users[2].Guid, i2);
            modes4 = srv.GetPermissions(Users[3].Guid, i2);

            Assert.AreEqual(defaultAccess | ad3.AccessModes, modes1);
            Assert.AreEqual(defaultAccess | ad2.AccessModes, modes2);
            Assert.AreEqual(defaultAccess | ad2.AccessModes, modes3);
            Assert.AreEqual(defaultAccess | ad2.AccessModes | ad3.AccessModes, modes4);

            modes1 = srv.GetPermissions(Users[0].Guid, c1);
            modes2 = srv.GetPermissions(Users[1].Guid, c1);
            modes3 = srv.GetPermissions(Users[2].Guid, c1);
            modes4 = srv.GetPermissions(Users[3].Guid, c1);

            Assert.AreEqual(defaultAccess | ad1.AccessModes, modes1);
            Assert.AreEqual(defaultAccess | ad1.AccessModes, modes2);
            Assert.AreEqual(defaultAccess, modes3);
            Assert.AreEqual(defaultAccess, modes4);
        }

        [TestMethod]
        public void TestDenyInheritanceFlag()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);

            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";


            AccessMode defaultAccess = GetDefaultPermissions();

            CFAccessDefinition ad1 = new CFAccessDefinition()
            {
                Name = "Test 1",
                AccessModes = AccessMode.Read | AccessMode.Write
            };

            CFAccessDefinition ad2 = new CFAccessDefinition()
            {
                Name = "Test 2",
                AccessModes = AccessMode.Control | AccessMode.Append
            };

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);

            List<CFAccessGroup> groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[0]), AccessDefinition = ad1 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Users[1].Guid), AccessDefinition = ad2 }
            };

            i1.AccessGroups = groups;
            i1.BlockInheritance = true;
            i1.Serialize();
            Db.SaveChanges();

            AccessMode modes1 = srv.GetPermissions(Users[0].Guid, i1);
            AccessMode modes2 = srv.GetPermissions(Users[1].Guid, i1);
            AccessMode modes3 = srv.GetPermissions(Users[2].Guid, i1);
            AccessMode modes4 = srv.GetPermissions(Users[3].Guid, i1);

            Assert.AreEqual(ad1.AccessModes, modes1);
            Assert.AreEqual(ad1.AccessModes | ad2.AccessModes, modes2);
            Assert.AreEqual(AccessMode.None, modes3);
            Assert.AreEqual(AccessMode.None, modes4);
        }

        [TestMethod]
        public void TestDenyInheritanceFlagOnSingleParent()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);

            string collectionName = "TestUserIsAdminCollection";
            string collectionDesciption = "Test collection description";
            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";

            AccessMode defaultAccess = GetDefaultPermissions();

            CFAccessDefinition ad1 = new CFAccessDefinition()
            {
                Name = "Test 1",
                AccessModes = AccessMode.Write
            };

            CFAccessDefinition ad2 = new CFAccessDefinition()
            {
                Name = "Test 2",
                AccessModes = AccessMode.Control | AccessMode.Append
            };

            CFAccessDefinition ad3 = new CFAccessDefinition()
            {
                Name = "Test 3",
                AccessModes = AccessMode.Read
            };

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c1 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);

            List<CFAccessGroup> groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[0]), AccessDefinition = ad1 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[1]), AccessDefinition = ad2 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[2]), AccessDefinition = ad3 }
            };

            c1.AccessGroups = groups;
            c1.BlockInheritance = true;
            c1.Serialize();
            i1.Serialize();

            c1.ChildMembers.Add(i1);
            Db.SaveChanges();

            AccessMode modes1 = srv.GetPermissions(Users[0].Guid, i1);
            AccessMode modes2 = srv.GetPermissions(Users[1].Guid, i1);
            AccessMode modes3 = srv.GetPermissions(Users[2].Guid, i1);
            AccessMode modes4 = srv.GetPermissions(Users[3].Guid, i1);

            Assert.AreEqual(ad1.AccessModes | ad3.AccessModes, modes1);
            Assert.AreEqual(ad1.AccessModes | ad2.AccessModes, modes2);
            Assert.AreEqual(ad2.AccessModes, modes3);
            Assert.AreEqual(ad2.AccessModes | ad3.AccessModes, modes4);
        }

        [TestMethod]
        public void TestDenyInheritanceFlagOnMultipleParents()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);

            string collectionName = "TestUserIsAdminCollection";
            string collectionDesciption = "Test collection description";
            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";

            AccessMode defaultAccess = GetDefaultPermissions();

            CFAccessDefinition ad1 = new CFAccessDefinition()
            {
                Name = "Test 1",
                AccessModes = AccessMode.Write
            };

            CFAccessDefinition ad2 = new CFAccessDefinition()
            {
                Name = "Test 2",
                AccessModes = AccessMode.Control | AccessMode.Append
            };

            CFAccessDefinition ad3 = new CFAccessDefinition()
            {
                Name = "Test 3",
                AccessModes = AccessMode.Read
            };

            CFAccessDefinition ad4 = new CFAccessDefinition()
            {
                Name = "Test 4",
                AccessModes = AccessMode.Read | AccessMode.Write
            };

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c1 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i2 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c2 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);

            List<CFAccessGroup> groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[0]), AccessDefinition = ad1 },
            };

            c1.AccessGroups = groups;
            c1.BlockInheritance = true;
            c1.Serialize();

            groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[1]), AccessDefinition = ad2 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[2]), AccessDefinition = ad3 }
            };

            i2.AccessGroups = groups;
            i2.BlockInheritance = true;
            i2.Serialize();

            groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[1]), AccessDefinition = ad4 }
            };

            c2.AccessGroups = groups;
            c2.Serialize();

            i1.Serialize();

            c1.ChildMembers.Add(i1);
            i2.ChildMembers.Add(i1);
            c2.ChildMembers.Add(c1);
            Db.SaveChanges();

            AccessMode modes1 = srv.GetPermissions(Users[0].Guid, i1);
            AccessMode modes2 = srv.GetPermissions(Users[1].Guid, i1);
            AccessMode modes3 = srv.GetPermissions(Users[2].Guid, i1);
            AccessMode modes4 = srv.GetPermissions(Users[3].Guid, i1);

            Assert.AreEqual(ad1.AccessModes | ad3.AccessModes, modes1);
            Assert.AreEqual(ad1.AccessModes | ad2.AccessModes, modes2);
            Assert.AreEqual(ad2.AccessModes, modes3);
            Assert.AreEqual(ad2.AccessModes | ad3.AccessModes, modes4);

            modes1 = srv.GetPermissions(Users[0].Guid, i2);
            modes2 = srv.GetPermissions(Users[1].Guid, i2);
            modes3 = srv.GetPermissions(Users[2].Guid, i2);
            modes4 = srv.GetPermissions(Users[3].Guid, i2);

            Assert.AreEqual(ad3.AccessModes, modes1);
            Assert.AreEqual(ad2.AccessModes, modes2);
            Assert.AreEqual(ad2.AccessModes, modes3);
            Assert.AreEqual(ad2.AccessModes | ad3.AccessModes, modes4);

            modes1 = srv.GetPermissions(Users[0].Guid, c1);
            modes2 = srv.GetPermissions(Users[1].Guid, c1);
            modes3 = srv.GetPermissions(Users[2].Guid, c1);
            modes4 = srv.GetPermissions(Users[3].Guid, c1);

            Assert.AreEqual(ad1.AccessModes, modes1);
            Assert.AreEqual(ad1.AccessModes, modes2);
            Assert.AreEqual(AccessMode.None, modes3);
            Assert.AreEqual(AccessMode.None, modes4);

            modes1 = srv.GetPermissions(Users[0].Guid, c2);
            modes2 = srv.GetPermissions(Users[1].Guid, c2);
            modes3 = srv.GetPermissions(Users[2].Guid, c2);
            modes4 = srv.GetPermissions(Users[3].Guid, c2);

            Assert.AreEqual(defaultAccess, modes1);
            Assert.AreEqual(defaultAccess | ad4.AccessModes, modes2);
            Assert.AreEqual(defaultAccess | ad4.AccessModes, modes3);
            Assert.AreEqual(defaultAccess | ad4.AccessModes, modes4);
        }

        [TestMethod]
        public void TestCircularParents()
        {
            SecurityServiceBase srv = new TestSecurityService(Db);

            string collectionName = "TestUserIsAdminCollection";
            string collectionDesciption = "Test collection description";
            string itemName = "TestUserIsAdminItem";
            string itemDesciption = "Test item description";

            AccessMode defaultAccess = GetDefaultPermissions();

            CFAccessDefinition ad1 = new CFAccessDefinition()
            {
                Name = "Test 1",
                AccessModes = AccessMode.Write
            };

            CFAccessDefinition ad2 = new CFAccessDefinition()
            {
                Name = "Test 2",
                AccessModes = AccessMode.Control | AccessMode.Append
            };

            CFAccessDefinition ad3 = new CFAccessDefinition()
            {
                Name = "Test 3",
                AccessModes = AccessMode.Read
            };

            int entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i1 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Collections).FirstOrDefault().Id;
            CFCollection c1 = mDh.CreateCollection(mDh.Cs, entityType, collectionName, collectionDesciption, true);
            entityType = mDh.Ets.GetEntityTypes(CFEntityType.eTarget.Items).FirstOrDefault().Id;
            CFItem i2 = mDh.CreateItem(mDh.Is, entityType, itemName, itemDesciption, true);

            List<CFAccessGroup> groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[0]), AccessDefinition = ad1 },
            };

            c1.AccessGroups = groups;
            c1.Serialize();

            groups = new List<CFAccessGroup>()
            {
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[1]), AccessDefinition = ad2 },
                new CFAccessGroup(){ AccessGuid = Guid.Parse(Groups[2]), AccessDefinition = ad3 }
            };

            i2.AccessGroups = groups;
            i2.Serialize();

            i1.Serialize();

            c1.ChildMembers.Add(i1);
            c1.ChildMembers.Add(i2); //NOTE: Here is our circular relationship
            i2.ChildMembers.Add(i1);
            Db.SaveChanges();

            AccessMode modes1 = srv.GetPermissions(Users[0].Guid, i1);
            AccessMode modes2 = srv.GetPermissions(Users[1].Guid, i1);
            AccessMode modes3 = srv.GetPermissions(Users[2].Guid, i1);
            AccessMode modes4 = srv.GetPermissions(Users[3].Guid, i1);

            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad3.AccessModes, modes1);
            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad2.AccessModes, modes2);
            Assert.AreEqual(defaultAccess | ad2.AccessModes, modes3);
            Assert.AreEqual(defaultAccess | ad2.AccessModes | ad3.AccessModes, modes4);

            modes1 = srv.GetPermissions(Users[0].Guid, i2);
            modes2 = srv.GetPermissions(Users[1].Guid, i2);
            modes3 = srv.GetPermissions(Users[2].Guid, i2);
            modes4 = srv.GetPermissions(Users[3].Guid, i2);

            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad3.AccessModes, modes1);
            Assert.AreEqual(defaultAccess | ad1.AccessModes | ad2.AccessModes, modes2);
            Assert.AreEqual(defaultAccess | ad2.AccessModes, modes3);
            Assert.AreEqual(defaultAccess | ad2.AccessModes | ad3.AccessModes, modes4);

            modes1 = srv.GetPermissions(Users[0].Guid, c1);
            modes2 = srv.GetPermissions(Users[1].Guid, c1);
            modes3 = srv.GetPermissions(Users[2].Guid, c1);
            modes4 = srv.GetPermissions(Users[3].Guid, c1);

            Assert.AreEqual(defaultAccess | ad1.AccessModes, modes1);
            Assert.AreEqual(defaultAccess | ad1.AccessModes, modes2);
            Assert.AreEqual(defaultAccess, modes3);
            Assert.AreEqual(defaultAccess, modes4);
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
*/