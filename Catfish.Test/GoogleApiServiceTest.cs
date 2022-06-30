using Catfish.GoogleApi.Helpers;
using Catfish.Test.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Test
{
    public class GoogleApiServiceTest
    {
        private protected TestHelper _testHelper;
        string _apiKey = "";

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _apiKey = _testHelper.Configuration.GetSection("GoogleApiKey").Value;
        }

        [Test]
        public void ColumnLetterConversionTest()
        {
            Assert.AreEqual(1, SheetHelper.Letter2Column("A"));
            Assert.AreEqual(2, SheetHelper.Letter2Column("B"));
            Assert.AreEqual(26, SheetHelper.Letter2Column("Z"));
            Assert.AreEqual(27, SheetHelper.Letter2Column("AA"));
            Assert.AreEqual(28, SheetHelper.Letter2Column("Ab"));
            Assert.AreEqual(52, SheetHelper.Letter2Column("AZ"));
            Assert.AreEqual(53, SheetHelper.Letter2Column("BA"));

            Assert.AreEqual("A", SheetHelper.Column2Letter(1));
            Assert.AreEqual("B", SheetHelper.Column2Letter(2));
            Assert.AreEqual("Z", SheetHelper.Column2Letter(26));
            Assert.AreEqual("AA", SheetHelper.Column2Letter(27));
            Assert.AreEqual("AB", SheetHelper.Column2Letter(28));
            Assert.AreEqual("AZ", SheetHelper.Column2Letter(52));
            Assert.AreEqual("BA", SheetHelper.Column2Letter(53));
        }
    }
}
