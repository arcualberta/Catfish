using System;
using Catfish.Core.Models.Forms;
using NUnit.Framework;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using Catfish.Core.Models;
using System.Collections.Generic;
using Catfish.Tests.Helpers;

namespace Catfish.Tests
{
    [TestFixture]
    public class XmlModelTest : BaseUnitTest
    {
        public static string GetSampleDataFilePathName(string fileName)
        {
            var dir = Directory.GetCurrentDirectory();
            dir = dir.Substring(0, dir.IndexOf("Catfish.Test"));
            dir = Path.Combine(dir, "XmlTemplates");
            return Path.Combine(dir, fileName);
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void MetadataFieldsTest()
        {
            string path = GetSampleDataFilePathName("MetadataSet.xml");
            Assert.IsTrue(File.Exists(path));

            XElement root = XElement.Load(path);
            Assert.IsNotNull(root);

            CFMetadataSet model = CFXmlModel.Parse(root) as CFMetadataSet;
            Assert.IsNotNull(model);

            IReadOnlyList<FormField> fields = model.Fields;
            Assert.AreEqual(3, fields.Count);

            var field = fields.Where(f => f is TextField).FirstOrDefault();
            Assert.IsNotNull(field);
            Assert.AreEqual("Subject", field.GetName("en"));
            Assert.AreEqual("Sujet", field.GetName("fr"));
            Assert.AreEqual("Enter the subject here.", field.GetDescription("en"));

            field = fields.Where(f => f is TextArea).FirstOrDefault();
            Assert.IsNotNull(field);
            Assert.AreEqual("Description", field.GetName("en"));
            Assert.AreEqual("La description", field.GetName("fr"));
            Assert.AreEqual("Enter the description here.", field.GetDescription("en"));

            field = fields.Where(f => f is RadioButtonSet).FirstOrDefault();
            Assert.IsNotNull(field);
            Assert.AreEqual("Radio button set 1", field.GetName("en"));
            Assert.AreEqual("Réglage du bouton radio 1", field.GetName("fr"));
            Assert.AreEqual("This is a radio button set.", field.GetDescription("en"));

            ////var options = (field as RadioButtonSet).GetOptions("fr");
            ////Assert.AreEqual("Première option\nDeuxième option\nTroisième option", options);
            ////Assert.AreEqual("First option\nSecond option\nThird option", (field as RadioButtonSet).Options);

        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void ItemTest()
        {
            string path = GetSampleDataFilePathName("Item.xml");
            Assert.IsTrue(File.Exists(path));

            CFItem model = CFXmlModel.Load(path) as CFItem;
            Assert.IsNotNull(model);

            List<CFMetadataSet> metadatasets = model.MetadataSets.ToList();
            Assert.AreEqual(2, metadatasets.Count());

        }

        protected override void OnSetup()
        {
        }

        protected override void OnTearDown()
        {
        }

        //[AssemblyInitialize]
        //public static void SetUp(TestContext context)
        //{
        //    var xx = Directory.GetParent(Path.GetDirectoryName(context.TestDir).ToString()).ToString();
        //    var y = Path.Combine(xx, @"packages\Selenium.WebDriver.ChromeDriver.2.32.0\driver\win32\chromedriver.exe");

        //    string solution_dir = xx.ToString();
        //}
        //[Test]
        //public void Test()
        //{
        //    Assert.AreEqual(1, 1);
        //}


    }
}
