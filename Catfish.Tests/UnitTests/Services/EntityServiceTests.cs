using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.UnitTests.Services
{
    [TestFixture]
    class EntityServiceTests : BaseUnitTest
    {
        private const string NAME_NAME = "Test Name";
        private const string NAME_DESCRIPTION = "Test Description";
        private const string NAME_DROPDOWN = "Test Options";
        private const string NEW_NAME_NAME = "Test Name";
        private const string NEW_NAME_DESCRIPTION = "Test Description";
        private const string NEW_NAME_DROPDOWN = "Test Options";
        private const string OPTION_ONE_EN = "ONE";
        private const string OPTION_TWO_EN = "Two";
        private const string OPTION_THREE_EN = "Three";

        private DatabaseHelper mDh { get; set; }

        protected override void OnSetup()
        {
            mDh = new DatabaseHelper(false);
        }

        protected override void OnTearDown()
        {
        }

        private Option CreateOption(string[] values, string[] languageCodes)
        {
            Option option = new Option();
            List<TextValue> optionValues = new List<TextValue>(values.Length);

            for(int i = 0; i < values.Length; ++i)
            {
                optionValues.Add(new TextValue(languageCodes[i], languageCodes[i], values[i]));
            }

            option.Value = optionValues;

            return option;
        }
        
        private CFMetadataSet CreateBasicMetadataSet(string name, string description)
        {
            List<FormField> fields = new List<FormField>();

            TextField nameField = new TextField()
            {
                Name = NAME_NAME,
                IsRequired = true
            };
            fields.Add(nameField);

            TextArea descriptionField = new TextArea()
            {
                Name = NAME_DESCRIPTION,
                IsRequired = false
            };
            fields.Add(descriptionField);

            DropDownMenu dropdownField = new DropDownMenu()
            {
                Name = NAME_DROPDOWN,
                IsRequired = false,

            };
            List<Option> options = new List<Option>()
            {
                CreateOption(new string[]{ OPTION_ONE_EN }, new string[]{ "en" }),
                CreateOption(new string[]{ OPTION_TWO_EN }, new string[]{ "en" }),
            };
            dropdownField.Options = options;
            fields.Add(dropdownField);

            return CreateMetadataSet(mDh, name, description, fields);
        }

        [Test]
        public void UpdateExistingEntityMetadataTest()
        {
            const string metadataName = "UpdateExistingEntityMetadataTest";
            const string metadataDescription = "Test for function UpdateExistingEntityMetadata.";
            const string entityTypeName = "UpdateExistingEntityMetadataTest";
            const string entityTypeDescription = "Test for function UpdateExistingEntityMetadata.";

            // Create an entity
            CFMetadataSet metadata = CreateBasicMetadataSet(metadataName, metadataDescription);

            CFEntityType entityType = CreateEntityType(mDh, entityTypeName, entityTypeDescription, "Items", metadata);

            mDh.Db.SaveChanges();

            CFItem item1 = mDh.CreateItem(mDh.Is, entityType.Id, "Item 1", "Item 1 Description", true);
            CFItem item2 = mDh.CreateItem(mDh.Is, entityType.Id, "Item 2", "Item 2 Description", true);



        }
    }
}
