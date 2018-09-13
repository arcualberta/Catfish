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
        public static string Name = "EntityType Name - Selenium";
        public static string Description = "EntityType Description";
        public static string Label = "Label text";
    }
    [TestFixture(typeof(ChromeDriver))]
    public class EntityTypeViewTests<TWebDriver> : BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {
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
            string name = EntityTypeTestValues.Name + " - " + EntityTypeTestValues.Rnd.Next(1, 100);
            AddEntityType(name);

            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");
          
            Assert.AreEqual(name, FindTestValue(name));
         
        }

        [Test]
        public void CanEditBasicInfoEntityType()
        {
            string name = EntityTypeTestValues.Name + " - " + EntityTypeTestValues.Rnd.Next(1, 100);

            AddEntityType(name);

            //make sure the entity type was created successfully
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");
           
            Assert.AreEqual(name, FindTestValue(name));
           
            //try to edit 
            //Itially have Collections,Items ==> uncheck "items" add Forms
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            clickOnEditBtn();
            //this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes/edit/" + entityType.Id);
            IWebElement chkItems = this.Driver.FindElement(By.Id("chk_Items"));
            jsExecutor.ExecuteScript("arguments[0].focus()", chkItems);
            chkItems.Click();
            //add forms
            IWebElement chkForms = this.Driver.FindElement(By.Id("chk_Forms"));
            jsExecutor.ExecuteScript("arguments[0].focus()", chkForms);
            chkForms.Click();
            
            clickSave();
            //To Do: have to revalidate the assert
            bool found = ValidateCheckBoxes("Collections,Forms");
            Assert.AreEqual(true, found);
           
        }

        [Test]
        public void CanModifyMetadataSets()
        {
            //can add, delete, rearrange the metadata sets
            //1 create entityType
            string name = EntityTypeTestValues.Name + " - " + EntityTypeTestValues.Rnd.Next(1, 100);

            AddEntityType(name);
            //validate the entityType created 
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");

            Assert.AreEqual(name, FindTestValue(name));

            //edit the entity Type -- add another metadataset
          
            clickOnEditBtn();
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
            //entityType = GetNewlyAddedEntityType();
            int numMetadataSet = CountMetadataSet();
            Assert.AreEqual(2, numMetadataSet);

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
           // this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");
           // entityType = GetNewlyAddedEntityType();
            Assert.AreEqual(1, CountMetadataSet());

        }

        [Test]
        public void CanAddAttributeMapping()
        {
            //can add, delete, rearrange the metadata sets
            //1 create entityType
            string name = EntityTypeTestValues.Name + " - " + EntityTypeTestValues.Rnd.Next(1, 100);

            AddEntityType(name);
            //validate the entityType created 
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");

            Assert.AreEqual(name, FindTestValue(name));

            //edit the entity Type -- add another metadataset

            clickOnEditBtn();

            //Add second MetadataSet
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

            //Add AttributeMapping

            AddAttributeMapping();
            clickSave();

            Assert.AreEqual(3, countAttributeMapping());

        }

        private void clickOnEditBtn()
        {
            var rows = this.Driver.FindElements(By.TagName("tr"));
            //get the last row -- the newly added item in the list

            rows[rows.Count - 1].FindElement(By.ClassName("glyphicon-edit")).Click();
        }
        private void AddEntityType(string name)
        {
            this.Driver.Navigate().GoToUrl(ManagerUrl + "/entitytypes");

            this.Driver.FindElement(By.LinkText("Add new")).Click();
            FillBasicEntityType(name);
            this.AddMetadataSet(1);
            this.FieldsMapping2();

            clickSave();
        }
        private void FillBasicEntityType(string name)
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;

            this.Driver.FindElement(By.Id("Name")).SendKeys(name);

            
            IWebElement description = this.Driver.FindElement(By.Id("Description"));
            description.SendKeys(EntityTypeTestValues.Description);

            // Applied to: Collections, Items, Files, Forms
            IWebElement collectionChk = this.Driver.FindElement(By.Id("chk_Collections"));
            jsExecutor.ExecuteScript("$(arguments[0]).focus()", collectionChk);
            collectionChk.Click();

            IWebElement itemChk = this.Driver.FindElement(By.Id("chk_Items"));
            jsExecutor.ExecuteScript("$(arguments[0]).focus()", itemChk);
            itemChk.Click();
        }

        private int CountMetadataSet()
        {
            IWebElement ele = this.Driver.FindElement(By.Id("fields-container"));

            var metas = ele.FindElements(By.TagName("span"));

            return metas.Count;

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

       private int  countAttributeMapping()
        {
            var Elements = (this.Driver.FindElement(By.Id("fieldmappings-container"))).FindElements(By.ClassName("fieldElement")); //i.e: Name mapping, Description mapping, etc

            return Elements.Count;

        }
        private void AddAttributeMapping()
        {
            IWebElement ButtonAdd = (this.Driver.FindElement(By.Id("fieldmappings-container"))).FindElement(By.ClassName("glyphicon-plus"));
            clickButton(ButtonAdd);

            var Elements = (this.Driver.FindElement(By.Id("fieldmappings-container"))).FindElements(By.ClassName("fieldElement")); //i.e: Name mapping, Description mapping, etc
            int numElement = 1;

            //fill out data for the added attribute mapping
            foreach (IWebElement ele in Elements)
            {
                if(numElement == Elements.Count)
                {
                    IWebElement title =  ele.FindElement(By.Name("Name"));
                    title.Clear();
                    title.SendKeys("My Custom Title");

                    //MetadataSet mapping
                    SelectElement select = new SelectElement(ele.FindElement(By.ClassName("mapMetadata")));
                    // select the last one in the list
                    int optionsCount = select.Options.Count;
                    for (int i = 1; i < optionsCount; i++)
                    {  
                        if (i == (optionsCount-1))
                        {
                            IWebElement option = select.Options[i];
                            select.SelectByText(option.Text);
                            break;
                        }
                    }

                    //Field Mapping
                    select = new SelectElement(ele.FindElement(By.ClassName("mapField")));

                    optionsCount = select.Options.Count;
                    for (int i = 1; i < optionsCount; i++)
                    {

                        if (i == (optionsCount -1))
                        {//only pick the first metadaset from the list
                            IWebElement option = select.Options[i];
                            select.SelectByText(option.Text);
                        }
                    }

                    //Label
                    ele.FindElement(By.ClassName("mapLabel")).SendKeys("My Custom Label");

                }
                numElement++;
            }

        }
        private void FieldsMapping2()
        {
            IWebElement mappingContainer = this.Driver.FindElement(By.Id("fieldmappings-container"));
            //ElementFocus(mappingContainer);
            var Elements = mappingContainer.FindElements(By.ClassName("fieldElement")); //i.e: Name mapping, Description mapping, etc
            
            int index = 0;
            foreach (IWebElement elm in Elements)
            {
                //MetadataSet mapping
                IWebElement selectMDS = elm.FindElement(By.ClassName("mapMetadata"));
                ElementFocus(selectMDS);
               // SelectElement select = new SelectElement(elm.FindElement(By.ClassName("mapMetadata")));
                SelectElement select = new SelectElement(selectMDS);
                // select.SelectByText("Dubline Core");
                int optionsCount = select.Options.Count;
                for (int i = 1; i < optionsCount; i++)
                {
                    IWebElement option = select.Options[i];
                    select.SelectByText(option.Text);
                    if (i < optionsCount) //only pick the first metadaset from the list
                        break;
                }

                //Field Mapping
                select = new SelectElement(elm.FindElement(By.ClassName("mapField")));
                
                optionsCount = select.Options.Count;
                for (int i = 1; i < optionsCount; i++)
                {
                    IWebElement option = select.Options[i + index];
                    select.SelectByText(option.Text);
                    if (i < optionsCount) //only pick the first metadaset from the list
                        break;
                }

                //Label
                elm.FindElement(By.ClassName("mapLabel")).SendKeys(EntityTypeTestValues.Label);

                index++;
            }
        }
      
        private bool ValidateCheckBoxes(string labels)
        {
            string[] checkboxLabels = labels.Split(',');
            int found = 0;
           // var checkboxes = this.Driver.FindElements(By.TagName("input"));
           
            for (int i= 0; i < checkboxLabels.Count(); i++)
            {
                if (checkboxLabels[i].Equals("Collections"))
                {
                    IWebElement chkCollections = this.Driver.FindElement(By.Id("chk_Collections"));
                    if (chkCollections.Selected)
                        found++;
                }
                else if (checkboxLabels[i].Equals("Items"))
                {
                    IWebElement chkItems = this.Driver.FindElement(By.Id("chk_Items"));
                    if (chkItems.Selected)
                        found++;
                }
                else if (checkboxLabels[i].Equals("Forms"))
                {
                    IWebElement chkForms = this.Driver.FindElement(By.Id("chk_Forms"));
                    if (chkForms.Selected)
                        found++;

                }
                else if (checkboxLabels[i].Equals("Files"))
                {
                    IWebElement chkFiles = this.Driver.FindElement(By.Id("chk_Files"));
                    if (chkFiles.Selected)
                        found++;
                }
            }

            if (found == checkboxLabels.Count())
                return true;
            else
                return false;

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

        private CFEntityType GetNewlyAddedEntityType()
        {
            CatfishDbContext db = new CatfishDbContext();
            if (db.Database.Connection.State == ConnectionState.Closed)
            {
                db.Database.Connection.Open();
            }
            var entityTypes = db.EntityTypes.OrderByDescending(i => i.Id).First();

            return entityTypes;
        }
       


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
