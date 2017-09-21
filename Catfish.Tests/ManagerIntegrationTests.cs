using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using System.Collections.Generic;
using System;

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
        public static string FieldOptions = "Option 1\r\nOption 2\r\nOption 3";
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
            this.AddFilledMetadataSet();
            this.Driver.FindElement(By.ClassName("save")).Click();
            this.Driver.FindElement(By.LinkText("Metadata Sets")).Click();

            Assert.AreEqual(this.GetLastNameFromList(), TestValues.MetadatasetName);
            Assert.AreEqual(this.GetLastDescriptionFromList(), TestValues.MetadatasetDescription);
            
        }

        [Test]
        public void CanCreateCompleteMetadataset()
        {
            this.AddFilledMetadataSet();
            this.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            SelectElement typeSelector = new SelectElement(this.Driver.FindElement(By.CssSelector("#field-type-selector")));
            IWebElement addFieldButton = this.Driver.FindElement(By.Id("add-field"));
            IWebElement saveButton = this.Driver.FindElement(By.ClassName("save"));
            int optionsCount = typeSelector.Options.Count;
            // Ignore first empty option. Add all fields and enter data 
            for (int i = 1; i < optionsCount; i++)
            {

                IWebElement option = typeSelector.Options[i];
                typeSelector.SelectByText(option.Text);
                addFieldButton.Click();

                IWebElement lastFieldElement = this.Driver.FindElement(By.CssSelector(".field-entry:last-child"));

                lastFieldElement.FindElement(By.ClassName("field-name")).SendKeys(TestValues.FieldName + i);
                lastFieldElement.FindElement(By.ClassName("field-description")).SendKeys(TestValues.FieldDescription + i);

                //XXX Check for options field
                if (lastFieldElement.FindElement(By.ClassName("field-options")).Displayed)
                {
                    lastFieldElement.FindElement(By.ClassName("field-options")).SendKeys(TestValues.FieldOptions + i);
                }
                
                lastFieldElement.FindElement(By.ClassName("field-is-required")).Click();
             
            }

            IJavaScriptExecutor js = (IJavaScriptExecutor)this.Driver;
            js.ExecuteScript("arguments[0].scrollIntoView(false)", saveButton);
            saveButton.Click();
            this.Driver.FindElement(By.LinkText("Metadata Sets")).Click();

            // Go to see editor view for newly created metadataset
            IWebElement editButton = this.Driver.FindElement(By.CssSelector(".list tr:last-child td:nth-child(3) a:nth-child(2)"));
            js.ExecuteScript("arguments[0].scrollIntoView(false)", editButton);

            editButton.Click();

            //XXX need to come back to this test once options are working

            Assert.AreEqual(this.Driver.FindElement(By.Id("Name")).GetAttribute("value"), TestValues.MetadatasetName);
            Assert.AreEqual(this.Driver.FindElement(By.Id("Description")).GetAttribute("value"), TestValues.MetadatasetDescription);

            // Check all field values

            IReadOnlyCollection<IWebElement> fields = this.Driver.FindElements(By.ClassName("field-entry"));

            
            // need to take into account and ignore the first empty option
            if (optionsCount - 1 == fields.Count)
            {
               int index = 1;
                foreach ( IWebElement fieldEntry in fields )
                {
                    Assert.AreEqual(TestValues.FieldName + index, fieldEntry.FindElement(By.ClassName("field-name")).GetAttribute("value"));
                    Assert.AreEqual(TestValues.FieldDescription + index, fieldEntry.FindElement(By.ClassName("field-description")).GetAttribute("value"));
                    if (fieldEntry.FindElement(By.ClassName("field-options")).Displayed)
                    {
                        Assert.AreEqual(TestValues.FieldOptions + index, fieldEntry.FindElement(By.ClassName("field-options")).GetAttribute("value"));
                    }
                    Assert.AreEqual(TestValues.FieldRequired, fieldEntry.FindElement(By.ClassName("field-is-required")).Selected);
                    ++index;
                }
            } else
            {
                Assert.Fail("Wrong number of metadataset fields");
            }
        }

        [Test]
        public void CanCreateSimpleEntityType()
        {
            this.AddFilledEntityType();
            this.Driver.FindElement(By.ClassName("save")).Click();
            this.Driver.FindElement(By.LinkText("Entity Types")).Click();

            Assert.AreEqual(this.GetLastNameFromList(), TestValues.EntityTypeName);
            Assert.AreEqual(this.GetLastDescriptionFromList(), TestValues.EntityTypeDescription);
        }

        [Test]
        public void CanCreateEntityTypeWithFields()
        {

            int metadatsetCount = 2;
            string[] metadatasetNames = new string[metadatsetCount];
            this.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Need to make sure there are two known metadata sets
            for (int i = 0; i < metadatasetNames.Length; ++i)
            {
                metadatasetNames[i] = TestValues.EntityTypeName + i;
                this.AddFilledMetadataSet(metadatasetNames[i]);
                this.Driver.FindElement(By.ClassName("save")).Click();
            }

            this.AddFilledEntityType();

            // Add two created metadata sets
            

            //SelectElement typeSelector = new SelectElement(this.Driver.FindElement(By.Id("metadataset-selector")));

            for (int i = 0; i < metadatasetNames.Length; ++i)
            {
                SelectElement typeSelector = new SelectElement(this.Driver.FindElement(By.Id("metadataset-selector")));
                IWebElement addField = this.Driver.FindElement(By.Id("add-field"));
                typeSelector.SelectByText(metadatasetNames[i]);                
                addField.Click();
            }

                //for (int i = 0; i < metadatasetNames.Length; ++i) {
                //    addField.Click();
                //    SelectElement typeSelector = new SelectElement(this.Driver.FindElement(By.CssSelector(".field-entry:last-child .metadataset-selector")));
                //    typeSelector.SelectByText(metadatasetNames[i]);
                //}

            this.Driver.FindElement(By.ClassName("save")).Click();
            this.Driver.FindElement(By.LinkText("Entity Types")).Click();
            this.Driver.FindElement(By.CssSelector(".list tr:last-child td:nth-child(3) a:nth-child(2)")).Click();

            // Check that values were saved

            Assert.AreEqual(TestValues.EntityTypeName, this.Driver.FindElement(By.Id("Name")).GetAttribute("value"));
            Assert.AreEqual(TestValues.EntityTypeDescription, this.Driver.FindElement(By.Id("Description")).GetAttribute("value"));

            IReadOnlyCollection<IWebElement> fields = this.Driver.FindElements(By.ClassName("field-entry"));

            if (metadatsetCount == fields.Count)
            {
                int index = 0;
                foreach (IWebElement field in fields)
                {
                    string metadatasetName = field.FindElement(By.ClassName("metadataset-name")).Text;
                    Assert.AreEqual(metadatasetNames[index], metadatasetName);
                    ++index;
                }                   
            } else
            {
                
                Assert.Fail("Wrong number of metadataset fields. Expecting "+metadatsetCount+ " and found " + fields.Count + ".");
            }

            //if (metadatsetCount == fields.Count)
            //{
            //    int index = 0;                
            //    foreach (IWebElement field in fields)
            //    {
            //        SelectElement typeSelector = new SelectElement(field.FindElement(By.ClassName("metadataset-selector")));
            //        Assert.AreEqual(metadatasetNames[index], typeSelector.SelectedOption.Text);
            //        ++index;
            //    }
            //} else
            //{
            //    Assert.Fail("Wrong number of metadataset fields");
            //}

        }

        private void AddFilledEntityType()
        {
            this.Driver.FindElement(By.LinkText("Entity Types")).Click();
            this.Driver.FindElement(By.ClassName("add")).Click();
            this.AddNameDescription(TestValues.EntityTypeName, TestValues.EntityTypeDescription);
        }

        private void AddFilledMetadataSet()
        {
            this.AddFilledMetadataSet(TestValues.MetadatasetName, TestValues.MetadatasetDescription);
        }


        private void AddFilledMetadataSet(string name = "Name", string description = "Description")
        {
            this.Driver.FindElement(By.LinkText("Metadata Sets")).Click();
            this.Driver.FindElement(By.ClassName("add")).Click();
            this.AddNameDescription(name, description);
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

        private void AddNameDescription(string name = "", string description = "")
        {
            this.Driver.FindElement(By.Id("Name")).SendKeys(name);
            this.Driver.FindElement(By.Id("Description")).SendKeys(description);            
        }

        private void Login()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            this.Driver.FindElement(By.Name("password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.Name("password")).SendKeys(Keys.Enter);
        }

    }

}

