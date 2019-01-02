using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using Catfish.Core.Models.Forms;
using Catfish.Tests.Helpers;
using Catfish.Tests.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Catfish.Tests
{
    [TestFixture]
    public class EntityTests : BaseUnitTest
    {

        MockEntity MockEntity;
        private DatabaseHelper mDh { get; set; }

        protected override void OnSetup()
        {
            MockEntity = new MockEntity();
            mDh = new DatabaseHelper(false);
        }

        protected override void OnTearDown()
        {
        }

        [Test]
        public void CanUseAccessGroups()
        {
            int count = 3;
            List<CFAccessGroup> accessGroups = new List<CFAccessGroup>();

            for (int i = 0; i < count; ++i)
            {
                accessGroups.Add(new CFAccessGroup());
            }

            MockEntity.AccessGroups = accessGroups;
            Assert.AreEqual(accessGroups.Count, MockEntity.AccessGroups.Count);

            for (int i = 0; i < count; ++i)
            {
                Assert.AreEqual(accessGroups[i].Data, MockEntity.AccessGroups[i].Data);
            }

            accessGroups.Clear();
            MockEntity.AccessGroups = accessGroups;
            Assert.AreEqual(0, MockEntity.AccessGroups.Count);
            
        }

        /// <summary>
        /// Test if a CFEntity can create a dictionary to be stored in solr and
        /// compare it to a manually created dictionary of the expected values
        /// </summary>

        [Test]
        public void CanCreateSolrDictionary()
        {
            const string entityTypeName = "Entity type name";
            const double numericValue = 42.0;
            MockEntity MockEntityChanges = new MockEntity();
            List<FormField> formFields = new List<FormField>();

            TextValue stringTextValue = new TextValue();
            stringTextValue.LanguageCode = "en";
            stringTextValue.Value = "Forty two";
            TextField textField = new TextField();
            textField.Name = "Text";
            textField.SetTextValues(new List<TextValue>() { stringTextValue });
            formFields.Add(textField);

            TextValue numericTextValue = new TextValue();
            numericTextValue.LanguageCode = "en";            
            numericTextValue.Value = numericValue.ToString();

            TextField numericField = new TextField();
            numericField.Name = "Numeric";
            numericField.SetTextValues(new List<TextValue>() { numericTextValue });
            formFields.Add(numericField);


            // Add two options and select second 

            OptionsField optionsField = new OptionsField();
            optionsField.Name = "Options field";            

            TextValue nonSelectedValue = new TextValue();
            nonSelectedValue.LanguageCode = "en";
            nonSelectedValue.Value = "Not selected";

            TextValue selectedValue = new TextValue();
            selectedValue.LanguageCode = "en";
            selectedValue.Value = "Selected";

            Option notSelectedOption = new Option();
            notSelectedOption.Selected = false;
            notSelectedOption.Value.Add(nonSelectedValue);

            Option selectedOption = new Option();
            selectedOption.Selected = true;
            selectedOption.Value.Add(selectedValue);

            List<Option> optList = new List<Option>();

            optList.Add(notSelectedOption);
            optList.Add(selectedOption);

            optionsField.Options = optList;

            formFields.Add(optionsField);

            CFMetadataSet metadataSet = CreateMetadataSet(
                mDh, 
                "name metadataset", 
                "description metadataset", 
                formFields);

            CFEntityType entityType = CreateEntityType(mDh,
                entityTypeName,
                "description entity type",
                "Items",
                metadataSet
                );
            
            MockEntity.EntityType = entityType;           
            MockEntityChanges.EntityType = entityType;
            //MockEntityChanges.MetadataSets
            MockEntity.MetadataSets = new List<CFMetadataSet>() { metadataSet };
            MockEntityChanges.MetadataSets = new List<CFMetadataSet>() { metadataSet };
            MockEntityChanges.MetadataSets[0].UpdateValueFields(formFields);

            MockEntity.UpdateValues(MockEntityChanges);

            // set vales

            //MockEntity.MetadataSets[0].UpdateValueFields(test);


            Dictionary<string, object> solrDictionary =
                MockEntity.ToSolrDictionary();

            string metadataSetGuid = metadataSet.MappedGuid.ToString().Replace("-", "_");
            string textFieldGuid = textField.MappedGuid.ToString().Replace("-", "_");
            string numericFieldGuid = numericField.MappedGuid.ToString().Replace("-", "_");
            string optionFieldGuid = optionsField.MappedGuid.ToString().Replace("-", "_");

            Dictionary<string, object> expectedResult = new Dictionary<string, object>()
            {
                { "id", solrDictionary["id"] },
                { "modeltype_s", MockEntity.GetType().Name},
                { "entitytype_s", entityTypeName },
                { $@"name_{metadataSetGuid}_{textFieldGuid}_txt_en",
                    textField.Name },
                { $@"name_{metadataSetGuid}_{numericFieldGuid}_txt_en",
                    numericField.Name },
                { $@"name_{metadataSetGuid}_{optionFieldGuid}_txt_en",
                    optionsField.Name },
                { $@"value_{metadataSetGuid}_{textFieldGuid}_txts_en",
                    new List<string>(){textField.Values[0].Value} },
                { $@"value_{metadataSetGuid}_{numericFieldGuid}_txts_en",
                    new List<string>(){numericValue.ToString()} },
                { $@"value_{metadataSetGuid}_{numericFieldGuid}_ds",
                    new List<decimal>(){ (decimal)numericValue } },
                { $@"value_{metadataSetGuid}_{numericFieldGuid}_is",
                    new List<int>(){(int)numericValue} },
                { $@"value_{metadataSetGuid}_{optionFieldGuid}_txts_en",
                    new List<string>(){ selectedValue.Value } }
            };

            foreach (KeyValuePair<string, object> entry in expectedResult)
            {
                Assert.AreEqual(entry.Value, solrDictionary[entry.Key]);
            }

        }
    }
}
