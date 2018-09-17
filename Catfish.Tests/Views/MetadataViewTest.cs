using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using Catfish.Core.Models;
using System.Data;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Catfish.Tests.Views
{

    static class MetadataTestValues
    {
        public static Random Rnd = new Random();
        public const string MetadatasetName = "Metadataset Name - Selenium";
        public const string MetadatasetDescription = "Metadataset Description";
        public const string FieldName = "Field Name";
        public const string FieldDescription = "Field Description";
        public const string FieldOptions = "Option 1\r\nOption 2\r\nOption 3";
        public const bool FieldRequired = true;
        public const string PathToDescription = "#Description";
        
    }
    [TestFixture(typeof(ChromeDriver))]
    public class MetadataViewTests<TWebDriver> : BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanCreateSimpleMetadataset()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicMetadataSet();

            clickSave();
           
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");
            int id = GetNewlyAddedMetadataId();

             //Assert.AreEqual(FindTestValue(id.ToString()), id.ToString());
            Assert.AreEqual(MetadataTestValues.MetadatasetDescription, GetTextFieldValue(MetadataTestValues.PathToDescription));
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanCreateMetadatasetWithFields()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");

            Navigate(SettingsLabel, MetadataSetsLabel);
            Click(AddId);

            this.FillBasicMetadataSet();
            this.AddAllMetadataFields();

            clickSave();
            int id = GetNewlyAddedMetadataId();

            Assert.Greater(id, 0);

            string description = GetTextFieldValue(MetadataTestValues.PathToDescription);
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");

            Click(GetField(string.Format("#model-row-{0} button.glyphicon-edit", id)));

            Assert.AreEqual(MetadataTestValues.MetadatasetDescription, description);

            //validate there are 2 fields added
            //Assert.AreEqual(8, GetNewlyAddedMetadaSet().Fields.Count);
           
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanReorderMetadatasetFields()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicMetadataSet();
            this.AddAllMetadataFields();

            clickSave();

            //validate creation of the metadaset is successfull
            int id = GetNewlyAddedMetadataId();

            string description = GetTextFieldValue(MetadataTestValues.PathToDescription);
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");
            //Assert.AreEqual(FindTestValue(id.ToString()), id.ToString());
            Assert.AreEqual(MetadataTestValues.MetadatasetDescription, description);

            //validate there are 8 fields added
           // Assert.AreEqual(8, GetNewlyAddedMetadaSet().Fields.Count);

            //try re order of the element
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata/edit/" + id);

            //reorder the elements
            IReadOnlyList<IWebElement> allElements = GetFieldElements();
            //swap Checkboxes and paragraph....move paragraph "up"paragraph will be 2nd from top
            foreach(var e in allElements)
            {
                if (e.FindElement(By.ClassName("title")).Text.Equals("Paragraph"))
                {

                    IWebElement btnUp = e.FindElement(By.ClassName("glyphicon-arrow-up"));
                    JsExecutor.ExecuteScript("arguments[0].focus();", btnUp);
                    btnUp.Click();
                    break;
                }
            }

            Thread.Sleep(500);
             allElements = GetFieldElements();
            //move attachment field "down" -- attachment should be last
            foreach (var e in allElements)
            {
                if (e.FindElement(By.ClassName("title")).Text.Equals("Attachment Field"))
                {
                    IWebElement btnDown = e.FindElement(By.ClassName("glyphicon-arrow-down"));
                    JsExecutor.ExecuteScript("arguments[0].focus();", btnDown);
                    btnDown.Click();
                    break;
                }
            }

            allElements = GetFieldElements();
            //assert --paragraph should be in 2nd and attachment should be last
            bool porder = allElements[1].FindElement(By.ClassName("title")).Text.Equals("Paragraph");
            Assert.AreEqual(true, porder);
            porder = allElements.Last().FindElement(By.ClassName("title")).Text.Equals("Attachment Field");
            Assert.AreEqual(true, porder);
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanEditBasicMetadataSet()
        {
            //create simple metadatset to edit 

            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicMetadataSet();

            clickSave();

            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");
            int id = GetNewlyAddedMetadataId();

            //Assert.AreEqual(FindTestValue(id.ToString()), id.ToString());
            Assert.AreEqual(MetadataTestValues.MetadatasetDescription, GetTextFieldValue(MetadataTestValues.PathToDescription));

            
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata/edit/" + id);

            //edit description field
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.PollingInterval = TimeSpan.FromMilliseconds(10);
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Name")));
            string newName = "Metadta Name" + TestData.LoremIpsum(5, 15);
            IWebElement name = this.Driver.FindElement(By.Id("Name"));//.SendKeys(newName);
            name.Clear();
            name.SendKeys(newName);

            string des = "Description" + TestData.LoremIpsum(5,20);
            IWebElement description = this.Driver.FindElement(By.Id("Description"));//.SendKeys(des);
            description.Clear();
            description.SendKeys(des);
            clickSave();

            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");
            //Assert.AreEqual(FindTestValue(newName), newName);

           // var metadataSet = GetMetadaSetById(id);
           // Assert.AreEqual(newName, metadataSet.Name);
            //Assert.AreEqual(des, metadataSet.Description);

        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanEditMetadataField()
        {
            //create metadataset with 2 field 9Short text and checkboxes to be edit
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicMetadataSet();
            this.AddMetadataFields();

            clickSave();
            int id = GetNewlyAddedMetadataId();

            string description = GetTextFieldValue(MetadataTestValues.PathToDescription);
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata");
            //Assert.AreEqual(FindTestValue(id.ToString()), id.ToString());
            Assert.AreEqual(MetadataTestValues.MetadatasetDescription, description);

            //validate there are 2 fields added
            //Assert.AreEqual(2, GetNewlyAddedMetadaSet().Fields.Count);
            ////

            this.Driver.Navigate().GoToUrl(ManagerUrl + "/metadata/edit/" + id);

            //remove checkbox
            var allElementTypes = GetFieldElements();
            foreach (IWebElement e in allElementTypes)
            {
                if (e.FindElement(By.ClassName("title")).Text.Equals("Checkboxes"))
                {                   
                    IWebElement btnRemove = e.FindElement(By.ClassName("glyphicon-remove"));
                    JsExecutor.ExecuteScript("arguments[0].focus();", btnRemove);
                    btnRemove.Click();
                }
            }

            //checkbox should be gone
            allElementTypes = GetFieldElements();
            Assert.AreEqual(1, allElementTypes.Count); //should be only 1 left
            //and it should be Short text
            string eleTitle = "";
            foreach (IWebElement e in allElementTypes)
            {
                eleTitle = e.FindElement(By.ClassName("title")).Text;    
            }

            Assert.AreEqual("Short text", eleTitle);

            //add paragraph
            AddParagraph();

            //add checkbox
            AddCheckboxes();

            clickSave();

            allElementTypes = GetFieldElements();
            Assert.AreEqual(3, allElementTypes.Count);
        }
        private void clickSave()
        {
            WaitUnitVisibleById(15000, SaveId);

            Click(SaveId);

        }
        private void AddMetadataFields()
        {
            AddShortText();//1st set element field
            AddCheckboxes(); //2nd set element field
        }
       
        private void AddAllMetadataFields()
        {
            AddShortText();//1st set element field
            AddCheckboxes(); //2nd set element field
            AddParagraph();
            AddPageBreak();
            AddMultipleChoices();
            AddDate();
            AddAttachmentField();
            AddDropdown();
        }

        private IReadOnlyList<IWebElement> GetFieldEntries(string fieldEntryTitle)
        {
            IReadOnlyList<IWebElement> entries=null;

            WebDriverWait webDriver = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("field-entry")));

            var allElementTypes = Driver.FindElements(By.ClassName("field-entry"));
            foreach(IWebElement e in allElementTypes)
            {
                if(e.FindElement(By.ClassName("title")).Text.Equals(fieldEntryTitle))
                {
                    entries =  e.FindElements(By.ClassName("input-field"));
                    break;
                }
            }
            return entries;
        }

        private IReadOnlyList<IWebElement> GetFieldElements()
        {
            
            WebDriverWait webDriver = new WebDriverWait(Driver, TimeSpan.FromMinutes(1));
            webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("field-entry")));

            var allElementTypes = Driver.FindElements(By.ClassName("field-entry"));

            return allElementTypes;
        }
        private void AddShortText() /*elemement number is --first element, second, etc) */
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Short text");
            this.Driver.FindElement(By.Id("add-field")).Click();
           
            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Short text");//this.Driver.FindElements(By.ClassName("input-field"));
            for (int i = 0; i < inputs.Count; ++i)
            {
                inputs[i].SendKeys(MetadataTestValues.FieldName + " text Language " + i);
            }
            
           
            checkRequiredCheckbox(MetadataTestValues.FieldRequired);
        }

        private void AddParagraph() /*elemement number is --first element, second, etc) */
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Paragraph");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Paragraph");//this.Driver.FindElements(By.ClassName("input-field"));
            for (int i = 0; i < inputs.Count; ++i)
            {
                inputs[i].SendKeys(MetadataTestValues.FieldName + " paragraph Language " + i);
            }


            checkRequiredCheckbox(MetadataTestValues.FieldRequired);
        }

        private void AddDate() /*elemement number is --first element, second, etc) */
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Date");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Date");//this.Driver.FindElements(By.ClassName("input-field"));
            for (int i = 0; i < inputs.Count; ++i)
            {
                inputs[i].SendKeys(MetadataTestValues.FieldName + " date"); 
            }

            checkRequiredCheckbox(MetadataTestValues.FieldRequired);
        }

        private void AddPageBreak() /*elemement number is --first element, second, etc) */
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Page Break");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Page Break");//this.Driver.FindElements(By.ClassName("input-field"));
            for (int i = 0; i < inputs.Count; ++i)
            {
                inputs[i].SendKeys(MetadataTestValues.FieldName + " page break " + i);
            }
            
        }

        private void AddAttachmentField() /*elemement number is --first element, second, etc) */
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Attachment Field");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Attachment Field");//this.Driver.FindElements(By.ClassName("input-field"));
            if (inputs.Count > 0)
            {
                inputs[0].SendKeys(MetadataTestValues.FieldName + " attachment Eng");

            }
            checkRequiredCheckbox(false);
        }
        private void checkRequiredCheckbox(bool req)
        {
            IWebElement chkReq = this.Driver.FindElement(By.ClassName("field-is-required"));
          
            if (req == true)
            {
                JsExecutor.ExecuteScript("arguments[0].click();", chkReq);
            }
        }
        private void AddCheckboxes()
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Checkboxes");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Checkboxes");//this.Driver.FindElements(By.ClassName("input-field"));
            
            for (int i = 0; i<inputs.Count -1; i++)
            {
                //1st -- name --has 3 entries (eng, french,spanish)
                if (i == 0)
                {
                    inputs[i].SendKeys(MetadataTestValues.FieldName + "checkbox"); //name - in english
                    JsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
                else if (i == 3)
                { // //2nd options 
                    inputs[i].SendKeys(MetadataTestValues.FieldOptions);  //options in english
                    JsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
            }
            

            checkRequiredCheckbox(false);
        }

        private void AddDropdown()
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Dropdown");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Dropdown");//this.Driver.FindElements(By.ClassName("input-field"));

            for (int i = 0; i < inputs.Count - 1; i++)
            {
                //1st -- name --has 3 entries (eng, french,spanish)
                if (i == 0)
                {
                    inputs[i].SendKeys(MetadataTestValues.FieldName + "dd"); //name - in english
                    JsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
                else if (i == 3)
                { // //2nd options 
                    inputs[i].SendKeys(MetadataTestValues.FieldOptions);  //options in english
                    JsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
            }


            checkRequiredCheckbox(true);
        }

        private void AddMultipleChoices()
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Multiple choice");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Multiple choice");//this.Driver.FindElements(By.ClassName("input-field"));

            for (int i = 0; i < inputs.Count - 1; i++)
            {
                //1st -- name --has 3 entries (eng, french,spanish)
                if (i == 0)
                {
                    inputs[i].SendKeys(MetadataTestValues.FieldName + "dd"); //name - in english
                    JsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
                else if (i == 3)
                { // //2nd options 
                    inputs[i].SendKeys(MetadataTestValues.FieldOptions);  //options in english
                    JsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
            }


            checkRequiredCheckbox(true);
        }
        private void FillBasicMetadataSet()
        {
            WebDriverWait webDriver = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Name")));
            this.Driver.FindElement(By.Id("Name")).SendKeys(MetadataTestValues.MetadatasetName + MetadataTestValues.Rnd.Next(1,100));

            webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Description")));
            IWebElement description = this.Driver.FindElement(By.Id("Description"));//.SendKeys(MetadataTestValues.MetadatasetDescription);
            description.SendKeys(MetadataTestValues.MetadatasetDescription);

            JsExecutor.ExecuteScript("$(arguments[0]).change()", description);
        }

        private int GetNewlyAddedMetadataId()
        {
            var contextName = Driver.FindElement(By.CssSelector("div.form-builder")).GetAttribute("id");

            int id = int.Parse(JsExecutor.ExecuteScript(string.Format("return {0}.Id();", contextName)).ToString());

            return id;
        }
    }
}
