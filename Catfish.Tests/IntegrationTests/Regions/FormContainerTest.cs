using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using Catfish.Tests.IntegrationTests.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catfish.Tests.Extensions;
using OpenQA.Selenium.Support.UI;
using Catfish.Core.Contexts;

namespace Catfish.Tests.IntegrationTests.Regions
{
    [TestFixture(typeof(ChromeDriver))]
    class FormContainerTest<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        enum eQuestionType
        {
            TextBox,
            TextArea,
            Slider
        }

        /// <summary>
        /// Creates a basic form to be used for testing.
        /// </summary>
        /// <param name="title">The name of the form to be using.</param>
        /// <param name="sectionCount">how many sections will be shown.</param>
        /// <param name="questionTypesPerSection">The types of questions to be shown in each section.</param>
        /// <param name="sectionTitleFunc">How to define the title of each section.</param>
        /// <param name="questionTitleFunc">How to define the title of each question.</param>
        /// <param name="setSectionDetailsFunc">Adds any details to the section.</param>
        /// <param name="setQuestionDetailsFunc">Adds any details to the question.</param>
        /// <returns></returns>
        static Form GenerateTestForm(string name, int sectionCount, IEnumerable<eQuestionType> questionTypesPerSection, Func<int, string> sectionTitleFunc, Func<int, int, eQuestionType, string> questionTitleFunc, Action<int, CompositeFormField> setSectionDetailsFunc = null, Action < int, int, FormField> setQuestionDetailsFunc = null)
        {
            List<Form> sections = new List<Form>();

            for(int i = 0; i < sectionCount; ++i)
            {
                Form form = new Form();
                List<FormField> fields = new List<FormField>();
                int questionId = 0;
                foreach(eQuestionType type in questionTypesPerSection)
                {
                    ++questionId;

                    FormField field;

                    switch (type)
                    {
                        default:
                            field = new TextField();
                            break;
                    }

                    field.Name = questionTitleFunc(i, questionId, type);
                    if(setQuestionDetailsFunc != null)
                    {
                        setQuestionDetailsFunc(i, questionId, field);
                    }

                    fields.Add(field);
                }

                form.Fields = fields.AsReadOnly();
                sections.Add(form);
            }

            if(sections.Count == 1)
            {
                sections[0].Name = name;
                return sections[0];
            }

            Form result = new Form();
            List<FormField> resultFields = new List<FormField>();

            for(int i = 0; i < sections.Count; ++i)
            {
                Form section = sections[i];

                CompositeFormField field = new CompositeFormField();
                field.Fields = section.Fields;
                field.Name = sectionTitleFunc(i);

                if(setSectionDetailsFunc != null)
                {
                    setSectionDetailsFunc(i, field);
                }

                resultFields.Add(field);
            }

            result.Name = name;
            result.Fields = resultFields.AsReadOnly();
            return result;
        }

        static int CreateForm(Form form)
        {
            // TODO: We need to change these to a selenium method when possible.
            CatfishDbContext db = new CatfishDbContext();

            Catfish.Core.Contexts.AccessContext current = new AccessContext(AccessContext.PublicAccessGuid, true, db);
            AccessContext.current = current;

            SubmissionService formSrv = new SubmissionService(db);
            form.Serialize();

            int result = formSrv.SaveForm(form);
            db.SaveChanges();

            return result;
        }

        static string BaseGenerateSectionName(int id)
        {
            return "Section " + id;
        }

        static string BaseGenerateQuestionName(int sectionId, int questionId, eQuestionType type)
        {
            return string.Format("{0} {1}_{2}", Enum.GetName(typeof(eQuestionType), type), sectionId, questionId);
        }

        protected void CreateAndAddFormContainerToMain(string formName, int regionIndex = 1)
        {
            CreateBaseEntityType(false);
            CreateCollection(EntityTypeName, new FormField[] { new TextField() { Name = "Name" } });

            // Create the region
            Driver.FindElement(By.LinkText(SettingsLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PageTypesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StandardPageLinkText), 10).Click();

            // add region to main page            
            Driver.FindElement(By.Id(RegionNameFieldId)).SendKeys(FormContainerRegionName);
            Driver.FindElement(By.Id(RegionInternalIdId)).SendKeys(FormContainerRegionName);

            IWebElement typeSelectorElement = Driver.FindElement(By.Id(RegionTypeSelectorId));
            SelectElement typeSelector = new SelectElement(typeSelectorElement);
            typeSelector.SelectByValue("Catfish.Models.Regions.FormContainer");

            Driver.FindElement(By.Id(AddRegionButtonId)).Click();

            // Set the region properties
            Driver.FindElement(By.LinkText(SaveLinkText), 10).Click();
            Driver.FindElement(By.LinkText(ContentLinkText), 10).Click();
            Driver.FindElement(By.LinkText(PagesLinkText), 10).Click();
            Driver.FindElement(By.LinkText(StartLinkText), 10).SendKeys(Keys.Return);

            Driver.FindElement(By.XPath($@"//button[contains(.,'{FormContainerRegionName}')]"), 10).Click();

            IWebElement region = Driver.FindElement(By.Id(FormContainerRegionName.Replace(" ", "")));

            IWebElement element = region.FindElement(By.Id("Regions_" + regionIndex + "__Body_FormId"));
            SelectElement elementSelector = new SelectElement(element);
            elementSelector.SelectByText(formName);

            element = region.FindElement(By.Id("Regions_" + regionIndex + "__Body_EntityTypeId"));
            elementSelector = new SelectElement(element);
            elementSelector.SelectByText(EntityTypeName);

            element = region.FindElement(By.Id("Regions_" + regionIndex + "__Body_CollectionId"));
            elementSelector = new SelectElement(element);
            elementSelector.SelectByIndex(0);

            // Save the page
            Driver.FindElement(By.ClassName(UpdateButtonClass), 10).Click();
        }

        [Test]
        public void CanCreateFormContainer()
        {
            Action<int, CompositeFormField> sectionSettings = (sectionId, field) =>
            {
                field.StepState = CompositeFormField.eStepState.Accumulative;
            };

            Action<int, int, FormField> questionSettings = (sectionId, questionId, field) =>
            {
                field.IsRequired = true;
            };

            Form form = GenerateTestForm("Test Form", 2, new eQuestionType[] { eQuestionType.TextBox }, BaseGenerateSectionName, BaseGenerateQuestionName, sectionSettings, questionSettings);
            CreateForm(form);

            CreateAndAddFormContainerToMain(form.Name);
        }
    }
}
