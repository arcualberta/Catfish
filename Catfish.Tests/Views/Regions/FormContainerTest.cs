﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Tests.Views.Regions
{
    [TestFixture(typeof(ChromeDriver))]
    public class FormContainerTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        const string PAGE_TYPE = "TESTFORMPAGETYPE";
        const string FORM = "TESTFORM";
        const string FORM_PAGE = "TESTFORMPAGE";
        const string FORM_NAVIGATION = "formcontainertest";
        const string FORM_CSS_ID = "mytestformonpage";

        private IWebDriver Driver;
        private string ManagerUrl;
        private string FormUrl;
        
        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";
            this.FormUrl = ConfigurationManager.AppSettings["ServerUrl"] + "home/" + FORM_NAVIGATION;

            Login();
            VerifyPageTypeExists();
            VerifyFormExists();
            VerifyPageExists();
        }

        [TearDown]
        public void TearDown()
        {
            this.Driver.Close();
        }

        private void Login()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            this.Driver.FindElement(By.Name("password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.TagName("button")).Click();
        }

        private void AddFormField(string fieldType, bool isRequired)
        {
            IReadOnlyList<IWebElement> inputs;

            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("field-type-selector")));
            select.SelectByText(fieldType);
            select.WrappedElement.SendKeys(Keys.Tab);
            this.Driver.FindElement(By.Id("add-field")).Click();
            this.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            IWebElement fieldPanel = this.Driver.FindElements(By.CssSelector("div.box.field-entry")).Last();

            // Set the name
            inputs = fieldPanel.FindElements(By.CssSelector(".languageInputField input.input-field[name^='Name']"));

            for(int i = 0; i < inputs.Count; ++i)
            {
                inputs[i].SendKeys(MetadataTestValues.FieldName);
            }

            // Set the description
            inputs = fieldPanel.FindElements(By.CssSelector(".languageInputField textarea.input-field[name^='Description']"));

            for (int i = 0; i < inputs.Count; ++i)
            {
                inputs[i].SendKeys(MetadataTestValues.FieldDescription);
            }

            // Set required option
            if (isRequired)
            {
                fieldPanel.FindElement(By.CssSelector(".field-is-required")).Click();
            }
        }

        private void VerifyFormExists()
        {
            bool buildForm = false;
            try
            {
                this.Driver.Navigate().GoToUrl(ManagerUrl + "/FormTemplates");
                buildForm = !this.Driver.FindElements(By.CssSelector(".container .table tr td")).Where(f => f.Text == FORM).Any();
            }
            catch (NoSuchElementException ex)
            {
                buildForm = true;
            }

            if (buildForm) { 
                this.Driver.Navigate().GoToUrl(ManagerUrl + "/FormTemplates/edit");
                this.Driver.FindElement(By.Id("Name")).SendKeys(FORM);
                this.Driver.FindElement(By.Id("Description")).SendKeys("This is an autogenerated form template to test the form page.");
                this.Driver.FindElement(By.Id("Description")).SendKeys(Keys.Tab);

                AddFormField("Short text", true);
                AddFormField("Paragraph", false);

                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
                wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("save")));

                IWebElement btnSave = this.Driver.FindElement(By.ClassName("save"));
                IJavaScriptExecutor jex = (IJavaScriptExecutor)Driver;

                jex.ExecuteScript("arguments[0].focus(); ", btnSave);
                btnSave.Click();
            }
        }

        private void VerifyPageTypeExists()
        {
            try
            {
                this.Driver.Navigate().GoToUrl(ManagerUrl + "/template/pagelist");
                var element = this.Driver.FindElement(By.LinkText(PAGE_TYPE));
            }catch(NoSuchElementException ex)
            {
                this.Driver.Navigate().GoToUrl(ManagerUrl + "/template/page");
                this.Driver.FindElement(By.Id("Template_Name")).SendKeys(PAGE_TYPE);
                this.Driver.FindElement(By.Id("Template_Description")).SendKeys("This is an autogenerated page template to test the form page.");
                this.Driver.FindElement(By.Id("newregionName")).SendKeys("Form Test");
                this.Driver.FindElement(By.Id("newregionInternalId")).SendKeys("form_test");
                this.Driver.FindElement(By.Id("newregionType")).Click();
                this.Driver.FindElement(By.CssSelector("option[value='Catfish.Models.Regions.FormContainer']")).Click();
                this.Driver.FindElement(By.Id("btnAddRegion")).Click();
                this.Driver.FindElement(By.CssSelector(".save.submit")).Click();
            }
        }

        private void VerifyPageExists()
        {
            try
            {
                this.Driver.Navigate().GoToUrl(ManagerUrl + "/page");
                var element = this.Driver.FindElement(By.LinkText(FORM_NAVIGATION));
            }
            catch (NoSuchElementException ex)
            {
                this.Driver.FindElement(By.CssSelector(".add")).Click();
                IEnumerable<IWebElement> elements = this.Driver.FindElements(By.CssSelector("#create-new .templates"));

                foreach(var element in elements)
                {
                    if(element.FindElement(By.CssSelector("h3")).Text == PAGE_TYPE)
                    {
                        element.FindElement(By.CssSelector(".preview")).Click();
                        break;
                    }
                }

                this.Driver.FindElement(By.Id("Page_Title")).SendKeys(FORM_PAGE);
                this.Driver.FindElement(By.Id("Page_NavigationTitle")).SendKeys(FORM_NAVIGATION);
                this.Driver.FindElement(By.Id("btn_form_test")).Click();

                var region = this.Driver.FindElement(By.CssSelector(".region-body.active"));
                int i = 0;
                for(; i < 10; ++i)
                {
                    try{
                        region.FindElement(By.Id(string.Format("Regions_{0}__IsNew", i)));
                        break;
                    }catch(NoSuchElementException noelement)
                    {
                        // Region not found
                    }
                }

                SelectElement select = new SelectElement(region.FindElement(By.Id(string.Format("Regions_{0}__Body_FormId", i))));
                select.SelectByText(FORM);

                region.FindElement(By.Id(string.Format("Regions_{0}__Body_CssId", i))).SendKeys(FORM_CSS_ID);

                // At the moment assume we have the default eneity type and collection.

                IWebElement btnSave = this.Driver.FindElement(By.ClassName("publish"));
                IJavaScriptExecutor jex = (IJavaScriptExecutor)Driver;

                jex.ExecuteScript("arguments[0].focus(); ", btnSave);
                btnSave.Click();
            }
        }

        public void SubmitForm(IWebElement region, bool tryNo = false)
        {
            region.FindElement(By.CssSelector("input[name='submit']")).Click();

            if (tryNo)
            {
                region.FindElement(By.CssSelector("input[name='no']")).Click();
                region.FindElement(By.CssSelector("input[name='submit']")).Click();
            }

            region.FindElement(By.CssSelector("input[name='yes']")).Click();
        }

        [Test]
        public void TestBasicForm()
        {
            this.Driver.Navigate().GoToUrl(FormUrl);
            IWebElement region = this.Driver.FindElement(By.Id(FORM_CSS_ID));
            IReadOnlyList<IWebElement> elements = region.FindElements(By.CssSelector("div.input"));

            elements[0].FindElement(By.CssSelector("input[id$='__Value']")).SendKeys("Field 1");
            elements[1].FindElement(By.CssSelector("textarea[id$='__Value']")).SendKeys("Field 2");

            SubmitForm(region, true);
        }

        [Test]
        public void TestBasicFormRequired()
        {
            this.Driver.Navigate().GoToUrl(FormUrl);
            IWebElement region = this.Driver.FindElement(By.Id(FORM_CSS_ID));
            IReadOnlyList<IWebElement> elements = region.FindElements(By.CssSelector("div.input"));

            SubmitForm(region);

            Assert.IsNotEmpty(elements[0].FindElement(By.CssSelector(".error.form-error")).Text);

            elements[0].FindElement(By.CssSelector("input[id$='__Value']")).SendKeys("Field 1");
            elements[1].FindElement(By.CssSelector("textarea[id$='__Value']")).SendKeys("Field 2");

            SubmitForm(region);
        }
    }
}
