using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
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
    public class EntityContainerTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        const string PAGE_TYPE = "Entity Container";
        const string PAGE_TITLE = "Entity Container Test";
        private IWebDriver Driver;
        private string ManagerUrl;
        
        private string customMapping;

        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";
         
            Login();
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
        [TestCase]
        public void CanCreateAPage()
        {
            bool pageTypeExisted = VerifyPageTypeExists();
            if(pageTypeExisted)
            {
                CreateAPage();
            }

            bool pageCreated = VerifyPageCreated();
            Assert.AreEqual(true, pageCreated);

            bool addAttributeMapping = VerifyAttributeMappingAdded();
            Assert.AreEqual(true, addAttributeMapping);
        }
        [TestCase]
        public void CanReorderMapping()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/page");

            IWebElement element = this.Driver.FindElement(By.LinkText(PAGE_TITLE));


            Actions builder = new Actions(this.Driver);
            builder.MoveToElement(element, 10, 5);

            builder.Click();
            builder.Build().Perform();

            ScrollBottom();

            var tbl = this.Driver.FindElement(By.Id("AttributeMappingTable"));
            var rows = tbl.FindElements(By.TagName("tr"));
            List<string> originalOrder = new List<string>();

            foreach(var r in rows)
            { originalOrder.Add(r.Text); }

            //move the second to the first
            IWebElement moveUp = rows[1].FindElement(By.ClassName("glyphicon-arrow-up"));
            ElementFocus(moveUp);
            moveUp.Click();

            tbl = this.Driver.FindElement(By.Id("AttributeMappingTable"));
            rows = tbl.FindElements(By.TagName("tr"));
            Assert.AreEqual(originalOrder.ElementAt(1), rows[0].Text);

        }
        [TestCase]
        public void CanDeleteMapping()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/page");

            IWebElement element = this.Driver.FindElement(By.LinkText(PAGE_TITLE));


            Actions builder = new Actions(this.Driver);
            builder.MoveToElement(element, 10, 5);

            builder.Click();
            builder.Build().Perform();

            ScrollBottom();

            var tbl = this.Driver.FindElement(By.Id("AttributeMappingTable"));
            var rows = tbl.FindElements(By.TagName("tr"));

            int initialNumOfFields = rows.Count - 1; //don't count the dropdownList -- 3

            //delete the last mapping
            IWebElement delBtn = rows[initialNumOfFields - 1].FindElement(By.ClassName("glyphicon-minus-sign"));
            ElementFocus(delBtn);
            delBtn.Click();

            tbl = this.Driver.FindElement(By.Id("AttributeMappingTable"));
            rows = tbl.FindElements(By.TagName("tr"));
            int numOfFiledAfterDeletion = rows.Count - 1; // 2

            Assert.AreEqual(initialNumOfFields - 1, numOfFiledAfterDeletion);
        }
        private bool VerifyPageTypeExists()
        {
            try
            {
                this.Driver.Navigate().GoToUrl(ManagerUrl + "/template/pagelist");
                var element = this.Driver.FindElement(By.LinkText(PAGE_TYPE));

                if (element.Text.Equals(PAGE_TYPE))
                    return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
            return false;
        }

        private bool VerifyPageCreated()
        {
            try
            {
                this.Driver.Navigate().GoToUrl(ManagerUrl + "/page");
                var element = this.Driver.FindElement(By.LinkText(PAGE_TITLE));

                if (element.Text.Equals(PAGE_TITLE))
                    return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
            return false;
        }

        private bool VerifyAttributeMappingAdded()
        {
            bool added = false;
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/page");
           
            IWebElement element = this.Driver.FindElement(By.LinkText(PAGE_TITLE));
           

            Actions builder = new Actions(this.Driver);
            builder.MoveToElement(element,10,5);
           
            builder.Click();
            builder.Build().Perform();
          
            var tbl = this.Driver.FindElement(By.Id("AttributeMappingTable"));
            var rows = tbl.FindElements(By.TagName("tr"));

           
            foreach(var r in rows)
            {
                if(r.Text.Equals(customMapping))
                {
                    added = true;
                    break;
                }
            }
            return added;
        }
        private void CreateAPage() {
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/page");
            this.Driver.FindElement(By.CssSelector(".add")).Click();
           
            IEnumerable<IWebElement> elements = this.Driver.FindElements(By.CssSelector("#create-new .templates"));

            foreach (var element in elements)
            {
                if (element.FindElement(By.CssSelector("h3")).Text == PAGE_TYPE)
                {
                    element.FindElement(By.CssSelector(".preview")).Click();
                    break;
                }
            }

           this.Driver.FindElement(By.Id("Page_Title")).SendKeys(PAGE_TITLE);
           
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("Regions_0__Body_selectedField")));            
            int optionsCount = select.Options.Count;
                       
            for (int i = 0; i < optionsCount; i++)
            {
                IWebElement option = select.Options[i]; 
                select.SelectByText(option.Text);
                customMapping = option.Text;
                this.Driver.FindElement(By.ClassName("glyphicon-plus-sign")).Click();
                
                break; //only add 1 metadataset
            }

            IWebElement btnSave = this.Driver.FindElement(By.ClassName("publish"));
            IJavaScriptExecutor jex = (IJavaScriptExecutor)Driver;
            ScrollTop();
            jex.ExecuteScript("arguments[0].focus(); ", btnSave);
            
            btnSave.Click();
        }

        private void ScrollTop()
        {
            IJavaScriptExecutor jex = (IJavaScriptExecutor)Driver;
            jex.ExecuteScript("scroll(0, -250);");
        }

        private void ScrollBottom()
        {
            IJavaScriptExecutor jex = (IJavaScriptExecutor)Driver;
            jex.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
        }

        private void ElementFocus(IWebElement element)
        {
            IJavaScriptExecutor jex = (IJavaScriptExecutor)Driver;
            jex.ExecuteScript("arguments[0].focus(); ", element);

        }
       
    }
}
