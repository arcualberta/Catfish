using Catfish.Core.Models;
using Catfish.Core.Models.Ingestion;
using Catfish.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Tests.Services
{
    [TestClass]
    public class IngestionServiceTest
    {        
        [TestMethod]
        public void ImportCorrectNewTest()
        {
            DatabaseHelper Dh = new DatabaseHelper(false);
            using(FileStream stream = File.Open("./Resources/IngestionDatabase1.xml", FileMode.Open))
            {
                Dh.Igs.Import(stream);
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


        }

        [TestMethod]
        public void ImportCorrectOverwriteTest()
        {
            DatabaseHelper Dh = new DatabaseHelper(false);
            using (FileStream stream = File.Open("./Resources/IngestionDatabase2.xml", FileMode.Open))
            {
                Dh.Igs.Import(stream);
            }

            // Check collection Info
        }

        [TestMethod]
        public void ImportBadTest()
        {
            DatabaseHelper Dh = new DatabaseHelper(false);

            try
            {
                using (FileStream stream = File.Open("./Resources/IngestionDatabase3.xml", FileMode.Open))
                {
                    Dh.Igs.Import(stream);
                }

                Assert.Fail();
            }catch(Exception ex)
            {

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

        [TestMethod]
        public void ExportPartialTest()
        {
            string result = "";
            DatabaseHelper Dh = new DatabaseHelper(true);

            Collection collection = Dh.Db.Collections.First();
            IEnumerable<Item> items = Dh.Db.Items.Take(2);
            IEnumerable<EntityType> entitytypes = Dh.Db.EntityTypes.Where(e => e.Id == collection.EntityType.Id || items.Select(i => i.EntityTypeId).Contains(e.Id)).Distinct();
            IEnumerable<MetadataSet> metadatasets = Dh.Db.MetadataSets.Where(m => entitytypes.SelectMany(e => e.MetadataSets.Select(mm => mm.Id)).Contains(m.Id)).Distinct();


            Ingestion ingestion = new Ingestion();
            ingestion.MetadataSets.AddRange(metadatasets);
            ingestion.EntityTypes.AddRange(entitytypes);
            ingestion.Aggregations.Add(collection);
            ingestion.Aggregations.AddRange(items);

            Item[] itemArray = items.ToArray();

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

            AsserXmlEqualIngestion(testResult, ingestion);
        }

        [TestMethod]
        public void ExportAllTest()
        {
            string result;

            DatabaseHelper Dh = new DatabaseHelper(true);
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
                else if(name == "aggrigations")
                {
                    Assert.AreEqual(element.Elements().Count(), Dh.Db.Items.Count() + Dh.Db.Collections.Count() + Dh.Db.FormTemplates.Count());
                }
                else if(name == "relationships")
                {
                    //TODO
                }
                else
                {
                    Assert.Fail("Unkown element \"{0}\" found.", name);
                }
            }
        }
    }
}
