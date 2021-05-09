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
            var sheetsService = _testHelper.GoogleSpreadsheetService;

            Assert.AreEqual(1, sheetsService.Letter2Column("A"));
            Assert.AreEqual(2, sheetsService.Letter2Column("B"));
            Assert.AreEqual(26, sheetsService.Letter2Column("Z"));
            Assert.AreEqual(27, sheetsService.Letter2Column("AA"));
            Assert.AreEqual(28, sheetsService.Letter2Column("Ab"));
            Assert.AreEqual(52, sheetsService.Letter2Column("AZ"));
            Assert.AreEqual(53, sheetsService.Letter2Column("BA"));

            Assert.AreEqual("A", sheetsService.Column2Letter(1));
            Assert.AreEqual("B", sheetsService.Column2Letter(2));
            Assert.AreEqual("Z", sheetsService.Column2Letter(26));
            Assert.AreEqual("AA", sheetsService.Column2Letter(27));
            Assert.AreEqual("AB", sheetsService.Column2Letter(28));
            Assert.AreEqual("AZ", sheetsService.Column2Letter(52));
            Assert.AreEqual("BA", sheetsService.Column2Letter(53));
        }
    }
}
