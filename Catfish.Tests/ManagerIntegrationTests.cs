using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace Catfish.Tests
{

    static class TestValues
    {
        public static string MetadatasetName = "Metadataset Name";
        public static string MetadatasetDescription = "Metadataset Description";
        public static string EntityTypeName = "Entity Type Name";
        public static string EntityTypeDescription = "Entity Type Description";
        public static string FieldName = "Field Name";
        public static string FieldDescription = "Field Description";
        public static string FieldOptions = "Option 1\nOption 2\nOption 3";
        public static bool FieldRequired = true;
        public static string PostfixEdit = " Edit";
    }

    [TestFixture(typeof(ChromeDriver))]
    public class ManagerIntegrationTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver Driver;        
        private string ManagerUrl;

        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "/manager";
            this.Login();
        }

        [TearDown]
        public void TearDown()
        {
            this.Driver.Close();
        }

        [Test]
        public void CanLogin()
        {
            bool isLogoutDisplayed = this.Driver.FindElement(By.ClassName("logout")).Displayed;
            Assert.IsTrue(isLogoutDisplayed);
        }

        [Test]
        public void CanCreateSimpleMetadataset()
        {
            this.Driver.FindElement(By.LinkText("Metadata Sets")).Click();
            this.Driver.FindElement(By.ClassName("add")).Click();
            this.AddNameDescription(TestValues.MetadatasetName, TestValues.MetadatasetDescription);
            this.Driver.FindElement(By.ClassName("save")).Click();
           
            Assert.AreEqual(this.GetLastNameFromList(), TestValues.MetadatasetName);
            Assert.AreEqual(this.GetLastDescriptionFromList(), TestValues.MetadatasetDescription);
            
        }

        [Test]
        public void CanCreateComplexMetadataset()
        {           
            this.Driver.FindElement(By.LinkText("Metadata Sets")).Click();
            this.Driver.FindElement(By.ClassName("add")).Click();
            this.AddNameDescription(TestValues.MetadatasetName, TestValues.MetadatasetDescription);

            SelectElement typeSelector = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            IWebElement addFieldButton = this.Driver.FindElement(By.Id("add-field"));
            // Add all fields and enter data
            for (int i=0; i < typeSelector.Options.Count; i++)
            {
                IWebElement option = typeSelector.Options[i];
                typeSelector.SelectByText(option.Text);

                IWebElement lastFieldElement = this.Driver.FindElement(By.ClassName("field-entry:last-child"));
                lastFieldElement.FindElement(By.ClassName("field-name")).SendKeys(TestValues.FieldName + " " + i);
                lastFieldElement.FindElement(By.ClassName("field-description")).SendKeys(TestValues.FieldDescription + " " + i);

                //XXX Checkl for options field

                //IWebElement options = lastFieldElement.FindElement(By.ClassName("field-options"))

                lastFieldElement.FindElement(By.ClassName("field-is-required")).Click();

                addFieldButton.Click();
                
            }
        }

        [Test]
        public void CanCreateSimpleEntityType()
        {
            this.Driver.FindElement(By.LinkText("Entity Types")).Click();
            this.Driver.FindElement(By.ClassName("add")).Click();
            this.AddNameDescription(TestValues.EntityTypeName, TestValues.EntityTypeDescription);
            this.Driver.FindElement(By.ClassName("save")).Click();

            Assert.AreEqual(this.GetLastNameFromList(), TestValues.EntityTypeName);
            Assert.AreEqual(this.GetLastDescriptionFromList(), TestValues.EntityTypeDescription);
        }

        private string GetLastNameFromList()
        {
            //XXX Add css classes to simplify selectors
            IWebElement nameElement = this.Driver.FindElement(By.CssSelector(".list tr:last-child td:nth-child(1)"));    
            return nameElement.Text;
        }

        private string GetLastDescriptionFromList()
        {
            //XXX Add css classes to simplify selectors
            IWebElement descriptionElement = this.Driver.FindElement(By.CssSelector(".list tr:last-child td:nth-child(2)"));         
            return descriptionElement.Text;
        }

        private void AddNameDescription(string name, string description)
        {
            this.Driver.FindElement(By.Id("Name")).SendKeys(name);
            this.Driver.FindElement(By.Id("Description")).SendKeys(description);            
        }

        private void Login()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys("admin");
            this.Driver.FindElement(By.Name("password")).SendKeys("admin");
            this.Driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
        }

    }

}

