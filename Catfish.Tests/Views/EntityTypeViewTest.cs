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

    static class EntityTypeTestValues
    {
        public static Random Rnd = new Random();
        public static string Name = "EntityType Name - Selenium" + Rnd.Next(1,100);
        public static string Description = "EntityType Description"; 
    }
    [TestFixture(typeof(ChromeDriver))]
    public class EntityTypeViewTests<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver Driver;
        private string ManagerUrl;

        [SetUp]
        public void SetUp()
        {
            this.Driver = new TWebDriver();
            this.ManagerUrl = ConfigurationManager.AppSettings["ServerUrl"] + "manager";
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
        public void CanCreateEntityType()
        {
           
            AddEntityType();


            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");
            var entityType = GetNewlyAddedEntityType();
           
            Assert.AreEqual(entityType.Id.ToString(), FindTestValue(entityType.Id.ToString()));
            Assert.AreEqual(EntityTypeTestValues.Name, FindTestValue(EntityTypeTestValues.Name));
            Assert.AreEqual("Collections,Items", entityType.TargetTypes);

        }

        [Test]
        public void CanEditBasicInfoEntityType()
        {
           
            AddEntityType();

            //make sure the entity type was created successfully
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");
            var entityType = GetNewlyAddedEntityType();

            Assert.AreEqual(entityType.Id.ToString(), FindTestValue(entityType.Id.ToString()));
            Assert.AreEqual(EntityTypeTestValues.Name, FindTestValue(EntityTypeTestValues.Name));
            Assert.AreEqual("Collections,Items", entityType.TargetTypes);

            //try to edit 
            //uncheck "items" add Forms
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes/edit/" + entityType.Id);
            IWebElement chkItems = this.Driver.FindElement(By.Id("chk_Items"));
            jsExecutor.ExecuteScript("arguments[0].focus()", chkItems);
            chkItems.Click();
            //add forms
            IWebElement chkForms = this.Driver.FindElement(By.Id("chk_Forms"));
            jsExecutor.ExecuteScript("arguments[0].focus()", chkForms);
            chkForms.Click();
            
            clickSave();
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");
            Thread.Sleep(500);
            entityType = GetNewlyAddedEntityType();
            Assert.AreEqual("Collections,Forms", entityType.TargetTypes);
        }
       [Test]
        public void CanModifyMetadataSets()
        {
            //can add, delete, rearrange the metadata sets
            //1 create entityType
            AddEntityType();
            //validate the entityType created 
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");
            var entityType = GetNewlyAddedEntityType();

            Assert.AreEqual(entityType.Id.ToString(), FindTestValue(entityType.Id.ToString()));
            Assert.AreEqual(EntityTypeTestValues.Name, FindTestValue(EntityTypeTestValues.Name));
            Assert.AreEqual("Collections,Items", entityType.TargetTypes);


            //edit the entity Type -- add another metadataset
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes/edit/" + entityType.Id);
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("dd_MetadataSets")));
            //select.SelectByText("Dubline Core");
            int optionsCount = select.Options.Count;
            // Ignore first empty option. Add all fields and enter data 
            
            for (int i = 1; i < optionsCount; i++)
            {
                //get the second metadataset in the list 
                if (i == 2)
                {
                    IWebElement option = select.Options[i];
                    select.SelectByText(option.Text);
                    this.Driver.FindElement(By.Id("btnAddMetadataSet")).Click();
                    break;
                }
            }

            clickSave();
            entityType = GetNewlyAddedEntityType();
            Assert.AreEqual(2, entityType.MetadataSets.Count);


            //delete one of the metadataset
            //delete the last one
            IReadOnlyList<IWebElement> removeBtns = this.Driver.FindElements(By.ClassName("glyphicon-remove"));
            for (int i = 0; i < removeBtns.Count; i++)
            {
                if (i == (removeBtns.Count - 1))
                {
                    clickButton(removeBtns[i]);
                    break;
                }
            }
       
            clickSave();
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");
            entityType = GetNewlyAddedEntityType();
            Assert.AreEqual(1, entityType.MetadataSets.Count);

        }

        private void AddEntityType()
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");

            this.Driver.FindElement(By.LinkText("Add new")).Click();

            this.FillBasicEntityType();
            this.AddMetadataSet(1);
            this.FieldsMapping();

            clickSave();
        }
        private void FillBasicEntityType()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;

            //WebDriverWait webDriver = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
           // webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Name")));
            this.Driver.FindElement(By.Id("Name")).SendKeys(EntityTypeTestValues.Name);

            //webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("Description")));
            IWebElement description = this.Driver.FindElement(By.Id("Description"));//.SendKeys(MetadataTestValues.MetadatasetDescription);
            description.SendKeys(EntityTypeTestValues.Description);

            // Applied to: Collections, Items, Files, Forms
            IWebElement collectionChk = this.Driver.FindElement(By.Id("chk_Collections"));
            jsExecutor.ExecuteScript("$(arguments[0]).focus()", collectionChk);
            collectionChk.Click();

            IWebElement itemChk = this.Driver.FindElement(By.Id("chk_Items"));
            jsExecutor.ExecuteScript("$(arguments[0]).focus()", itemChk);
            itemChk.Click();
        }


        private void AddMetadataSet(int num)
        {
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("dd_MetadataSets")));
            //select.SelectByText("Dubline Core");
            int optionsCount = select.Options.Count;
            // Ignore first empty option. Add all fields and enter data 
            int count = 1; //count how many metadataset need to be added
            for (int i = 1; i < optionsCount; i++)
            {
                IWebElement option = select.Options[i];
                select.SelectByText(option.Text);
                this.Driver.FindElement(By.Id("btnAddMetadataSet")).Click();
                if (count == num)
                    break;

                count++;
            }
        }
       
        private void FieldsMapping()
        {
            //name
            SelectElement select = new SelectElement(this.Driver.FindElement(By.Id("dd_NameMappingMetadataSet")));
            // select.SelectByText("Dubline Core");
            int optionsCount = select.Options.Count;
            for(int i=1; i< optionsCount; i++)
            {
                IWebElement option = select.Options[i];
                select.SelectByText(option.Text);
                if (i < optionsCount) //only pick the first metadaset from the list
                    break;
            }
            this.Driver.FindElement(By.Id("btnUpdateNameMetadataMapping")).Click();
          
            select = new SelectElement(this.Driver.FindElement(By.Id("dd_NameMetadataMappingFields")));
            // select.SelectByText("Title");
            optionsCount = select.Options.Count;
            for (int i = 1; i < optionsCount; i++)
            {
                if (i == optionsCount) //only pick the last available fields from the list to mapped to Name
                {
                    IWebElement option = select.Options[i];
                    select.SelectByText(option.Text);
                    this.Driver.FindElement(By.Id("btnUpdateNameMetadataFieldMapping")).Click();
                    break;
                }
                    
            }
          

            //description
           
             select = new SelectElement(this.Driver.FindElement(By.Id("dd_descMetadataSetMapping")));
            // select.SelectByText("Dubline Core");
            optionsCount = select.Options.Count;
            for(int i = 1; i < optionsCount; i++)
            {
                //pick the 1st metadataset for mapping to description
                IWebElement option = select.Options[i];
                select.SelectByText(option.Text);
                this.Driver.FindElement(By.Id("btnDescMetadataSetMapping")).Click();
                break;
            }

            //mapping description field
            select = new SelectElement(this.Driver.FindElement(By.Id("dd_DescMetadataFieldMapping")));
            //select.SelectByText("Description");
            optionsCount = select.Options.Count;
            for (int i = 1; i < optionsCount; i++)
            {
                //pick the 1st metadataset for mapping to description
                IWebElement option = select.Options[i];
                select.SelectByText(option.Text);
                this.Driver.FindElement(By.Id("btn_DescMetadataFieldMapping")).Click();
                break;
            }  
        }

        private void clickSave()
        {
            //this.Driver.FindElement(By.ClassName("save")).Click(); ==> this option sometimes throw error, element not found!!!
           // WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
          // wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("save")));

            IWebElement btnSave = this.Driver.FindElement(By.Id("btnSave"));
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;
          
            ex.ExecuteScript("arguments[0].focus(); ", btnSave);
            Thread.Sleep(500);
            btnSave.Click();

        }

        private void clickButton(IWebElement btn)
        {
            //this.Driver.FindElement(By.ClassName("save")).Click(); ==> this option sometimes throw error, element not found!!!
            // WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            // wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("save")));

           
            IJavaScriptExecutor ex = (IJavaScriptExecutor)Driver;

            ex.ExecuteScript("arguments[0].focus(); ", btn);
            Thread.Sleep(500);
            btn.Click();

        }

        private EntityType GetNewlyAddedEntityType()
        {
            CatfishDbContext db = new CatfishDbContext();
            if (db.Database.Connection.State == ConnectionState.Closed)
            {
                db.Database.Connection.Open();
            }
            var entityTypes = db.EntityTypes.OrderByDescending(i => i.Id).First();

            return entityTypes;
        }
        //private IReadOnlyList<IWebElement> GetFieldEntries(string fieldEntryTitle)
        //{
        //    IReadOnlyList<IWebElement> entries=null;

        //    WebDriverWait webDriver = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
        //    webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("field-entry")));

        //    var allElementTypes = Driver.FindElements(By.ClassName("field-entry"));
        //    foreach(IWebElement e in allElementTypes)
        //    {
        //        if(e.FindElement(By.ClassName("title")).Text.Equals(fieldEntryTitle))
        //        {
        //            entries =  e.FindElements(By.ClassName("input-field"));
        //            break;
        //        }
        //    }
        //    return entries;
        //}

        //private IReadOnlyList<IWebElement> GetFieldElements()
        //{

        //    WebDriverWait webDriver = new WebDriverWait(Driver, TimeSpan.FromMinutes(1));
        //    webDriver.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.ClassName("field-entry")));

        //    var allElementTypes = Driver.FindElements(By.ClassName("field-entry"));

        //    return allElementTypes;
        //}



        private string FindTestValue(string expectedValue)
        {
            string val = "";
            var cols = this.Driver.FindElements(By.TagName("td"));

            foreach (var col in cols)
            {
                if (col.Text == expectedValue)
                {
                    val = col.Text;
                    break;
                }
            }
            return val;
        }

        //private int GetNewlyAddedMetadataId()
        //{
        //    return GetNewlyAddedMetadaSet().Id;
        //}

        //private string GetNewlyAddedMetadataDescription()
        //{
        //    return GetNewlyAddedMetadaSet().Description;
        //}


        //private MetadataSet GetNewlyAddedMetadaSet()
        //{
        //    CatfishDbContext db = new CatfishDbContext();
        //    if (db.Database.Connection.State == ConnectionState.Closed)
        //    {
        //        db.Database.Connection.Open();
        //    }
        //    var metadata = db.MetadataSets.OrderByDescending(i => i.Id).First();

        //    return metadata;
        //}
        //private MetadataSet GetMetadaSetById(int id)
        //{
        //    CatfishDbContext db = new CatfishDbContext();
        //    if (db.Database.Connection.State == ConnectionState.Closed)
        //    {
        //        db.Database.Connection.Open();
        //    }
        //    var metadata = db.MetadataSets.Where(m => m.Id == id).FirstOrDefault();

        //    return metadata;
        //}
    }
}
