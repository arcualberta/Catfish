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
   
    static class FormTestValues
    {
        public static Random Rnd = new Random();
        public static string Name = "Form Name - Selenium" + Rnd.Next(1,100);
        public static string Description = "Form Description";
        public static string FieldName = "Field Name";
        public static string FieldDescription = "Field Description";
        public static string FieldOptions = "Option 1\r\nOption 2\r\nOption 3";
        public static bool FieldRequired = true;
        
    }
    [TestFixture(typeof(ChromeDriver))]
    public class FormViewTests<TWebDriver> : BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private string formUrl;
        
        protected override void OnSetup()
        {
            this.formUrl = ManagerUrl + "/FormTemplates";
        }

        private void Login()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            this.Driver.FindElement(By.Name("password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.TagName("button")).Click();
        }

        [Test]
        public void CanCreateSimpleForm()
        {
            this.Driver.Navigate().GoToUrl(formUrl);

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicFormInfo();

            clickSave();
           
            this.Driver.Navigate().GoToUrl(formUrl);
            var form = GetNewlyAddedForm();


            Assert.AreEqual(form.Id.ToString(), FindTestValue(form.Id.ToString()));
            Assert.AreEqual(FormTestValues.Description, form.Description);
        }

       [Test]
        public void CanCreateFormWithFields()
        {
            this.Driver.Navigate().GoToUrl(formUrl);

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicFormInfo();
            this.AddAllMetadataFields();

            clickSave();
            var form = this.GetNewlyAddedForm();
           

            
            this.Driver.Navigate().GoToUrl(formUrl);
            Assert.AreEqual(form.Id.ToString(), FindTestValue(form.Id.ToString()));
            Assert.AreEqual(FormTestValues.Description, form.Description);
        }

        [Test]
        public void CanReorderFormFields()
        {
            this.Driver.Navigate().GoToUrl(formUrl);

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicFormInfo();
            this.AddAllMetadataFields();

            clickSave();

            //validate creation of the metadaset is successfull
            var form = GetNewlyAddedForm();
            
            this.Driver.Navigate().GoToUrl(formUrl);
            Assert.AreEqual(form.Id.ToString(),FindTestValue(form.Id.ToString()));
            Assert.AreEqual(FormTestValues.Description, form.Description);
            //validate there are 8 fields added
            Assert.AreEqual(8, form.Fields.Count);

            //try re order of the element
            this.Driver.Navigate().GoToUrl(formUrl + "/edit/" + form.Id);
            
            //reorder the elements
            IReadOnlyList<IWebElement> allElements = GetFieldElements();
            //swap Checkboxes and paragraph....move paragraph "up"paragraph will be 2nd from top
            foreach(var e in allElements)
            {
                if (e.FindElement(By.ClassName("title")).Text.Equals("Paragraph"))
                {
                    IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

                    IWebElement btnUp = e.FindElement(By.ClassName("glyphicon-arrow-up"));
                    ex.ExecuteScript("arguments[0].focus();", btnUp);
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
                    IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

                    IWebElement btnDown = e.FindElement(By.ClassName("glyphicon-arrow-down"));
                    ex.ExecuteScript("arguments[0].focus();", btnDown);
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

        [Test]
        public void CanEditBasicFormInfo()
        {
            //create simple form to edit 

            this.Driver.Navigate().GoToUrl(formUrl);

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicFormInfo();

            clickSave();
            //validate the form was created
            this.Driver.Navigate().GoToUrl(formUrl);
            var form = GetNewlyAddedForm();

            Assert.AreEqual(form.Id.ToString(),FindTestValue(form.Id.ToString()));
            Assert.AreEqual(FormTestValues.Description, form.Description);

            
            this.Driver.Navigate().GoToUrl(formUrl + "/edit/" + form.Id);

            //edit description field
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.PollingInterval = TimeSpan.FromMilliseconds(10);
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Name")));
            string newName = "Edited Form Name" + FormTestValues.Rnd.Next(1,100);
            IWebElement name = this.Driver.FindElement(By.Id("Name"));//.SendKeys(newName);
            name.Clear();
            name.SendKeys(newName);

            string des = "Description " + TestData.LoremIpsum(5,10);
            IWebElement description = this.Driver.FindElement(By.Id("Description"));//.SendKeys(des);
            description.Clear();
            description.SendKeys(des);
            clickSave();

           
            this.Driver.Navigate().GoToUrl(formUrl);
            form = GetNewlyAddedForm();
            Assert.AreEqual(newName, FindTestValue(newName));
            Assert.AreEqual(des, form.Description);
        }
      [Test]
        public void CanEditFormField()
        {
            //create metadataset with 2 field 9Short text and checkboxes to be edit
            this.Driver.Navigate().GoToUrl(formUrl);

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicFormInfo();
            this.AddMetadataFields();

            clickSave();
            var form = GetNewlyAddedForm();
         
            this.Driver.Navigate().GoToUrl(formUrl);
            Assert.AreEqual(form.Id.ToString(),FindTestValue(form.Id.ToString()));
          

            this.Driver.Navigate().GoToUrl(formUrl + "/edit/" + form.Id);

            //remove checkbox
            var allElementTypes = GetFieldElements();
            foreach (IWebElement e in allElementTypes)
            {
                if (e.FindElement(By.ClassName("title")).Text.Equals("Checkboxes"))
                {
                    IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
                   
                    IWebElement btnRemove = e.FindElement(By.ClassName("glyphicon-remove"));
                    ex.ExecuteScript("arguments[0].focus();", btnRemove);
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
            //this.Driver.FindElement(By.ClassName("save")).Click(); ==> this option sometimes throw error, element not found!!!
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("save")));

            IWebElement btnSave = this.Driver.FindElement(By.ClassName("save"));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
          
            ex.ExecuteScript("arguments[0].focus(); ", btnSave);
            Thread.Sleep(500);
            btnSave.Click();

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
            if (inputs.Count > 0)
            {
                inputs[0].SendKeys(MetadataTestValues.FieldName + " text Eng");
                inputs[1].SendKeys(MetadataTestValues.FieldName + " text Fr");
                inputs[2].SendKeys(MetadataTestValues.FieldName + " text Sp");
            }
            
           
            checkRequiredCheckbox(MetadataTestValues.FieldRequired);
        }

        private void AddParagraph() /*elemement number is --first element, second, etc) */
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Paragraph");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Paragraph");//this.Driver.FindElements(By.ClassName("input-field"));
            if (inputs.Count > 0)
            {
                inputs[0].SendKeys(MetadataTestValues.FieldName + " paragraph Eng");
                inputs[1].SendKeys(MetadataTestValues.FieldName + " paragraph Fr");
                inputs[2].SendKeys(MetadataTestValues.FieldName + " paragraph Sp");
            }


            checkRequiredCheckbox(MetadataTestValues.FieldRequired);
        }

        private void AddDate() /*elemement number is --first element, second, etc) */
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Date");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Date");//this.Driver.FindElements(By.ClassName("input-field"));
            if (inputs.Count > 0)
            {
                inputs[0].SendKeys(MetadataTestValues.FieldName + " date Eng");
                
            }
            checkRequiredCheckbox(MetadataTestValues.FieldRequired);
        }

        private void AddPageBreak() /*elemement number is --first element, second, etc) */
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Page Break");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Page Break");//this.Driver.FindElements(By.ClassName("input-field"));
            if (inputs.Count > 0)
            {
                inputs[0].SendKeys(MetadataTestValues.FieldName + " page break Eng");
                inputs[1].SendKeys(MetadataTestValues.FieldName + " page break Fr");
                inputs[3].SendKeys(MetadataTestValues.FieldDescription + " page break Eng");
                inputs[4].SendKeys(MetadataTestValues.FieldDescription + " page break Fr");
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
                IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
                ex.ExecuteScript("arguments[0].click();", chkReq);
            }
        }
        private void AddCheckboxes()
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText("Checkboxes");
            this.Driver.FindElement(By.Id("add-field")).Click();

            IReadOnlyList<IWebElement> inputs = GetFieldEntries("Checkboxes");//this.Driver.FindElements(By.ClassName("input-field"));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            
            for (int i = 0; i<inputs.Count -1; i++)
            {
                //1st -- name --has 3 entries (eng, french,spanish)
                if (i == 0)
                {
                    inputs[i].SendKeys(MetadataTestValues.FieldName + "checkbox"); //name - in english
                    jsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
                else if (i == 3)
                { // //2nd options 
                    inputs[i].SendKeys(MetadataTestValues.FieldOptions);  //options in english
                    jsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
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
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;

            for (int i = 0; i < inputs.Count - 1; i++)
            {
                //1st -- name --has 3 entries (eng, french,spanish)
                if (i == 0)
                {
                    inputs[i].SendKeys(MetadataTestValues.FieldName + "dd"); //name - in english
                    jsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
                else if (i == 3)
                { // //2nd options 
                    inputs[i].SendKeys(MetadataTestValues.FieldOptions);  //options in english
                    jsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
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
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;

            for (int i = 0; i < inputs.Count - 1; i++)
            {
                //1st -- name --has 3 entries (eng, french,spanish)
                if (i == 0)
                {
                    inputs[i].SendKeys(MetadataTestValues.FieldName + "dd"); //name - in english
                    jsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
                else if (i == 3)
                { // //2nd options 
                    inputs[i].SendKeys(MetadataTestValues.FieldOptions);  //options in english
                    jsExecutor.ExecuteScript("$(arguments[0]).change()", inputs[i]);
                }
            }


            checkRequiredCheckbox(true);
        }
        private void FillBasicFormInfo()
        {
            WebDriverWait webDriver = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Name")));
            this.Driver.FindElement(By.Id("Name")).SendKeys(FormTestValues.Name);

            webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Description")));
            IWebElement description = this.Driver.FindElement(By.Id("Description"));//.SendKeys(MetadataTestValues.MetadatasetDescription);
            description.SendKeys(FormTestValues.Description);

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            jsExecutor.ExecuteScript("$(arguments[0]).change()", description);
        }

        private string FindTestValue(string expectedValue)
        {
            string val = "";
            var cols = this.Driver.FindElements(By.TagName("td"));

            foreach (var col in cols)
            {
                if (col.Text.Equals(expectedValue))
                {
                    val = col.Text;
                    break;
                }
            }
            return val;
        }

       

       
        private Form GetNewlyAddedForm()
        {
            CatfishDbContext db = new CatfishDbContext();
            if (db.Database.Connection.State == ConnectionState.Closed)
            {
                db.Database.Connection.Open();
            }
            // var metadata = db.MetadataSets.OrderByDescending(i => i.Id).First();
            var forms = db.FormTemplates.OrderByDescending(i => i.Id).First();
            return forms;
        }
       
    }
}
