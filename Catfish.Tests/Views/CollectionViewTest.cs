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

    static class CollectionTestValues
    {
        public static Random Rnd = new Random();
        public static string Name = "Collection Name - Selenium" + Rnd.Next(1,100);
        public static string Description = "Collection Description";
        public static string EditedName = "Edited Collection Name - Selenium" + Rnd.Next(1, 100);
    }
   

    [TestFixture(typeof(ChromeDriver))]
    public class CollectionViewTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver Driver;
        private string ManagerUrl;
        private string indexUrl = "";

        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";
            this.indexUrl = ManagerUrl + "/collections";
            this.Login();
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

      [Test]
        public void CanCreateCollection()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);
            ClickOnAddBtn();
            SelectEntityType();
            filledFormFields();
            clickSave();

            var item = GetNewlyAddedCollection();

           Assert.AreEqual(CollectionTestValues.Name, item.Name);
        }

        [Test]
        public void CanEditCollection()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);
            ClickOnAddBtn();
            SelectEntityType();
            filledFormFields();
            clickSave();

            var col = GetNewlyAddedCollection();

            Assert.AreEqual(CollectionTestValues.Name, col.Name);

            //edit
            IWebElement btnEdit = FindElementOnThePage(col.Id.ToString(), "glyphicon-edit");
            clickButton(btnEdit);

            editFormFields();
            clickSave();

            this.Driver.Navigate().GoToUrl(indexUrl);
         
            Assert.AreEqual(CollectionTestValues.EditedName, FindTestValue(CollectionTestValues.EditedName));

        }
        [Test]
        public void CanDeleteCollection()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);
            ClickOnAddBtn();
            SelectEntityType();
            filledFormFields();
            clickSave();

            var col = GetNewlyAddedCollection();

            Assert.AreEqual(CollectionTestValues.Name, col.Name);

            //Delete
            IWebElement btnDelete = FindElementOnThePage(col.Id.ToString(), "glyphicon-remove");
            clickButtonDelete(btnDelete);

            Thread.Sleep(500);
            //the item should be gone
            Collection delCollection = GetCollectionById(col.Id);

            Assert.AreEqual(null, delCollection);
        }
        [Test]
        public void CanLinkItem()
        {
            this.Driver.Navigate().GoToUrl(indexUrl);
            ClickOnAddBtn();
            SelectEntityType();
            filledFormFields();
            clickSave();

            var col = GetNewlyAddedCollection();

            Assert.AreEqual(CollectionTestValues.Name, col.Name);

            //link -- add/remove child item
            IWebElement btnLink = FindElementOnThePage(col.Id.ToString(), "glyphicon-link");
            clickButton(btnLink);

            AddChildCollection();
            var groups = this.Driver.FindElements(By.ClassName("box"));
            SelectElement typeSelector = new SelectElement(groups[1].FindElement(By.Name("childrenList")));
            Assert.AreEqual(2, typeSelector.Options.Count);

            //remove childItems
            RemoveChildCollection();
            Assert.AreEqual(0, typeSelector.Options.Count);

            //add/remove related item
            AddChildItem();
            typeSelector = new SelectElement(groups[2].FindElement(By.Name("childrenList")));
            Assert.AreEqual(2, typeSelector.Options.Count);

            RemoveChildItem();
            Assert.AreEqual(0, typeSelector.Options.Count);
        }

        private void AddChildCollection()
        {
            var groups = this.Driver.FindElements(By.ClassName("box"));
            SelectElement typeSelector = new SelectElement(groups[1].FindElement(By.Name("masterList")));
            IWebElement addButton = groups[1].FindElement(By.ClassName("glyphicon-arrow-left"));
            IWebElement saveButton = groups[1].FindElement(By.ClassName("save"));
            int optionsCount = typeSelector.Options.Count;
            // Ignore first empty option. Add all fields and enter data 
            for (int i = 0; i < optionsCount; i++)
            {
                IWebElement option = typeSelector.Options[i];
                typeSelector.SelectByText(option.Text);
                clickButton(addButton);
                if (i == 1)
                    break; //add 2 items only
            }

            clickButton(saveButton);
        }

        private void RemoveChildCollection()
        {
            var groups = this.Driver.FindElements(By.ClassName("box"));
            SelectElement typeSelector = new SelectElement(groups[1].FindElement(By.Name("childrenList")));
            IWebElement removeButton = groups[1].FindElement(By.ClassName("glyphicon-arrow-right"));
            IWebElement saveButton = groups[1].FindElement(By.ClassName("save"));
            int optionsCount = typeSelector.Options.Count;
            // Ignore first empty option. Add all fields and enter data 
            for (int i = 0; i < optionsCount; i++)
            {
                IWebElement option = typeSelector.Options[0];
                typeSelector.SelectByText(option.Text);
                clickButton(removeButton);
            }

            clickButton(saveButton);
        }
        private void AddChildItem()
        {
            var groups = this.Driver.FindElements(By.ClassName("box"));
            SelectElement typeSelector = new SelectElement(groups[2].FindElement(By.Name("masterList")));
            IWebElement addButton = groups[2].FindElement(By.ClassName("glyphicon-arrow-left"));
            IWebElement saveButton = groups[2].FindElement(By.ClassName("save"));
            int optionsCount = typeSelector.Options.Count;
            // Ignore first empty option. Add all fields and enter data 
            for (int i = 0; i < optionsCount; i++)
            {
                IWebElement option = typeSelector.Options[i];
                typeSelector.SelectByText(option.Text);
                clickButton(addButton);
                if (i == 1)
                    break; //add 2 items only
            }

            clickButton(saveButton);
        }

        private void RemoveChildItem()
        {
            var groups = this.Driver.FindElements(By.ClassName("box"));
            SelectElement typeSelector = new SelectElement(groups[2].FindElement(By.Name("childrenList")));
            IWebElement removeButton = groups[2].FindElement(By.ClassName("glyphicon-arrow-right"));
            IWebElement saveButton = groups[2].FindElement(By.ClassName("save"));
            int optionsCount = typeSelector.Options.Count;
            // Ignore first empty option. Add all fields and enter data 
            for (int i = 0; i < optionsCount; i++)
            {
                IWebElement option = typeSelector.Options[0];
                typeSelector.SelectByText(option.Text);
                clickButton(removeButton);
            }

            clickButton(saveButton);
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

        private void filledFormFields()
        {
            IReadOnlyList<IWebElement> fields = this.Driver.FindElements(By.ClassName("form-field"));
            int count = 0;
            foreach(var f in fields)
            {
                IWebElement name = f.FindElement(By.Name("label")); //text field
               
                string txt = name.Text;

                if(count == 0)
                {
                    txt = CollectionTestValues.Name;
                    count++;
                }
                IReadOnlyList<IWebElement> inputs = f.FindElements(By.ClassName("languageInputField"));
                //each field has 3 input fields each for each language -- eng, fr and sp
               for(int i =0; i < inputs.Count; i++)
                {
                    //grab text field or text area
                    IWebElement textEl;
                    bool bfound = isElementFound(inputs[i],"input");
                    if (bfound)
                    {
                        textEl = inputs[i].FindElement(By.TagName("input"));
                    }
                    else
                    {
                        textEl = inputs[i].FindElement(By.TagName("textarea"));
                    }
   
                    if(i==0)  //eng
                    {
                        textEl.SendKeys(txt);
                    }
                    else if(i==1)//fr
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

        private bool isElementFound(IWebElement el, string tagName)
        {
            bool found = false;
            try
            {
                IWebElement elFound = el.FindElement(By.TagName(tagName));
                found = true;
            }
            catch(NoSuchElementException ex)
            {
                found = false;
            }

            return found;
        }
        private void editFormFields()
        {
            IReadOnlyList<IWebElement> fields = this.Driver.FindElements(By.ClassName("form-field"));
            int count = 0;
            foreach (var f in fields)
            {
                IWebElement name = f.FindElement(By.Name("label"));
                string txt = name.Text;

                if (count == 0)
                {
                    txt = CollectionTestValues.EditedName;
                    count++;
                }
                IReadOnlyList<IWebElement> inputs = f.FindElements(By.ClassName("languageInputField"));
                //each field has 3 input fields each for each language -- eng, fr and sp
                for (int i = 0; i < inputs.Count; i++)
                {
                    IWebElement textEl;
                    bool bfound = isElementFound(inputs[i], "input");
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
        private void clickSave()
        {
            //this.Driver.FindElement(By.ClassName("save")).Click(); ==> this option sometimes throw error, element not found!!!
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
           wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("save")));

            IWebElement btnSave = this.Driver.FindElement(By.ClassName("save"));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
          
            ex.ExecuteScript("window.scrollTo(0, 0) ", btnSave);
            Thread.Sleep(500);
            btnSave.Click();

        }

        private void clickButton(IWebElement btn)
        {
            
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

            ex.ExecuteScript("arguments[0].focus(); ", btn);
            Thread.Sleep(500);
            btn.Click();
        }

        private void clickButtonDelete(IWebElement btn)
        {
            
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

            ex.ExecuteScript("arguments[0].focus(); ", btn);
          
            btn.Click();

            //have to click on'ok' on alert pop up 
            IAlert alert = this.Driver.SwitchTo().Alert();
          
            alert.Accept();
        }

        private Collection GetNewlyAddedCollection()
        {
            CatfishDbContext db = new CatfishDbContext();
            if (db.Database.Connection.State == ConnectionState.Closed)
            {
                db.Database.Connection.Open();
            }
            var col = db.Collections.OrderByDescending(i => i.Id).First();

            return col;
        }

        private Collection GetCollectionById(int id)
        {
            CatfishDbContext db = new CatfishDbContext();
            if (db.Database.Connection.State == ConnectionState.Closed)
            {
                db.Database.Connection.Open();
            }
            Collection col = db.Collections.Where(i => i.Id==id).FirstOrDefault();

            return col;
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
            this.Driver.Navigate().GoToUrl(indexUrl);
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
