using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Catfish.Core.Models.Metadata;
using Catfish.Core.Models;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Catfish.Tests
{
    [TestClass]
    public class MetadataSetTest: XmlModelTest
    {
        [TestMethod]
        public void MetadataFieldsTest()
        {
            string path = GetSampleDataFilePathName("MetadataSet.xml");
            Assert.IsTrue(File.Exists(path));

            XElement root = XElement.Load(path);
            Assert.IsNotNull(root);

            MetadataSet model = XmlModel.Parse(root) as MetadataSet;
            Assert.IsNotNull(model);

            List<MetadataField> fields = model.Fields;
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

            var options = (field as RadioButtonSet).GetOptions("fr");
            Assert.AreEqual("Première option\nDeuxième option\nTroisième option", options);
            Assert.AreEqual("First option\nSecond option\nThird option", (field as RadioButtonSet).Options);

        }
    }
}
