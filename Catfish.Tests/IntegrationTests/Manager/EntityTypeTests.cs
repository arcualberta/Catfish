﻿using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Tests.Extensions;
using Catfish.Tests.IntegrationTests.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.IntegrationTests.Manager
{
    [TestFixture(typeof(ChromeDriver))]
    class EntityTypeTests<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        [Test]
        public void CanCreateNewEntityType()
        {
            string EntityTypeName = "Entity type name";
            string EntityTypeDescription = "Entity type description";
            string MetadataSetName = "Metadata set name";
            string MetadataSetDescription = "Metadata set description";


            TextField fieldName = new TextField();
            fieldName.Name = "Name";

            TextArea fieldDescription = new TextArea();
            fieldName.Name = "Description";
            List<FormField> formFields = new List<FormField>();
            formFields.Add(fieldName);
            formFields.Add(fieldDescription);

            CreateMetadataSet(MetadataSetName, MetadataSetDescription, formFields.ToArray());

            CreateEntityType(EntityTypeName, EntityTypeDescription, new[] {
                MetadataSetName
                }, new CFEntityType.eTarget[0]);

            // add linking

            Driver.FindElement(By.LinkText(SettingsLinkText), 10).Click();
            Driver.FindElement(By.LinkText(EntityTypesLinkText), 10).Click();

            IWebElement lastEditButton = GetLastEditButton();
            lastEditButton.Click();
            string nameText = Driver.FindElement(By.Id("Name")).GetAttribute("value");
            string descriptionText = Driver.FindElement(By.Id("Description")).GetAttribute("value");

            Assert.AreEqual(MetadataSetName, nameText);
            Assert.AreEqual(MetadataSetDescription, descriptionText);

        }
    }
}
