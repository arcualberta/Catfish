using Catfish.Tests.Views;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Catfish.Tests.Api
{
    static class ItemTestValues
    {
        public static Random Rnd = new Random();
        public static string Name = "Item Api Name - Selenium" + Rnd.Next(1, 100);
        public static string Description = "EntityType Description";
        public static string EditedName = "Edited Item Name - Selenium" + Rnd.Next(1, 100);
    }


    [TestFixture(typeof(ChromeDriver))]
    public class ItemsControllerTest<TWebDriver> : BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private string indexUrl = "";

        private void Login()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            this.Driver.FindElement(By.Name("password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.TagName("button")).Click();
        }

        private string CreateItem()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            SelectElement typeSelector = new SelectElement(this.Driver.FindElement(By.CssSelector("#field-type-selector")));
            IWebElement nextButton = this.Driver.FindElement(By.Id("add-field"));

            int optionsCount = typeSelector.Options.Count;

            for (int i = 0; i < optionsCount; i++)
            {
                IWebElement option = typeSelector.Options[i];
                typeSelector.SelectByText(option.Text);
                nextButton.Click();
                break; //select 1st one in the list
            }

            // Fill Form Fields
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => drv.FindElement(By.ClassName("form-field-name")));

            IReadOnlyList<IWebElement> fields = this.Driver.FindElements(By.ClassName("form-field"));
            int count = 0;
            foreach (var f in fields)
            {
                IWebElement name = f.FindElement(By.Name("label")); //text field
                string txt = name.Text;

                if (count == 0)
                {
                    txt = ItemTestValues.Name;
                    count++;
                }
                IReadOnlyList<IWebElement> inputs = f.FindElements(By.ClassName("languageInputField"));
                //each field has 3 input fields each for each language -- eng, fr and sp
                for (int i = 0; i < inputs.Count; i++)
                {
                    //grab text field or text area
                    IWebElement textEl;
                    bool bfound = IsElementFound(inputs[i], "input");
                    if (bfound)
                    {
                        textEl = inputs[i].FindElement(By.TagName("input"));
                    }
                    else
                    {
                        textEl = inputs[i].FindElement(By.TagName("textarea"));
                    }

                    if (i == 0)  //eng
                    {
                        textEl.SendKeys(txt);
                    }
                    else if (i == 1)//fr
                    {
                        textEl.SendKeys(txt + " Fr");
                    }
                    else//sp
                    {
                        textEl.SendKeys(txt + " Sp");
                    }
                    textEl.SendKeys(Keys.Tab);
                }
            }

            // Click save
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("save")));

            IWebElement btnSave = this.Driver.FindElement(By.ClassName("save"));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

            ex.ExecuteScript("window.scrollTo(0, 0) ", btnSave);
            Thread.Sleep(500);
            btnSave.Click();

            return Driver.FindElement(By.CssSelector("form input[name='Id']")).GetAttribute("value");
        }

        private bool IsElementFound(IWebElement el, string tagName)
        {
            bool found = false;
            try
            {
                IWebElement elFound = el.FindElement(By.TagName(tagName));
                found = true;
            }
            catch (NoSuchElementException ex)
            {
                found = false;
            }

            return found;
        }

        [Test]
        public void CanGetItem()
        {
            string collecitonId = CreateItem();

            //TODO: Post call
        }

    }
    
}
