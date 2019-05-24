using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Forms;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catfish.Tests.Extensions;
using Catfish.Core.Services;
using SolrNet;
using Catfish.Core.Models.Access;
using System.Text.RegularExpressions;
using System.Threading;
using Catfish.Services;

namespace Catfish.Tests.IntegrationTests.Helpers

{
    abstract class BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        protected IWebDriver Driver;
        protected string ManagerUrl;
        protected string FrontEndUrl;
        protected const string ContentLinkText = "CONTENT";
        protected const string SettingsLinkText = "SETTINGS";
        protected const string SystemLinkText = "SYSTEM";
        protected const string LogoutLinkText = "LOGOUT";
        protected const string MetadataSetsLinkText = "Metadata Sets";
        protected const string EntityTypesLinkText = "Entity Types";
        protected const string ItemsLinkText = "Items";
        protected const string CollectionsLinkText = "Collections";
        protected const string FormsLinkText = "Forms";
        protected const string AccessDefinitionsLinkText = "Access Definitions";
        protected const string PagesLinkText = "Pages";
        protected const string PageTypesLinkText = "Page types";
        protected const string StandardPageLinkText = "Standard page";
        protected const string ToolBarAddButtonId = "toolbar_add_button";
        protected const string ToolBarSaveButtonId = "toolbar_save_button";
        protected const string NameId = "Name";
        protected const string DescriptionId = "Description";
        protected const string EntityTypeName = "Entity type name";
        protected const string EntityTypeDescription = "Entity type description";
        protected const string MetadataSetName = "Metadata set name";
        protected const string MetadataSetDescription = "Metadata set description";
        protected const string FieldName = "Field name";
        protected const string ItemValue = "Item value";
        protected const string RegionName = "Region Name";
        protected const string AdvancedSearchRegionName = "Advances Search Region Name";
        protected const string FormContainerRegionName = "Form Container Region Name";
        protected const string EntityListRegionName = "Entity List Region Name";
        protected const string GraphPanelName = "Graph Panel Region Name";
        protected const string AccessDefinitionName = "Access definition";
        protected const string PublicGroupName = "Public";
        protected const string UserNameFieldId = "usrName";
        protected const string AddUserAccessButtonId = "btnAddUserAccess";
        protected const string RegionNameFieldId = "newregionName";
        protected const string RegionInternalIdId = "newregionInternalId";
        protected const string RegionTypeSelectorId = "newregionType";
        protected const string AddRegionButtonId = "btnAddRegion";
        protected const string SaveLinkText = "Save";
        protected const string StartLinkText = "Start";
        protected const string UpdateButtonClass = "publish";
        protected const string ObjectListClass = "object-list";
        protected const string ListEntitiesId = "ListEntitiesPanelTableBody";
        protected const string MultipleField = "Multiple Field";
        // There should be a better way of defining access permissions
        protected const string MenuItemWrapperClass = "ui-menu-item-wrapper";
        protected const AccessMode AccessDefinitionMode = AccessMode.Read;

        // FormFields will be used to share FormField values between CFAggregation 
        // initialization

        protected FormField[][] FormFields;

        [SetUp]
        public void SetUp()
        {
            InitializeSolr();
            Driver = new TWebDriver();
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            FrontEndUrl = ConfigurationManager.AppSettings["ServerUrl"];
            ManagerUrl = FrontEndUrl + "manager";

            ClearDatabase();
            ResetServerCache();

            SetupPiranha();

            RunMigrations();
            LoginAsAdmin();


            OnSetup();
        }

        [TearDown]
        public void TearDown()
        {
            OnTearDown();
            Driver.Close();
            ClearDatabase();
        }

        protected virtual void OnSetup() { }

        protected virtual void OnTearDown() { }

        private void InitializeSolr()
        {
            string solrString = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];
            SolrService.ForceInit(solrString);
        }

        private void ClearDatabase()
        {
            CatfishDbContext context = new CatfishDbContext();
            string query = @"DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR
                SET @Cursor = CURSOR FAST_FORWARD FOR
                SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + ']'
                FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
                LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME

                OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql

                WHILE (@@FETCH_STATUS = 0)
                BEGIN
                Exec sp_executesql @Sql
                FETCH NEXT FROM @Cursor INTO @Sql
                END

                CLOSE @Cursor DEALLOCATE @Cursor";
            int result = context.Database.ExecuteSqlCommand(query);

            context.Database.ExecuteSqlCommand(@"EXEC sp_MSforeachtable 'DROP TABLE ?'");

            result = context.Database.ExecuteSqlCommand(query);

            ISolrQuery allEntries = new SolrQuery("*:*");
            SolrService.SolrOperations.Delete(allEntries);
            SolrService.SolrOperations.Commit();
        }

        private void ResetServerCache()
        {
            string webConfig = ConfigurationManager.AppSettings["WebConfigLocation"];
            File.SetLastWriteTime(webConfig, DateTime.Now);
        }

        private void RunMigrations()
        {
            Catfish.Core.Migrations.Configuration config = new Catfish.Core.Migrations.Configuration();
            var migrator = new DbMigrator(config);
            migrator.Update();
        }

        private void SetupPiranha()
        {

            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.Name("UserLogin")).SendKeys(ConfigurationManager.AppSettings["AdminLogin"]);
            Driver.FindElement(By.Name("Password")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            Driver.FindElement(By.Name("PasswordConfirm")).SendKeys(ConfigurationManager.AppSettings["AdminPassword"]);
            Driver.FindElement(By.Name("UserEmail")).SendKeys(ConfigurationManager.AppSettings["AdminEmail"]);

            Driver.FindElement(By.Id("full")).Click();
        }

        protected void LoginAsAdmin()
        {
            Login(ConfigurationManager.AppSettings["AdminLogin"],
                ConfigurationManager.AppSettings["AdminPassword"]);
        }

        protected void Login(string user, string password)
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.Id("login")).SendKeys(user);
            Driver.FindElement(By.Name("password")).SendKeys(password);
            Driver.FindElement(By.TagName("button")).Click();
        }

        // First

        protected IWebElement GetFirstObjectRow()
        {
            return Driver.FindElement(By.XPath("(//tbody[contains(@class, 'object-list')]/tr)[1]"), 10);
        }

        protected IWebElement GetFirstActionPanel()
        {
            return GetFirstObjectRow().FindElement(By.XPath(".//td[contains(@class, 'action-panel')]"));
        }

        public IWebElement GetFirstButtonByClass(string cssClass)
        {
            return GetFirstObjectRow().FindElement(By.XPath($".//button[contains(@class, '{cssClass}')]"));
        }

        protected IWebElement GetFirstEditButton()
        {
            return GetFirstButtonByClass("object-edit");
        }

        protected IWebElement GetFirstAssociationsButton()
        {
            return GetFirstButtonByClass("object-associations");
        }

        protected IWebElement GetFirstDeleteButton()
        {
            return GetFirstButtonByClass("object-delete");
        }

        protected IWebElement GetFirstAccessGroupButton()
        {
            return GetFirstButtonByClass("object-accessgroup");
        }

        // Last

        protected IWebElement GetLastObjectRow()
        {
            return Driver.FindElement(By.XPath("(//tbody[contains(@class, 'object-list')]/tr)[last()]"), 10);
        }

        protected IWebElement GetLastActionPanel()
        {
            return GetLastObjectRow().FindElement(By.XPath(".//td[contains(@class, 'action-panel')]"));
        }

        public IWebElement GetLastButtonByClass(string cssClass)
        { 
           return GetLastObjectRow().FindElement(By.XPath($".//button[contains(@class, '{cssClass}')]"));
        }

        protected IWebElement GetLastEditButton()
        {
            return GetLastButtonByClass("object-edit");
        }

        protected IWebElement GetLastAssociationsButton()
        {
            return GetLastButtonByClass("object-associations");
        }

        protected IWebElement GetLastDeleteButton()
        {
            return GetLastButtonByClass("object-delete");
        }

        protected IWebElement GetLastAccessGroupButton()
        {
            return GetLastButtonByClass("object-accessgroup");
        }

        public void CreateMetadataSet(string name, string description, FormField[] fields, bool isMultiple = false, bool required=false)
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(SettingsLinkText)).Click();
            Driver.FindElement(By.LinkText(MetadataSetsLinkText)).Click();
            Driver.FindElement(By.Id(ToolBarAddButtonId)).Click();
            Driver.FindElement(By.Id(NameId)).SendKeys(name);
            Driver.FindElement(By.Id(DescriptionId)).SendKeys(description);

            // Add metadata set fields

            // id field-type-selector
            IWebElement fieldTypeSelectorElement = Driver.FindElement(By.Id("field-type-selector"));
            SelectElement fieldTypeSelector = new SelectElement(fieldTypeSelectorElement);

            int i = 1;
            foreach (FormField field in fields)
            {
                //field.GetType();
                CFTypeLabelAttribute typeLabelAttribute = (CFTypeLabelAttribute)field.GetType()
                    .GetCustomAttributes(typeof(CFTypeLabelAttribute), true)
                    .First();

                // select option by label
                fieldTypeSelector.SelectByText(typeLabelAttribute.Name);
                // click on id add-field

                Driver.FindElement(By.Id("add-field")).Click();

                // fill values on last field-entry
                IWebElement lastFieldEntry = Driver.FindElement(By.XPath("(//div[contains(@class, 'field-entry')])[last()]"), 10);

                // fill class field-is-required with fields[0].IsRequired

                // XXX generalize to use all language codes Name_sp, Name_fr
                lastFieldEntry.FindElement(By.Name("Name_en")).SendKeys(field.Name);

                //make the 1st field mandatory
                if(i == 1)
                {
                    field.IsRequired = required;
                }

                // XXX need to add options ?

                if (field.IsRequired)
                {
                    lastFieldEntry.FindElement(By.ClassName("field-is-required")).Click();
                }

                if (isMultiple)
                {
                    //only make the first field multiple
                    if (i == 1)
                    {
                        lastFieldEntry.FindElement(By.ClassName("field-is-multiple")).Click();
                        i++;
                    }
                }

                // Add options
                if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
                {
                    IEnumerable<Option> options = ((OptionsField)field).Options;

                    IEnumerable<IWebElement> elements = lastFieldEntry.FindElements(By.CssSelector(".languageInputField > textarea"));

                    for (int f = 0; f < elements.Count(); ++f)
                    {
                        elements.ElementAt(f).SendKeys(string.Join("\n", options.Select(o => o.Value.Count > f ? o.Value.ElementAt(f).Value : " ")));
                    }
                }
            }

            Driver.FindElement(By.Id(ToolBarSaveButtonId), 10).Click();
        }

        private void AddEntityTypeFieldMapping(string name, string metadataName, string fieldName)
        {
            string fieldElementsXpath = "//div[@id = 'fieldmappings-container']//div[contains(@class, 'fieldElement')]";
            List<IWebElement> fieldElements = Driver.FindElements(By.XPath(fieldElementsXpath), 10).ToList();
            int count = fieldElements.Count + 1;

            string fieldElementsAddXpath = "//div[@id = 'fieldmappings-container']//button[contains(@class, 'glyphicon glyphicon-plus')]";
            Driver.FindElement(By.XPath(fieldElementsAddXpath)).Click();


            IWebElement fieldElement = Driver.FindElement(By.XPath(fieldElementsXpath + "[" + count + "]"), 10);

            string mapMetadataXpath = $".//select[contains(@class, 'mapMetadata')]";
            string mapFieldXpath = $".//select[contains(@class, 'mapField')]";

            // Set the data
            IWebElement mapName = fieldElement.FindElement(By.Name("Name"));
            mapName.Clear();
            mapName.SendKeys(name);

            IWebElement mapMetadataElement = fieldElement.FindElement(By.XPath(mapMetadataXpath));
            SelectElement mapMetadataSelector = new SelectElement(mapMetadataElement);
            mapMetadataSelector.SelectByText(metadataName);

            IWebElement mapFieldElement = fieldElement.FindElement(By.XPath(mapFieldXpath));
            SelectElement mapFieldSelector = new SelectElement(mapFieldElement);
            mapFieldSelector.SelectByText(fieldName);
        }

        private void FillEntityTypeNameMapping()
        {
            string fieldElementsXpath = "//div[@id = 'fieldmappings-container']//div[contains(@class, 'fieldElement')]";
            List<IWebElement> fieldElements = Driver.FindElements(By.XPath(fieldElementsXpath), 10).ToList();

            // for simplicity sake link name and description to first element

            string mapMetadataXpath = $".//select[contains(@class, 'mapMetadata')]";
            string mapFieldXpath = $".//select[contains(@class, 'mapField')]";

            for (int i = 0; i < 2; ++i)
            {
                IWebElement mapMetadataElement = fieldElements[i]
                    .FindElement(By.XPath(mapMetadataXpath));
                SelectElement mapMetadataSelector = new SelectElement(mapMetadataElement);
                mapMetadataSelector.SelectByIndex(1);

                IWebElement mapFieldElement = fieldElements[i]
                    .FindElement(By.XPath(mapFieldXpath));
                SelectElement mapFieldSelector = new SelectElement(mapFieldElement);
                mapFieldSelector.SelectByIndex(1);
            }
        }
        private void ScrollTop()
        {
            IJavaScriptExecutor jex = (IJavaScriptExecutor)Driver;
            jex.ExecuteScript("scroll(0, -250);");
        }

        public void CreateEntityType(string name, string description,
            string[] metadataSetNames, CFEntityType.eTarget[] targetTypes, Tuple<string, FormField>[] mappings = null)
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(SettingsLinkText)).Click();
            Driver.FindElement(By.LinkText(EntityTypesLinkText)).Click();
            Driver.FindElement(By.Id(ToolBarAddButtonId)).Click();
            Driver.FindElement(By.Id(NameId)).SendKeys(name);
            Driver.FindElement(By.Id(DescriptionId)).SendKeys(description);

            //XXX for now make it applicable to all

            Driver.FindElement(By.Id("chk_Collections")).Click();
            Driver.FindElement(By.Id("chk_Items")).Click();
            Driver.FindElement(By.Id("chk_Files")).Click();
            Driver.FindElement(By.Id("chk_Forms")).Click();

            // use first metadataset and fields for name and description

            IWebElement metadataSetSelectorElement = Driver.FindElement(By.Id("dd_MetadataSets"));
            SelectElement metadatasetSelector = new SelectElement(metadataSetSelectorElement);

            foreach (string metadataSetName in metadataSetNames)
            {
                metadatasetSelector.SelectByText(metadataSetName);
                Driver.FindElement(By.Id("btnAddMetadataSet")).Click();
            }

            FillEntityTypeNameMapping();

            // Need to add field mappings
            if (mappings != null)
            {
                foreach (Tuple<string, FormField> field in mappings)
                {
                    AddEntityTypeFieldMapping(field.Item2.Name + " Mapping", field.Item1, field.Item2.Name);
                }
            }

            Driver.FindElement(By.Id(ToolBarSaveButtonId), 10).Click();
        }

        public void CreateCFAggregation(string aggregationLinkText, string entityTypeName, FormField[] metadatasetValues, bool isMultiple = false)
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(ContentLinkText)).Click();
            Driver.FindElement(By.LinkText(aggregationLinkText)).Click();
            Driver.FindElement(By.Id(ToolBarAddButtonId)).Click();

            IWebElement fieldTypeSelectorElement = Driver.FindElement(By.Id("field-type-selector"));
            SelectElement fieldTypeSelector = new SelectElement(fieldTypeSelectorElement);
            fieldTypeSelector.SelectByText(entityTypeName);
            Driver.FindElement(By.Id("add-field")).Click();

            // XXX For now fill first input with field name
            for (int i = 0; i < metadatasetValues.Length; ++i)
            {
                if (metadatasetValues[i].IsRequired)
                {
                    //this should fail when click save without entering the req input data
                    Driver.FindElement(By.Id(ToolBarSaveButtonId), 5).Click();
                    Assert.IsTrue(Driver.FindElement(By.ClassName("sys-message")).Displayed);

                }


                IWebElement formField = Driver.FindElements(By.ClassName("form-field"), 10).ElementAt(i);
               

                if (typeof(TextField).IsAssignableFrom(metadatasetValues[i].GetType()))
                {
                   
                    formField.FindElement(By.XPath(".//input[contains(@class, 'text-box single-line')][1]")).SendKeys(metadatasetValues[i].Values[0].Value);

                    if (isMultiple)
                    {
                        Assert.IsTrue(formField.FindElement(By.ClassName("ButtonTextFieldAddEntry")).Displayed);
                        formField.FindElement(By.ClassName("ButtonTextFieldAddEntry")).Click();
                        Driver.FindElement(By.Name("MetadataSets[" + i + "].Fields[0].Values[3].Value"), 10).SendKeys(MultipleField);
                    }
                }
                else if (typeof(SingleSelectOptionsField).IsAssignableFrom(metadatasetValues[i].GetType()))
                {
                    SingleSelectOptionsField optionsField = (SingleSelectOptionsField)metadatasetValues[i];
                    Option value = optionsField.Options.Where(o => o.Selected).FirstOrDefault();

                    SelectElement select = new SelectElement(formField.FindElement(By.XPath(".//select")));

                    if (value == null)
                    {
                        select.SelectByIndex(0);
                    }
                    else
                    {
                        select.SelectByText(string.Join(" / ", value.Value.Select(v => v.Value)));
                    }
                }
            }

            Driver.FindElement(By.Id(ToolBarSaveButtonId), 10).Click();
        }

        public void CreateItem(string entityTypeName, FormField[] metadatasetValues, bool isMultiple = false)
        {
            CreateCFAggregation(ItemsLinkText, entityTypeName, metadatasetValues, isMultiple);
        }

        public void CreateCollection(string entityTypeName, FormField[] metadatasetValues)
        {
            CreateCFAggregation(CollectionsLinkText, entityTypeName, metadatasetValues);
        }

        public void CreateForm(int entityTypeId)
        {
            throw new NotImplementedException();
        }

        public string CreateUser(string userName, string password, string email)
        {
            throw new NotImplementedException();
        }

        public void CreateAccessDefinition(string name, AccessMode accessMode)
        {

            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(SystemLinkText)).Click();
            Driver.FindElement(By.LinkText(AccessDefinitionsLinkText)).Click();
            Driver.FindElement(By.Id(ToolBarAddButtonId)).Click();

            Driver.FindElement(By.Id("Name")).SendKeys(name);

            //XXX For now just select read access mode and ignore accessMode parameter
            bool isChecked = Driver.FindElement(By.Id("1")).Selected;

            if (!isChecked)
            {
                Driver.FindElement(By.Id("1")).Click();
            }

            Driver.FindElement(By.Id(ToolBarSaveButtonId)).Click();
        }

        protected void CreateBaseMetadataSet(bool multipleField = false, bool required = false)
        {
            TextField fieldName = new TextField();
            fieldName.Name = FieldName;

            TextValue textValue = new TextValue("en", "English", ItemValue);
            fieldName.SetTextValues(new List<TextValue> { textValue });

            TextArea fieldDescription = new TextArea();
            fieldDescription.Name = "Description";

            FormFields = new FormField[2][];
            FormFields[0] = new FormField[2];

            FormFields[0][0] = fieldName;
            FormFields[0][1] = fieldDescription;

            //List<FormField> formFields = new List<FormField>();
            //formFields.Add(fieldName);
            //formFields.Add(fieldDescription);

            CreateMetadataSet(MetadataSetName, MetadataSetDescription, FormFields[0], multipleField, required);
        }

        //XXX Change to be able to specify entity type and metadata set names
        protected void CreateBaseEntityType(bool multipleField = false, bool required = false)
        {
            // Create metadata set
            // create entity type

            CreateBaseMetadataSet(multipleField, required);

            CreateEntityType(EntityTypeName, EntityTypeDescription, new[] {
                MetadataSetName
                }, new CFEntityType.eTarget[0]);
        }

        protected void CreateBaseItem(string itemString)
        {
            CreateBaseItem(itemString, EntityTypeName);
        }

        protected void CreateBaseItem(string itemString, string entityTypeName)
        {
            TextValue itemValue = new TextValue("en", "English", itemString);

            //FormFields[0][0].SetTextValues(new List<TextValue> { itemValue });
            TextField formField = new TextField();
            formField.SetTextValues(new List<TextValue> { itemValue });
            FormFields[0][0] = formField;
            CreateItem(entityTypeName, FormFields[0]);
        }

        protected void AssertMatchesSolrInformationFromUrl()
        {
            // get id from url
            string url = Driver.Url;
            const string urlPattern = @".+\/(\d+)";
            const string keyValuePattern = @"^value_.+_txts_\w{2}$";
            const string associationsPattern = @"^(?:parents|related)_ss$";

            Regex urlRegex = new Regex(urlPattern);
            Regex keyValyeRegex = new Regex(keyValuePattern);
            Regex associationsRegex = new Regex(associationsPattern);

            Match urlMatch = urlRegex.Match(url);

            if (urlMatch.Success && urlMatch.Groups.Count == 2)
            {
                Int64 id = Convert.ToInt64(urlMatch.Groups[1].Value);
                CatfishDbContext db = new CatfishDbContext();
                CFEntity model = db.Entities.Find(id);
                Dictionary<string, object> result = model.ToSolrDictionary();

                SolrQuery q = new SolrQuery($@"id:{model.MappedGuid}");
                SolrQueryResults<Dictionary<string, object>> solrResults = SolrService.SolrOperations.Query(q);
                if (solrResults.Count == 1)
                {
                    Dictionary<string, object> fromSolr = solrResults[0];

                    foreach (KeyValuePair<string, object> entry in result)
                    {
                        // first we need to make sure the entry value is not empty, 
                        // otherwise is not indexed in solr
                        //if (entry.Value.ToString().Length > 0 &&  entry.Value != fromSolr[entry.Key])
                        //{
                        //    return false;
                        //}


                        //if (entry.Value.ToString().Length > 0)
                        //{
                        //    // check if key has value pattern
                        //    Match keyValueMatch = keyValyeRegex.Match(entry.Key);
                        //    if (keyValueMatch.Success)
                        //    {
                        //        // compare as list of values

                        //        CollectionAssert.AreEqual((List<string>)entry.Value, 
                        //            (System.Collections.ArrayList)fromSolr[entry.Key]);

                        //    } else
                        //    {
                        //        Assert.AreEqual(entry.Value, fromSolr[entry.Key]);
                        //    }

                        //}

                        Match keyValueMatch = keyValyeRegex.Match(entry.Key);
                        Match associationsMatch = associationsRegex.Match(entry.Key);
                        if (keyValueMatch.Success || associationsMatch.Success)
                        {
                            // treat as multi values

                            List<string> test = (List<string>)entry.Value;

                            if (test.Count > 0 && test[0] != "")
                            {
                                CollectionAssert.AreEqual(test,
                                    (System.Collections.ArrayList)fromSolr[entry.Key]);
                            }

                            //XXX Should it fail if the previous test is not passed?
                        }
                        else
                        {
                            // treat as regular values
                            if (!string.IsNullOrEmpty(entry.Value.ToString()))
                            {
                                Assert.AreEqual(entry.Value, fromSolr[entry.Key]);
                            }
                        }


                    }
                    //return true;
                }

            }

            //return false;
        }

        //public void CreateItem(string entityTypeName, string name, bool attachment = false)
        //{
        //    Driver.Navigate().GoToUrl(ManagerUrl);
        //    Driver.FindElement(By.LinkText(ContentLinkText)).Click();
        //    Driver.FindElement(By.LinkText(ItemsLinkText)).Click();
        //    Driver.FindElement(By.Id(ToolBarAddButtonId)).Click();

        //    // id field-type-selector
        //    IWebElement fieldTypeSelectorElement = Driver.FindElement(By.Id("field-type-selector"));
        //    SelectElement fieldTypeSelector = new SelectElement(fieldTypeSelectorElement);

        //    fieldTypeSelector.SelectByText(entityTypeName);

        //    Driver.FindElement(By.Id("add-field")).Click();
        //    FilledItemFormFields(name);

        //    //attach an image
        //    if (attachment)
        //    {           
        //         string sourceFile = ConfigurationManager.AppSettings["SourceTestFile"];
        //         sourceFile = Path.Combine(sourceFile, "image1.jpg");

        //        Driver.FindElement(By.XPath("//input[@type='file']"), 10).SendKeys(sourceFile);
        //    }

        //    Driver.FindElement(By.Id(ToolBarSaveButtonId), 10).Click(); //wait 10 sec before finding the element
        //}

        private void FilledItemFormFields(string strname)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(drv => drv.FindElement(By.ClassName("form-field-name")));

            IReadOnlyList<IWebElement> fields = this.Driver.FindElements(By.ClassName("form-field"));
            int count = 0;
            foreach (var f in fields)
            {
                IWebElement name = f.FindElement(By.Name("label")); //text field
                string txt = name.Text;

                if (count == 0)
                {
                    txt = strname;
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
                Console.WriteLine("Exception : {0}", ex.Message);
                found = false;
            }

            return found;
        }

        protected void CreateAndAddAddvancedSearchToMain(bool includeKeywordSearch, IEnumerable<FormField> fields, int regionIndex = 1, bool isDropdown = false)
        {
            // Create the advanced search region
            Driver.FindElement(By.LinkText(SettingsLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PageTypesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StandardPageLinkText), 10).Click();

            // add region to main page            
            Driver.FindElement(By.Id(RegionNameFieldId)).SendKeys(AdvancedSearchRegionName);
            Driver.FindElement(By.Id(RegionInternalIdId)).SendKeys(AdvancedSearchRegionName);

            IWebElement typeSelectorElement = Driver.FindElement(By.Id(RegionTypeSelectorId));
            SelectElement typeSelector = new SelectElement(typeSelectorElement);
            typeSelector.SelectByValue("Catfish.Models.Regions.AdvanceSearchContainer");

            Driver.FindElement(By.Id(AddRegionButtonId)).Click();

            Driver.FindElement(By.LinkText(SaveLinkText), 10).Click();
            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PagesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StartLinkText), 10).SendKeys(Keys.Return);

            Driver.FindElement(By.XPath($@"//button[contains(.,'{AdvancedSearchRegionName}')]"), 10).Click();

            IWebElement region = Driver.FindElement(By.Id(AdvancedSearchRegionName.Replace(" ", "")));

            if (includeKeywordSearch)
            {
                region.FindElement(By.Id("Regions_" + regionIndex + "__Body_HasGeneralSearch")).Click();
            }

            if (fields != null)
            {

                foreach (FormField field in fields)
                {
                    IWebElement fieldSelectorElement = Driver.FindElement(By.Id("Regions_" + regionIndex + "__Body_selectedField"));
                    SelectElement fieldSelector = new SelectElement(fieldSelectorElement);
                    fieldSelector.SelectByText(field.Name + " Mapping");


                    region.FindElement(By.XPath(".//span[contains(@class, 'glyphicon glyphicon-plus-sign')][" + regionIndex + "]")).Click();
                    //turn on the auto complete for textField
                    if (field.GetType().FullName.Equals("Catfish.Core.Models.Forms.TextField"))
                    {
                        region.FindElement(By.XPath(".//input[contains(@name, '.IsAutoComplete')]")).Click();
                    }

                    //display the year in dropdown
                    if (field.Name.Equals("Year") && isDropdown)
                    {
                        SelectElement selectOptions = new SelectElement((region.FindElements(By.XPath(".//select[contains(@name, '.SelectedDisplayOption')]"))[2]));
                        selectOptions.SelectByText("DropDownList");

                    }
                }
            }

            // Save the page
            Driver.FindElement(By.ClassName(UpdateButtonClass), 10).Click();
        }

        protected void CreateAndAddCalculationToMain(int regionIndex, string panelName, string panelId, ItemQueryService.eFunctionMode function, string title, string medatadataSetName, string fieldName, string prefix = "$", int decimalPlaces = 2, string groupByMetadataSetName = null, string groupByFieldName = null)
        {
            // Create the Graph Section
            Driver.FindElement(By.LinkText(SettingsLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PageTypesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StandardPageLinkText), 10).Click();

            // add region to main page            
            Driver.FindElement(By.Id(RegionNameFieldId)).SendKeys(panelName);
            Driver.FindElement(By.Id(RegionInternalIdId)).SendKeys(panelId);

            IWebElement typeSelectorElement = Driver.FindElement(By.Id(RegionTypeSelectorId));
            SelectElement typeSelector = new SelectElement(typeSelectorElement);
            typeSelector.SelectByValue("Catfish.Models.Regions.CalculatedFieldPanel");

            Driver.FindElement(By.Id(AddRegionButtonId)).Click();

            Driver.FindElement(By.LinkText(SaveLinkText), 10).Click();
            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PagesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StartLinkText), 10).SendKeys(Keys.Return);

            Driver.FindElement(By.XPath($@"//button[contains(.,'{panelName}')]"), 10).Click();

            // Define the region settings
            string regionBaseName = "Regions_" + regionIndex + "__Body_";

            IWebElement element = Driver.FindElement(By.Id(regionBaseName + "Title"), 10);
            element.Clear();
            element.SendKeys(title);

            element = Driver.FindElement(By.Id(regionBaseName + "Prefix"), 10);
            element.Clear();
            element.SendKeys(prefix);

            element = Driver.FindElement(By.Id(regionBaseName + "DecimalPlaces"), 10);
            element.Clear();
            element.SendKeys(decimalPlaces.ToString());

            SelectElement select = new SelectElement(Driver.FindElement(By.Id(regionBaseName + "SelectedFieldMetadataSet"), 10));
            select.SelectByText(medatadataSetName);

            select = new SelectElement(Driver.FindElement(By.Id(regionBaseName + "SelectedField"), 10));
            select.SelectByText(fieldName);

            if (groupByMetadataSetName != null && groupByFieldName != null)
            {
                select = new SelectElement(Driver.FindElement(By.Id(regionBaseName + "SelectedGroupByFieldMetadataSet"), 10));
                select.SelectByText(groupByMetadataSetName);

                select = new SelectElement(Driver.FindElement(By.Id(regionBaseName + "SelectedGroupByField"), 10));
                select.SelectByText(groupByFieldName);
            }

            select = new SelectElement(Driver.FindElement(By.Id(regionBaseName + "SelectedFunction"), 10));
            select.SelectByText(Enum.GetName(typeof(ItemQueryService.eFunctionMode), function));

            // Save the page
            Driver.FindElement(By.ClassName(UpdateButtonClass), 10).Click();
        }

        protected void CreateAndAddGraphToMain(string xAxisLabel, string xMetadataSet, string xFieldName, string yAxisLabel, string yMetadataSet, string yFieldName, float xDataScale, float yDataScale, string categoryMetadataSet = null, string categoryField = null, int regionIndex = 1)
        {
            // Create the Graph Section
            Driver.FindElement(By.LinkText(SettingsLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PageTypesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StandardPageLinkText), 10).Click();

            // add region to main page            
            Driver.FindElement(By.Id(RegionNameFieldId)).SendKeys(GraphPanelName);
            Driver.FindElement(By.Id(RegionInternalIdId)).SendKeys(GraphPanelName);

            IWebElement typeSelectorElement = Driver.FindElement(By.Id(RegionTypeSelectorId));
            SelectElement typeSelector = new SelectElement(typeSelectorElement);
            typeSelector.SelectByValue("Catfish.Models.Regions.GraphPanel");

            Driver.FindElement(By.Id(AddRegionButtonId)).Click();

            Driver.FindElement(By.LinkText(SaveLinkText), 10).Click();
            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PagesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StartLinkText), 10).SendKeys(Keys.Return);

            Driver.FindElement(By.XPath($@"//button[contains(.,'{GraphPanelName}')]"), 10).Click();

            // Define the region settings
            IWebElement region = Driver.FindElement(By.Id(GraphPanelName.Replace(" ", "")));
            SelectElement metadataSelect, fieldSelect;
            IWebElement scale;

            // Set the X Values
            region.FindElement(By.Id("Regions_" + regionIndex + "__Body_XaxisLabel")).SendKeys(xAxisLabel);

            metadataSelect = new SelectElement(region.FindElement(By.Id("Regions_" + regionIndex + "__Body_SelectedMetadataSetX")));
            metadataSelect.SelectByText(xMetadataSet);

            fieldSelect = new SelectElement(region.FindElement(By.Id("Regions_" + regionIndex + "__Body_XaxisField")));
            fieldSelect.SelectByText(xFieldName);

            scale = region.FindElement(By.Id("Regions_" + regionIndex + "__Body_XScale"));
            scale.Clear();
            scale.SendKeys(xDataScale.ToString());

            // Set the Y Values
            region.FindElement(By.Id("Regions_" + regionIndex + "__Body_YaxisLabel")).SendKeys(yAxisLabel);

            metadataSelect = new SelectElement(region.FindElement(By.Id("Regions_" + regionIndex + "__Body_SelectedMetadataSetY")));
            metadataSelect.SelectByText(yMetadataSet);

            fieldSelect = new SelectElement(region.FindElement(By.Id("Regions_" + regionIndex + "__Body_YaxisField")));
            fieldSelect.SelectByText(yFieldName);

            scale = region.FindElement(By.Id("Regions_" + regionIndex + "__Body_YScale"));
            scale.Clear();
            scale.SendKeys(yDataScale.ToString());

            // Set the category
            if (categoryMetadataSet != null)
            {
                metadataSelect = new SelectElement(region.FindElement(By.Id("Regions_" + regionIndex + "__Body_SelectedMetadataSetCat")));
                metadataSelect.SelectByText(categoryMetadataSet);

                fieldSelect = new SelectElement(region.FindElement(By.Id("Regions_" + regionIndex + "__Body_Category")));
                fieldSelect.SelectByText(categoryField);
            }

            // Save the page
            Driver.FindElement(By.ClassName(UpdateButtonClass), 10).Click();
        }

        protected void CreateAndAddEntityListToMain(int regionIndex = 1)
        {
            // create list entity region
            Driver.FindElement(By.LinkText(SettingsLinkText)).Click();
            Driver.FindElement(By.LinkText(PageTypesLinkText)).Click();
            Driver.FindElement(By.LinkText(StandardPageLinkText)).Click();

            // add region to main page            
            Driver.FindElement(By.Id(RegionNameFieldId)).SendKeys(EntityListRegionName);
            Driver.FindElement(By.Id(RegionInternalIdId)).SendKeys(EntityListRegionName);

            IWebElement typeSelectorElement = Driver.FindElement(By.Id(RegionTypeSelectorId));
            SelectElement typeSelector = new SelectElement(typeSelectorElement);
            typeSelector.SelectByValue("Catfish.Models.Regions.ListEntitiesPanel");

            Driver.FindElement(By.Id(AddRegionButtonId)).Click();

            // Save button does not contain the id set on other views toolbar_save_button
            // Instead we will click on "Save"
            Driver.FindElement(By.LinkText(SaveLinkText), 10).Click();
            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PagesLinkText), 10).Click();
            // Start is the link to the starting page
            // Send enter instead of clicking to get around element overlay
            Driver.FindElement(By.LinkText(StartLinkText), 10).SendKeys(Keys.Return);

            IWebElement region = Driver.FindElement(By.Id(EntityListRegionName.Replace(" ", "")));
            //Driver.FindElement(By.LinkText(regionName)).Click();
            Driver.FindElement(By.XPath($@"//button[contains(.,'{EntityListRegionName}')]"), 10).Click();
            region.FindElement(By.Id("Regions_" + regionIndex + "__Body_ItemPerPage")).SendKeys("10");
            region.FindElement(By.XPath(".//span[contains(@class, 'glyphicon glyphicon-plus-sign')]")).Click();
            Driver.FindElement(By.ClassName(UpdateButtonClass), 10).Click();
        }

        protected void NavigateToCollections()
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(CollectionsLinkText), 10).Click();
        }

        protected void NavigateToItems()
        {
            Driver.Navigate().GoToUrl(ManagerUrl);
            Driver.FindElement(By.LinkText(ItemsLinkText), 10).Click();
        }

        protected string FindTestValue(string expectedValue)
        {
            string val = "";
            IWebElement tbody = Driver.FindElement(By.ClassName("object-list"), 10);
            var cols = tbody.FindElements(By.TagName("td"));

            foreach (var col in cols)
            {
                if (!string.IsNullOrEmpty(col.Text) && col.Text.Contains(expectedValue)) //Item's name will display in all 3 lang separated by '/' -- eng is the first one
                {
                    val = col.Text.Split('/')[0].Trim();
                    break;
                }
            }
            return val;
        }
    }
}
