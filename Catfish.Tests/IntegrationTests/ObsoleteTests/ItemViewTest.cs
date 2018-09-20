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
    static class ItemTestValues
    {
        public static Random Rnd = new Random();
        public static string Name = "Item Name - Selenium" + Rnd.Next(1,100);
        public static string Description = "EntityType Description";
        public static string EditedName = "Edited Item Name - Selenium" + Rnd.Next(1, 100);
    }
   

    [TestFixture(typeof(ChromeDriver))]
    public class ItemViewTests<TWebDriver> : BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private string indexUrl = "";
        
        protected override void OnSetup()
        {
            this.indexUrl = ManagerUrl + "/items";
        }

        private void Login()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl);
            this.Driver.FindElement(By.Id("login")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            this.Driver.FindElement(By.Name("password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            this.Driver.FindElement(By.TagName("button")).Click();
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanCreateItem()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);
            ClickOnAddBtn();
            SelectEntityType();
            FilledFormFields();
            ClickSave();

            var itemId = GetNewlyAddedItem();

            this.Driver.Navigate().GoToUrl(indexUrl + "/edit/" + itemId);
            IWebElement element = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"));
            Assert.AreEqual(ItemTestValues.Name, element.GetAttribute("value"));
        }

        private IWebElement GetListButton(string itemId, string @class)
        {
            IWebElement btnEdit = null;

            while ((btnEdit = FindElementOnThePage(itemId, @class)) == null)
            {
                IWebElement element = Driver.FindElements(By.CssSelector("div.linkDiv > a.btn-success")).Where(a => a.Text.StartsWith("Next")).FirstOrDefault();
                IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

                ex.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
                element.Click();
            }

            return btnEdit;
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanEditItem()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);
            ClickOnAddBtn();
            SelectEntityType();
            FilledFormFields();
            ClickSave();

            var itemId = GetNewlyAddedItem();

            this.Driver.Navigate().GoToUrl(indexUrl + "/edit/" + itemId);
            IWebElement element = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"));
            Assert.AreEqual(ItemTestValues.Name, element.GetAttribute("value"));

            //edit
            this.Driver.Navigate().GoToUrl(indexUrl);
            IWebElement btnEdit = GetListButton(itemId, "glyphicon-edit");
            ClickButton(btnEdit);

            EditFormFields();
            ClickSave();

            this.Driver.Navigate().GoToUrl(indexUrl);
            btnEdit = GetListButton(itemId, "glyphicon-edit");

            Assert.AreEqual(ItemTestValues.EditedName, FindTestValue(ItemTestValues.EditedName));

        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanDeleteItem()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);
            ClickOnAddBtn();
            SelectEntityType();
            FilledFormFields();
            ClickSave();

            var itemId = GetNewlyAddedItem();

            this.Driver.Navigate().GoToUrl(indexUrl + "/edit/" + itemId);
            IWebElement element = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"));
            Assert.AreEqual(ItemTestValues.Name, element.GetAttribute("value"));

            //Delete
            this.Driver.Navigate().GoToUrl(indexUrl);
            IWebElement btnDelete = GetListButton(itemId, "glyphicon-remove");
            ClickButtonDelete(btnDelete);

            //the item should be gone
            this.Driver.Navigate().GoToUrl(indexUrl + "/edit/" + itemId);

            // Check 404
            Assert.IsTrue(this.Driver.PageSource.Contains("404"));
            Assert.IsTrue(this.Driver.PageSource.Contains("Item was not found"));
        }

        [Ignore("Test needs to be corrected")]
        [Test]
        public void CanLinkItem()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);
            ClickOnAddBtn();
            SelectEntityType();
            FilledFormFields();
            ClickSave();

            var itemId = GetNewlyAddedItem();

            this.Driver.Navigate().GoToUrl(indexUrl + "/edit/" + itemId);
            IWebElement element = Driver.FindElement(By.Id("MetadataSets_0__Fields_0__Values_0__Value"));
            Assert.AreEqual(ItemTestValues.Name, element.GetAttribute("value"));

            //link -- add/remove child item
            this.Driver.Navigate().GoToUrl(indexUrl);
            IWebElement btnLink = GetListButton(itemId, "glyphicon-link");
            ClickButton(btnLink);

            AddChildItem(1);
            var groups = this.Driver.FindElements(By.ClassName("box"));
            SelectElement typeSelector = new SelectElement(groups[2].FindElement(By.Name("childrenList")));
            Assert.AreEqual(2, typeSelector.Options.Count);

            //remove childItems
            RemoveChildItem(2); // This switched from 1 to 2 becuase the success message has a class of "box"
            groups = this.Driver.FindElements(By.ClassName("box"));
            typeSelector = new SelectElement(groups[2].FindElement(By.Name("childrenList")));
            Assert.AreEqual(0, typeSelector.Options.Count);

            //add/remove related item
            AddChildItem(3);
            groups = this.Driver.FindElements(By.ClassName("box"));
            typeSelector = new SelectElement(groups[3].FindElement(By.Name("childrenList")));
            Assert.AreEqual(2, typeSelector.Options.Count);

            RemoveChildItem(3);
            groups = this.Driver.FindElements(By.ClassName("box"));
            typeSelector = new SelectElement(groups[3].FindElement(By.Name("childrenList")));
            Assert.AreEqual(0, typeSelector.Options.Count);
        }

        private void AddChildItem(int groupId)// 1- Child Item, 2 - Related Item
        {
            var groups = this.Driver.FindElements(By.ClassName("box"));
            SelectElement typeSelector = new SelectElement(groups[groupId].FindElement(By.Name("masterList")));
            IWebElement addButton = groups[groupId].FindElement(By.ClassName("glyphicon-arrow-left"));
            IWebElement saveButton = this.Driver.FindElement(By.Id("toolbar_save_button"));
            int optionsCount = typeSelector.Options.Count;

            List<string> options = new List<string>();
            for (int i = 0; i < optionsCount && i < 2; ++i)
            {
                options.Add(typeSelector.Options[i].Text);
            }

            // Ignore first empty option. Add all fields and enter data 
            foreach (string option in options)
            {
                typeSelector = new SelectElement(groups[groupId].FindElement(By.Name("masterList")));
                typeSelector.SelectByText(option);
                ClickButton(addButton);
            }

            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
            ex.ExecuteScript("window.scrollTo(0, 0); ");

            ClickButton(saveButton);

            WebDriverWait wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("sys-message")));

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("sys-message")));
        }

        private void RemoveChildItem(int groupId)// 1- Child Item, 2 - Related Item
        {
            var groups = this.Driver.FindElements(By.ClassName("box"));
            SelectElement typeSelector = new SelectElement(groups[groupId].FindElement(By.Name("childrenList")));
            IWebElement removeButton = groups[groupId].FindElement(By.ClassName("glyphicon-arrow-right"));
            IWebElement saveButton = this.Driver.FindElement(By.Id("toolbar_save_button"));
            int optionsCount = typeSelector.Options.Count;

            List<string> options = new List<string>();
            for (int i = 0; i < 2 && i < optionsCount; ++i)
            {
                options.Add(typeSelector.Options[i].Text);
            }

            // Ignore first empty option. Add all fields and enter data 
            foreach (string option in options)
            {
                typeSelector = new SelectElement(groups[groupId].FindElement(By.Name("childrenList")));
                typeSelector.SelectByText(option);
                ClickButton(removeButton);
            }

            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
            ex.ExecuteScript("window.scrollTo(0, 0); ");

            ClickButton(saveButton);

            WebDriverWait wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("sys-message")));

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("sys-message")));
        }

        private void ClickOnAddBtn()
        {
            this.Driver.FindElement(By.LinkText("Add new")).Click();
        }
       private void SelectEntityType()
        {
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
        }

        private void FilledFormFields()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => drv.FindElement(By.ClassName("form-field-name")));

            IReadOnlyList<IWebElement> fields = this.Driver.FindElements(By.ClassName("form-field"));
            int count = 0;
            foreach(var f in fields)
            {
                IWebElement name = f.FindElement(By.Name("label")); //text field
                string txt = name.Text;

                if(count == 0)
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

        private void EditFormFields()
        {
            IReadOnlyList<IWebElement> fields = this.Driver.FindElements(By.ClassName("form-field"));
            int count = 0;
            foreach (var f in fields)
            {
                IWebElement name = f.FindElement(By.Name("label"));
                string txt = name.Text;

                if (count == 0)
                {
                    txt = ItemTestValues.EditedName;
                    count++;
                }
                IReadOnlyList<IWebElement> inputs = f.FindElements(By.ClassName("languageInputField"));
                //each field has 3 input fields each for each language -- eng, fr and sp
                for (int i = 0; i < inputs.Count; i++)
                {
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
                        textEl.Clear();
                        textEl.SendKeys(txt);
                    }
                    else if (i == 1)//fr
                    {
                        textEl.Clear();
                        textEl.SendKeys(txt + " Fr");
                    }
                    else//sp
                    {
                        textEl.Clear();
                        textEl.SendKeys(txt + " Sp");
                    }
                    textEl.SendKeys(Keys.Tab);

                }

            }
        }
        private void ClickSave()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
           wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("save")));

            IWebElement btnSave = this.Driver.FindElement(By.ClassName("save"));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
          
            ex.ExecuteScript("window.scrollTo(0, 0) ", btnSave);
            Thread.Sleep(500);
            btnSave.Click();

        }

        private void ClickButton(IWebElement btn)
        {
            
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

            ex.ExecuteScript("arguments[0].focus(); ", btn);
            Thread.Sleep(500);
            btn.Click();
            //ex.ExecuteScript("arguments[0].click();", btn);
        }

        private void ClickButtonDelete(IWebElement btn)
        {
            
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

            ex.ExecuteScript("arguments[0].focus(); ", btn);
          
            btn.Click();

            //have to click on'ok' on alert pop up 
            IAlert alert = this.Driver.SwitchTo().Alert();
          
            alert.Accept();
        }

        private string GetNewlyAddedItem()
        {
            var itemId = Driver.FindElement(By.CssSelector("form input[name='Id']")).GetAttribute("value");

            return itemId;
        }


        private string FindTestValue(string expectedValue)
        {
            string val = "";
            var cols = this.Driver.FindElements(By.TagName("td"));

            foreach (var col in cols)
            {
                if (col.Text.Contains(expectedValue)) //Item's name will display in all 3 lang separated by '/' -- eng is the first one
                {
                    val = col.Text.Split('/')[0].Trim();
                    break;
                }
            }
            return val;
        }

        private IWebElement FindElementOnThePage(string searchText, string className)
        {
            IWebElement el=null;
            IReadOnlyList<IWebElement> rows = this.Driver.FindElements(By.TagName("tr"));
            foreach(IWebElement r in rows)
            {
                    if(r.Text.Contains(searchText))
                    {
                        el = r.FindElement(By.ClassName(className));
                        return el;
                    } 
            }
            return el;
        }
        
    }
}
