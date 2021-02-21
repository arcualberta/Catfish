using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Expressions;
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

            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\simpleFormWorkflow_generared.xml");

        }

        [Test]
        public void VisibleIf_RequiredIf_Fields_Test()
        {
            string lang = "en";
            string templateName = "Visible-if Requiref-if Form Template";
            string submissionFormName = "Visible-if / Requiref-if Form";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);
            DataItem form = template.GetRootDataItem(false);
            Assert.IsNotNull(form);

            string[] options = new string[] { "Option 1", "Option 2", "Option 3", "Option 4" };
            var dd1 = form.CreateField<SelectField>("DD 1", lang, options, false);
            var rb1 = form.CreateField<RadioField>("RB 1", lang, options, false);
            var cb1 = form.CreateField<CheckboxField>("CB 1", lang, options, false);


            var textbox1 = form.CreateField<TextField>("Text 1", lang, false, false);
            textbox1.RequiredCondition
                .AppendLogicalExpression(dd1, ComputationExpression.eRelational.EQUAL, dd1.GetOption("Option 2", lang));
            textbox1.SetDescription("Required if DD 1 = Option 2", lang);

            var textbox2 = form.CreateField<TextField>("Text 2", lang, false, false);
            textbox2.RequiredCondition
                .AppendLogicalExpression(rb1, new Option[2] { rb1.GetOption("Option 1", lang), rb1.GetOption("Option 2", lang) }, ComputationExpression.eLogical.OR);
            textbox2.SetDescription("Required if RB 1 = Option 1 OR Option 2", lang);

            var textbox3 = form.CreateField<TextField>("Text 3", lang, false, false);
            textbox3.RequiredCondition
                .AppendLogicalExpression(cb1, new Option[2] { cb1.GetOption("Option 1", lang), cb1.GetOption("Option 3", lang) }, Core.Models.Contents.Expressions.ComputationExpression.eLogical.AND);
            textbox3.SetDescription("Required if CB 1 = Option 1 AND Option 3", lang);

            var textarea1 = form.CreateField<TextArea>("Text 4", lang, false, false);
            textarea1.VisibilityCondition
              .AppendLogicalExpression(dd1, ComputationExpression.eRelational.EQUAL, dd1.GetOption("Option 3", lang));
            textarea1.SetDescription("Visible if DD 1 = Option 3", lang);

            form.CreateField<DateField>("Date 1", lang)
                .SetDescription("Visible if DD 1 = Option 3", lang)
                .VisibilityCondition.AppendLogicalExpression(dd1, ComputationExpression.eRelational.EQUAL, dd1.GetOption("Option 3", lang));

            form.CreateField<IntegerField>("Integer 1", lang)
                .SetDescription("Visible if DD 1 = Option 3", lang)
                .VisibilityCondition.AppendLogicalExpression(dd1, ComputationExpression.eRelational.EQUAL, dd1.GetOption("Option 3", lang));

            form.CreateField<DecimalField>("Decimal 1", lang)
                .SetDescription("Visible if DD 1 = Option 3", lang)
                .VisibilityCondition.AppendLogicalExpression(dd1, ComputationExpression.eRelational.EQUAL, dd1.GetOption("Option 3", lang));

            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\visibleIf_RequiredIf_Fields_Workflow_generared.xml");

        }

        [Test]
        public void VisibleIf_RequiredIf_Options_Test()
        {
            string lang = "en";
            string templateName = "Visible-if Options Form Template";
            string submissionFormName = "Visible-if Options Form";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);
            DataItem form = template.GetRootDataItem(false);
            Assert.IsNotNull(form);

            string[] options = new string[] { "Option 1", "Option 2", "Option 3", "Option 4" };
            var dd1 = form.CreateField<SelectField>("DD 1", lang, options, false);

            //Drop-down menu with conditional options
            var options2 = new string[] { "A1", "A2", "B1", "B2", "B3", "B4" };
            var dd2 = form.CreateField<SelectField>("DD 2", lang, options2, true);
            dd2.SetDescription("The option group \"A\" should appear when Option 1 is selected for DD 1 and the option group \"B\" should appear other times.", lang);
            foreach (var option in dd2.Options.Where(op => op.OptionText.GetContent(lang).StartsWith("A")))
                option.VisibilityCondition.AppendLogicalExpression(dd1,
                    ComputationExpression.eRelational.EQUAL,
                    dd1.Options.Where(op => op.OptionText.GetContent(lang) == options[0]).First());

            foreach (var option in dd2.Options.Where(op => op.OptionText.GetContent(lang).StartsWith("B")))
                option.VisibilityCondition.AppendLogicalExpression(dd1,
                    ComputationExpression.eRelational.NOT_EQ,
                    dd1.Options.Where(op => op.OptionText.GetContent(lang) == options[0]).First());


            //Radio button set with conditional options
            var rb2 = form.CreateField<RadioField>("RB 2", lang, options2, true);
            rb2.SetDescription("The option group \"A\" should appear when Option 1 is selected for DD 1 and the option group \"B\" should appear other times.", lang);
            foreach (var option in rb2.Options.Where(op => op.OptionText.GetContent(lang).StartsWith("A")))
                option.VisibilityCondition.AppendLogicalExpression(dd1,
                    ComputationExpression.eRelational.EQUAL,
                    dd1.Options.Where(op => op.OptionText.GetContent(lang) == options[0]).First());

            foreach (var option in rb2.Options.Where(op => op.OptionText.GetContent(lang).StartsWith("B")))
                option.VisibilityCondition.AppendLogicalExpression(dd1,
                    ComputationExpression.eRelational.NOT_EQ,
                    dd1.Options.Where(op => op.OptionText.GetContent(lang) == options[0]).First());

            //Checkbox set with conditional options
            var cb2 = form.CreateField<CheckboxField>("CB 2", lang, options2, true);
            cb2.SetDescription("The option group \"A\" should appear when Option 1 is selected for DD 1 and the option group \"B\" should appear other times.", lang);
            foreach (var option in cb2.Options.Where(op => op.OptionText.GetContent(lang).StartsWith("A")))
                option.VisibilityCondition.AppendLogicalExpression(dd1,
                    ComputationExpression.eRelational.EQUAL,
                    dd1.Options.Where(op => op.OptionText.GetContent(lang) == options[0]).First());

            foreach (var option in cb2.Options.Where(op => op.OptionText.GetContent(lang).StartsWith("B")))
                option.VisibilityCondition.AppendLogicalExpression(dd1,
                    ComputationExpression.eRelational.NOT_EQ,
                    dd1.Options.Where(op => op.OptionText.GetContent(lang) == options[0]).First());


            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\visibleIf_Options_Workflow_generared.xml");

        }

        [Test]
        public void CompisiteFieldTest()
        {
            string lang = "en";
            string templateName = "Composite-field Form Template";
            string submissionFormName = "Composite-field Form";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);

            DataItem childForm = template.GetDataItem("Child Form", true, lang);
            //childForm.CreateField<TextField>("Name", lang);
            //childForm.CreateField<DateField>("DOB", lang);
            //childForm.CreateField<EmailField>("Email", lang);
            childForm.CreateField<TextArea>("Address", lang);
            //childForm.CreateField<RadioField>("Status", lang, new string[] { "Citizen", "Permenant Resident", "Visitor" });

            DataItem form = template.GetRootDataItem(true);
            Assert.IsNotNull(form);
            CompositeField cf = form.CreateField<CompositeField>("Person Info", lang);
            cf.Min = 1;
            cf.Max = 4;
            cf.AllowMultipleValues = true;
            cf.ChildTemplate = childForm;

            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\compositeField_Workflow_generared.xml");


        }
    }
}
