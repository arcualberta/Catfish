using Catfish.Core.Models;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.UnitTests
{
    public class ApiTests
    {
        protected AppDbContext _db;
        protected TestHelper _testHelper;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
        }

        [Test]
        public void ItemSaveTest()
        {
            //Use api/items/ID GET call to get a json object 


            //Use api/item POST method to post the json object back

        }




    }
}
