using Catfish.Core.Models;
using Catfish.Core.Models.Ingestion;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Tests.Services
{
    [TestFixture]
    public class IngestionServiceTest : BaseServiceTest
    {
        [Ignore("Test needs to be corrected")]
        [Test]
        public void ImportCorrectNewTest()
        {
            DatabaseHelper Dh = new DatabaseHelper(false);
            using(Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Catfish.Tests.Resources.IngestionDatabase1.xml"))
            {
                Dh.Igs.Import(stream);
                Dh.Db.SaveChanges();
            }

            // Check metadata set
            Assert.AreEqual(Dh.Db.MetadataSets.Count(), 1);
            Assert.IsFalse(Dh.Db.MetadataSets.ToList().Where(m => m.Guid == "964246e0dae940b7ae211dc60d992e35").Any());

            // Check entity types
            Assert.AreEqual(Dh.Db.EntityTypes.Count(), 1);
            Assert.IsFalse(Dh.Db.EntityTypes.Where(e => e.Id == 3).Any());
            
            //Check Aggrigations
            Assert.AreEqual(Dh.Db.Collections.Count(), 1);
            Assert.AreEqual(Dh.Db.Items.Count(), 2);

            Assert.IsFalse(Dh.Db.Collections.ToList().Where(c => c.Guid == "abdb71f7e05243a39ff6658799d9eed6").Any());
            Assert.IsFalse(Dh.Db.Items.ToList().Where(c => c.Guid == "34bd149fe442478f878b3e0b39a68144").Any());
            Assert.IsFalse(Dh.Db.Items.ToList().Where(c => c.Guid == "844475ec5dc24de58110852a19988880").Any());

            // Check Relationships
            Assert.AreEqual(Dh.Db.Items.SelectMany(i => i.ChildRelations).Count(), 1);
            Assert.AreEqual(Dh.Db.Collections.SelectMany(c => c.ChildMembers).Count(), 2);
            
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void ImportCorrectOverwriteTest()
        {
            DatabaseHelper Dh = new DatabaseHelper(false);
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Catfish.Tests.Resources.IngestionDatabase2.xml"))
            {
                Dh.Igs.Import(stream);
                Dh.Db.SaveChanges();
            }
            
            // Check metadata set
            Assert.AreEqual(Dh.Db.MetadataSets.Count(), 1);
            Assert.IsTrue(Dh.Db.MetadataSets.ToList().Where(m => m.Guid == "964246e0dae940b7ae211dc60d992e35").Any());

            // Check entity types
            Assert.AreEqual(Dh.Db.EntityTypes.Count(), 1);
            Assert.IsTrue(Dh.Db.EntityTypes.Where(e => e.Id == 1).Any());

            // Check attribute mappings
            Assert.IsTrue(Dh.Db.EntityTypeAttributeMappings.Where(a =>
                a.Name == "Name Mapping" &&
                a.FieldName == "Name"
            ).Select(a => a.MetadataSet).ToList().Where(m =>
                m.Guid == "964246e0dae940b7ae211dc60d992e35"
            ).Any());

            Assert.IsTrue(Dh.Db.EntityTypeAttributeMappings.Where(a =>
                a.Name == "Description Mapping" &&
                a.FieldName == "Description"
            ).Select(a => a.MetadataSet).ToList().Where(m =>
                m.Guid == "964246e0dae940b7ae211dc60d992e35"
            ).Any());

            //Check Aggrigations
            Assert.AreEqual(Dh.Db.Collections.Count(), 1);
            Assert.AreEqual(Dh.Db.Items.Count(), 2);

            Assert.IsTrue(Dh.Db.Collections.ToList().Where(c => c.Guid == "abdb71f7e05243a39ff6658799d9eed6").Any());
            Assert.IsTrue(Dh.Db.Items.ToList().Where(c => c.Guid == "34bd149fe442478f878b3e0b39a68144").Any());
            Assert.IsTrue(Dh.Db.Items.ToList().Where(c => c.Guid == "844475ec5dc24de58110852a19988880").Any());

            // Check Relationships
            Assert.AreEqual(Dh.Db.Collections.SelectMany(c => c.ChildMembers).Count(), 2);
            Assert.IsTrue(Dh.Db.Collections.SelectMany(c => c.ChildMembers).ToList().Where(i => i.Guid == "34bd149fe442478f878b3e0b39a68144").Any());
            Assert.IsTrue(Dh.Db.Collections.SelectMany(c => c.ChildMembers).ToList().Where(i => i.Guid == "844475ec5dc24de58110852a19988880").Any());
            Assert.AreEqual(Dh.Db.Items.SelectMany(i => i.ChildRelations).Count(), 1);
            Assert.IsTrue(Dh.Db.Items.SelectMany(i => i.ChildRelations).ToList().Where(i => i.Guid == "34bd149fe442478f878b3e0b39a68144").Any());
        }

        [Test]
        public void ImportBadTest()
        {
            DatabaseHelper Dh = new DatabaseHelper(false);

            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Catfish.Tests.Resources.IngestionDatabase3.xml"))
                {
                    Dh.Igs.Import(stream);
                    Dh.Db.SaveChanges();
                }

                Assert.Fail();
            }catch(FormatException ex)
            {
                Assert.AreEqual("Invalid XML relationship element.", ex.Message);
                // We should reach htis state because of our bad data.
            }
        }

        private void AssertMetadatasetsXmlEqualIngestion(XElement xml, Ingestion ingestion)
        {
            Assert.AreEqual(xml.Name.LocalName, "metadata-sets");
            int setCount = ingestion.MetadataSets.Count;

            foreach(XElement element in xml.Elements())
            {
                string name = element.Name.LocalName;

                if(name == "metadata-set")
                {
                    --setCount;
                    string guid = element.Attribute("guid").Value;
                    Assert.IsTrue(ingestion.MetadataSets.Select(m => m.Guid).Contains(guid));
                }
                else
                {
                    Assert.Fail("Unkown element \"{0}\" found.", name);
                }
            }

            Assert.AreEqual(setCount, 0);
        }

        private void AssertEntityTypesXmlEqualIngestion(XElement xml, Ingestion ingestion)
        {
            Assert.AreEqual(xml.Name.LocalName, "entity-types");
            int setCount = ingestion.EntityTypes.Count;

            foreach (XElement element in xml.Elements())
            {
                string name = element.Name.LocalName;

                if (name == "entity-type")
                {
                    --setCount;
                    string id = element.Attribute("id").Value;
                    Assert.IsTrue(ingestion.EntityTypes.Select(m => m.Id.ToString()).Contains(id));
                }
                else
                {
                    Assert.Fail("Unkown element \"{0}\" found.", name);
                }
            }

            Assert.AreEqual(setCount, 0);
        }

        private void AssertAggrigationsEqualIngestion(XElement xml, Ingestion ingestion)
        {
            Assert.AreEqual(xml.Name.LocalName, "aggregations");
            int setCount = ingestion.Aggregations.Count;

            foreach (XElement element in xml.Elements())
            {
                string name = element.Name.LocalName;

                if (name == "collection")
                {
                    --setCount;
                    string guid = element.Attribute("guid").Value;
                    Assert.IsTrue(ingestion.Aggregations.Select(m => m.Guid).Contains(guid));
                }else if (name == "item")
                {
                    --setCount;
                    string guid = element.Attribute("guid").Value;
                    Assert.IsTrue(ingestion.Aggregations.Select(m => m.Guid).Contains(guid));
                }
                else
                {
                    Assert.Fail("Unkown element \"{0}\" found.", name);
                }
            }

            Assert.AreEqual(setCount, 0);
        }

        private void AssertRelationshipsEqualIngestion(XElement xml, Ingestion ingestion)
        {
            Assert.AreEqual(xml.Name.LocalName, "relationships");
            int setCount = ingestion.Relationships.Count;

            foreach (XElement element in xml.Elements())
            {
                string name = element.Name.LocalName;

                if (name == "relationship")
                {
                    --setCount;
                }
                else
                {
                    Assert.Fail("Unkown element \"{0}\" found.", name);
                }
            }

            Assert.AreEqual(setCount, 0);
        }

        private void AsserXmlEqualIngestion(XElement xml, Ingestion ingestion)
        {
            Assert.AreEqual(xml.Name.LocalName, "ingestion");
            Assert.AreEqual(bool.Parse(xml.Attribute("overwrite").Value), ingestion.Overwrite);

            List<string> attributes = new List<string>() { "overwrite" };
            List<string> elements = new List<string>() { "metadata-sets", "entity-types", "aggregations", "relationships" };

            foreach (XAttribute attribute in xml.Attributes())
            {
                string name = attribute.Name.LocalName;

                if (name == "overwrite")
                {
                    Assert.AreEqual(bool.Parse(attribute.Value), ingestion.Overwrite);
                    attributes.Remove("overwrite");
                }
                else
                {
                    Assert.Fail("Invalid attribute \"{0}\" found.", name);
                }
            }

            foreach (XElement element in xml.Elements())
            {
                string name = element.Name.LocalName;
                if (name == "metadata-sets")
                {
                    AssertMetadatasetsXmlEqualIngestion(element, ingestion);
                    elements.Remove("metadata-sets");
                }
                else if (name == "entity-types")
                {
                    AssertEntityTypesXmlEqualIngestion(element, ingestion);
                    elements.Remove("entity-types");
                }
                else if (name == "aggregations")
                {
                    AssertAggrigationsEqualIngestion(element, ingestion);
                    elements.Remove("aggregations");
                }
                else if (name == "relationships")
                {
                    AssertRelationshipsEqualIngestion(element, ingestion);
                    elements.Remove("relationships");
                }
                else
                {
                    Assert.Fail("Unkown element \"{0}\" found.", name);
                }
            }

            Assert.AreEqual(attributes.Count, 0);
            Assert.AreEqual(elements.Count, 0);
        }

        [Test]
        public void ExportPartialTest()
        {
            string result = "";
            DatabaseHelper Dh = new DatabaseHelper(true);

            // Build the Ingestion
            CFCollection collection = Dh.Db.Collections.First();
            IEnumerable<CFItem> items = Dh.Db.Items.Take(2);
            IEnumerable<CFEntityType> entitytypes = Dh.Db.EntityTypes.Where(e => e.Id == collection.EntityType.Id || items.Select(i => i.EntityTypeId).Contains(e.Id)).Distinct();
            IEnumerable<CFMetadataSet> metadatasets = Dh.Db.MetadataSets.Where(m => entitytypes.SelectMany(e => e.MetadataSets.Select(mm => mm.Id)).Contains(m.Id)).Distinct();

            Ingestion ingestion = new Ingestion();
            ingestion.MetadataSets.AddRange(metadatasets);
            ingestion.EntityTypes.AddRange(entitytypes);
            ingestion.Aggregations.Add(collection);
            ingestion.Aggregations.AddRange(items);

            CFItem[] itemArray = items.ToArray();

            ingestion.Relationships.Add(new Relationship()
            {
                ParentRef = collection.Guid,
                ChildRef = itemArray[0].Guid
            });
            ingestion.Relationships.Add(new Relationship()
            {
                IsMember = true,
                IsRelated = false,
                ParentRef = collection.Guid,
                ChildRef = itemArray[1].Guid
            });
            ingestion.Relationships.Add(new Relationship()
            {
                IsRelated = true,
                IsMember = false,
                ParentRef = itemArray[0].Guid,
                ChildRef = itemArray[1].Guid
            });

            // Export the injestion
            using(MemoryStream stream = new MemoryStream())
            {
                Dh.Igs.Export(stream, ingestion);
                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }

            XElement testResult = XElement.Parse(result);

            // Test result
            AsserXmlEqualIngestion(testResult, ingestion);
        }

        [Test]
        public void ExportAllTest()
        {
            string result;
            DatabaseHelper Dh = new DatabaseHelper(true);

            // Extract all database content
            using (MemoryStream stream = new MemoryStream())
            {
                Dh.Igs.Export(stream);
                stream.Position = 0;
                
                using(StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }

            XElement testResult = XElement.Parse(result);

            // Check the result values.
            Assert.AreEqual(testResult.Name.LocalName, "ingestion");

            foreach(XElement element in testResult.Elements())
            {
                string name = element.Name.LocalName;

                if(name == "metadata-sets")
                {
                    Assert.AreEqual(element.Elements().Count(), Dh.Db.MetadataSets.Count());
                }
                else if(name == "entity-types")
                {
                    Assert.AreEqual(element.Elements().Count(), Dh.Db.EntityTypes.Count());
                }
                else if(name == "aggregations")
                {
                    Assert.AreEqual(element.Elements().Count(), Dh.Db.Items.Count() + Dh.Db.Collections.Count() + Dh.Db.FormTemplates.Count());
                }
                else if(name == "relationships")
                {
                    //TODO
                    Assert.AreEqual(element.Elements().Count(), countRelationship(Dh));
                }
                else
                {
                    Assert.Fail("Unkown element \"{0}\" found.", name);
                }
            }

            
        }
        private int countRelationship(DatabaseHelper Dh)
        {
            int total = 0;

            foreach (var col in Dh.Db.Collections)
            {
                foreach(var rel in col.ChildMembers)
                {
                    total++;
                }
            }
            foreach (var item in Dh.Db.Items)
            {
                foreach (var rel in item.ChildMembers)
                {
                    total++;
                }
                foreach (var rel in item.ChildRelations)
                {
                    total++;
                }
            }

            return total;
        }

    }
}
