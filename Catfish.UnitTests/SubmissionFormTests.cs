using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Core.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.UnitTests
{
    class SubmissionFormTests
    {
        private AppDbContext _db;
        private TestHelper _testHelper;
        private IAuthorizationService _auth;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
            IAuthorizationService _auth = _testHelper.AuthorizationService;
        }

        public ItemTemplate SubmissionItemTemplate(string templateName, string submissionFormName, string lang)
        {
            IWorkflowService ws = _testHelper.WorkflowService;

            ItemTemplate template = _db.ItemTemplates
                .Where(et => et.TemplateName == templateName)
                .FirstOrDefault();

            ItemTemplate t_new = ws.CreateBasicSubmissionTemplate(templateName, submissionFormName, lang);
            if (template == null)
            {
                template = t_new;
                _db.ItemTemplates.Add(template);
            }
            else
            {
                t_new.Id = template.Id;
                template.Data = t_new.Data;
                template.Initialize(false);
            }
            template.TemplateName = templateName;
            template.Name.SetContent(templateName);

            return template;
        }

        [Test]
        public void SimpleFormTest()
        {
            string lang = "en";
            string templateName = "Simple Form Template";
            string submissionFormName = "Simple Form";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);
            DataItem form = template.GetRootDataItem(false);
            Assert.IsNotNull(form);

            string[] options = new string[] { "Option 1", "Option 2", "Option 3", "Option 4" };
            form.CreateField<SelectField>("DD 1", lang, options, false);
            form.CreateField<RadioField>("RB 1", lang, options, false);
            form.CreateField<CheckboxField>("CB 1", lang, options, false);

            //Get the Workflow object using the workflow service
            Workflow workflow = template.Workflow;


            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\simpleFormWorkflow_generared.xml");

        }

    }
}
