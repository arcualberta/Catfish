using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Services
{
    [TestFixture]
   public class MetadataServiceTest : BaseServiceTest
    {
        private DatabaseHelper mDh { get; set; }
        
        protected override void OnSetup()
        {
            mDh = new DatabaseHelper(false);
        }

        private CFMetadataSet CreateMetadata(string name, string description, IEnumerable<FormField> fields)
        {
            CFMetadataSet metadata = new CFMetadataSet();
            metadata.Name = name;
            metadata.Description = description;

            metadata.Fields = new List<FormField>(fields);

            metadata.Serialize();

            return mDh.Ms.UpdateMetadataSet(metadata);
        }

        private IEnumerable<FormField> CreateFormFields(int count)
        {
            FormField field;
            List<FormField> fields = new List<FormField>();
            
            for (int i = 0; i < count; ++i)
            {
                int iCheck = i % 2;

                switch (iCheck)
                {
                    case 1:
                        field = new TextArea()
                        {
                            Name = "Text Area " + i,
                            IsRequired = false,
                            Help = "Some Text Area Help " + i
                        };
                        break;

                    default:
                        field = new TextField()
                        {
                            Name = "Text Field " + i,
                            IsRequired = false,
                            Help = "Some Help " + i
                        };
                        break;
                }
                fields.Add(field);
            }
            
            return fields;
        }

        [Test]
        public void GetMetadataSetTest()
        {
            MetadataService ms = mDh.Ms;

            int fieldCount = 10;

            string passName = "Good Metadata";
            string failName = "Bad Metadata";

            string passDescription = "Good Description";
            string failDescription = "Bad Description";

            IEnumerable<FormField> fields = CreateFormFields(fieldCount);

            CFMetadataSet check = CreateMetadata(passName, passDescription, fields);

            mDh.Db.SaveChanges();

            CFMetadataSet result = ms.GetMetadataSet(check.Id);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(failName, result.Name);
            Assert.AreEqual(passName, result.Name);
            Assert.AreNotEqual(failDescription, result.Description);
            Assert.AreEqual(passDescription, result.Description);
            Assert.AreEqual(fieldCount, result.Fields.Count);
        }

        [Test]
        public void GetMetadataSetsTest()
        {
            int fieldCount = 7;
            int setCount = 5;
            string guid = Guid.NewGuid().ToString();

            string baseName = guid + " Test Metada {0}";
            string baseDescription = guid + " Test Description {0}";

            IEnumerable<FormField> fields = CreateFormFields(fieldCount);

            for(int i = 0; i < setCount; ++i)
            {
                CreateMetadata(string.Format(baseName, i), string.Format(baseDescription, i), fields);
            }

            mDh.Db.SaveChanges();

            IEnumerable<CFMetadataSet> resultSets = mDh.Ms.GetMetadataSets().ToList().Where(m => m.Description.StartsWith(guid));

            Assert.AreEqual(setCount, resultSets.Count());

            foreach (CFMetadataSet set in resultSets)
            {
                Assert.AreEqual(fieldCount, set.Fields.Count);
                Assert.IsTrue(set.Name.StartsWith(guid));
            }
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void GetMetadataFieldTypesTest()
        {
            Type[] types =
            {
                typeof(TextField),
                typeof(TextArea),
                typeof(SingleSelectOptionsField),
                typeof(RadioButtonSet),
                typeof(PageBreak),
                typeof(DropDownMenu),
                typeof(DateField),
                typeof(CheckBoxSet),
                typeof(Attachment),
                typeof(SliderField)
            };

            List<Type> testTypes = mDh.Ms.GetMetadataFieldTypes();

            Assert.AreEqual(types.Length, testTypes.Count);

            foreach(Type t in types)
            {
                Assert.IsTrue(testTypes.Contains(t));
            }
        }

        [Test]
        public void UpdateMetadataSetTest()
        {
            MetadataService ms = mDh.Ms;
            string originalName = "First Name";
            string newName = "Second Name";

            string originalDescription = "Original Description";
            string newDescription = "New description";

            int fieldCount = 4;
            IEnumerable<FormField> fields = CreateFormFields(fieldCount);

            CFMetadataSet check = CreateMetadata(originalName, originalDescription, new FormField[] { });
            mDh.Db.SaveChanges();

            CFMetadataSet result = ms.GetMetadataSet(check.Id);

            Assert.AreEqual(originalName, check.Name);
            Assert.AreEqual(originalDescription, check.Description);
            Assert.AreEqual(0, check.Fields.Count);

            result.Name = newName;
            result.Description = newDescription;
            result.Fields = new List<FormField>(fields);
            result.Serialize();

            ms.UpdateMetadataSet(result);
            mDh.Db.SaveChanges();

            result = ms.GetMetadataSet(check.Id);

            Assert.AreEqual(newName, check.Name);
            Assert.AreEqual(newDescription, check.Description);
            Assert.AreEqual(fieldCount, check.Fields.Count);
        }
    }
}
