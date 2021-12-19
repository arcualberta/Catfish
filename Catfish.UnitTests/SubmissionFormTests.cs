using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Expressions;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Reports;
using Catfish.Core.Services;
using Catfish.Test.Helpers;
using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
            form.CreateField<SelectField>("DD 1", lang, options, true);
            form.CreateField<RadioField>("RB 1", lang, options, true);
            form.CreateField<CheckboxField>("CB 1", lang, options, false);
            form.CreateField<TextField>("TF 1", lang);
            form.CreateField<TextArea>("TA 1", lang).Cols = 22;
            form.CreateField<IntegerField>("INT 1", lang);
            form.CreateField<DecimalField>("DEC 1", lang);
            form.CreateField<DateField>("DATE 1", lang);
            form.CreateField<AttachmentField>("Supporting Documentation", lang).SetDescription(@"Please attach the required travel and conference supporting documentation here as <span style='color: Red;'>a <b>single PDF document</b>. [Be sure to review the section of the Policies and Procedures on required supporting documentation]</span>", lang);




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
            childForm.CreateField<TextField>("Name", lang, true);
            childForm.CreateField<DateField>("DOB", lang, false);
            childForm.CreateField<EmailField>("Email", lang);
            var ta = childForm.CreateField<TextArea>("Address", lang);
            ta.Cols = 40;
            childForm.CreateField<RadioField>("Status", lang, new string[] { "Citizen", "Permenant Resident", "Visitor" });
            childForm.CreateField<CheckboxField>("Languages", lang, new string[] { "English", "French", "Spanish" });
            
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

        [Test]
        public void CompisiteFieldComputedExpressionTest()
        {
            string lang = "en";
            string templateName = "Composite-field Child-field Sum Form Template";
            string submissionFormName = "Composite-field Child-field Sum Form";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);

            DataItem childForm = template.GetDataItem("Child Form", true, lang);
            childForm.CreateField<TextField>("Name", lang, false);
            childForm.CreateField<IntegerField>("Employee Count", lang, true);
            childForm.CreateField<DecimalField>("Salary", lang, true);
            childForm.CreateField<DecimalField>("Budget", lang, true);

            DataItem form = template.GetRootDataItem(true);
            Assert.IsNotNull(form);
            CompositeField cf = form.CreateField<CompositeField>("Organization", lang);
            cf.Min = 2;
            cf.Max = 4;
            cf.AllowMultipleValues = true;
            cf.ChildTemplate = childForm;

            var totalEmployeeCount = form.CreateField<IntegerField>("Total Employee Count", lang);
            totalEmployeeCount.Readonly = true;
            totalEmployeeCount.ValueExpression.AppendCompositeFieldSum(cf, 1);

            var totalSalary = form.CreateField<DecimalField>("Total Salary", lang);
            totalSalary.Readonly = true;
            totalSalary.ValueExpression.AppendCompositeFieldSum(cf, 2);

            var totalBudget = form.CreateField<IntegerField>("Total Budget", lang);
            totalBudget.Readonly = true;
            totalBudget.ValueExpression.AppendCompositeFieldSum(cf, 3);

            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\compositeFieldChildFieldSum_Workflow_generared.xml");
        }

        [Test]
        public void TableFieldTest()
        {
            string lang = "en";
            string templateName = "Table-field Form Template";
            string submissionFormName = "Table-field Form";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);

            DataItem form = template.GetRootDataItem(true);
            Assert.IsNotNull(form);

            TableField tf = form.CreateField<TableField>("Product List", lang, false, 1, 10);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";
            tf.TableHead.CreateField<DateField>("Date", lang);
            tf.TableHead.CreateField<TextField>("Product", lang, false);
            tf.TableHead.CreateField<TextArea>("Description", lang);
            var unitPrice = tf.TableHead.CreateField<DecimalField>("Unit Price", lang);
            var qty1 = tf.TableHead.CreateField<IntegerField>("Quantity 1", lang);
            var qty2 = tf.TableHead.CreateField<IntegerField>("Quantity 2", lang);

            var totQty = tf.TableHead.CreateField<IntegerField>("Total Qty", lang);
            totQty.ValueExpression
                .AppendValue(qty1)
                .AppendOperator(ComputationExpression.eMath.PLUS)
                .AppendValue(qty2);
            totQty.Readonly = true;

            var totalCost = tf.TableHead.CreateField<DecimalField>("Total", lang);
            totalCost.ValueExpression
                .AppendValue(unitPrice)
                .AppendOperator(ComputationExpression.eMath.MULT)
                .AppendValue(totQty);
            totalCost.Readonly = true;

            tf.TableHead.CreateField<RadioField>("Availability", lang, new string[] { "Available", "Not available" }, false);
            tf.TableHead.CreateField<CheckboxField>("Category", lang, new string[] { "Health", "Prescription", "Beauty", "Nutrition" });

            tf.AppendRows(1);


            TableRow footer = tf.AppendRow(TableField.eRowTarget.Footer);
            footer.SetReadOnly();

            //The first 4 fields and the last two fields in the footer are meaningless so 
            //we exclude them from rendering
            foreach (var i in new int[] { 0, 1, 2, 3, 8, 9 })
                footer.Fields[i].Exclude = true;

            for (var i = 4; i <= 7; ++i)
            {
                //The footer doesn't need value expressions inherited 
                //from the header elements, so we clear them first
                footer.Fields[i].ValueExpression.Clear();

                //Set the column-sum as the value expression.
                footer.Fields[i].ValueExpression.AppendColumnSum(tf, i);
            }


            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\tableField_Workflow_generared.xml");

        }

        [Test]
        public void TableField2Test()
        {
            string lang = "en";
            string templateName = "Table-field Form Template 2";
            string submissionFormName = "Table-field Form 2";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);

            DataItem form = template.GetRootDataItem(true);
            Assert.IsNotNull(form);

            string[] estConfBud = new string[] { "Total Funding From Other Sources", "Registration Fees" };

            TableField ctf = form.CreateField<TableField>("", lang, true, estConfBud.Length, estConfBud.Length);
            ctf.FieldLabelCssClass = "col-md-12";
            ctf.FieldValueCssClass = "col-md-12";

            ctf.TableHead.CreateField<InfoSection>("", lang, "", "Revenue");

            ctf.TableHead.CreateField<DecimalField>("Estimated ($)", lang);


            TableRow cfooter = ctf.AppendRow(TableField.eRowTarget.Footer);
            cfooter.Fields[0].SetValue("Total Revenue", lang);
            cfooter.SetReadOnly();
            for (var i = 1; i < cfooter.Fields.Count; ++i)
                cfooter.Fields[i].ValueExpression.AppendColumnSum(ctf, i);

            ctf.SetColumnValues(0, estConfBud, lang);
            ctf.AllowAddRows = false;

            ////
            ////TableField cctf = form.CreateField<TableField>("", lang, true, 1, 10);
            ////cctf.FieldLabelCssClass = "col-md-12";
            ////cctf.FieldValueCssClass = "col-md-12";
            ////cctf.TableHead.CreateField<InfoSection>("", lang);
            ////cctf.TableHead.CreateField<DecimalField>("Itemized Costs", lang);

            ////cctf.TableHead.CreateField<DecimalField>("Estimated Cost ($)", lang);


            ////TableRow ccfooter = cctf.AppendRow(TableField.eRowTarget.Footer);
            ////ccfooter.Fields[0].SetValue("Total Cost", lang);
            ////ccfooter.SetReadOnly();
            ////for (var i = 1; i < ccfooter.Fields.Count; ++i)
            ////    ccfooter.Fields[i].ValueExpression.AppendColumnSum(cctf, i);

            ////cctf.AllowAddRows = true;
            ////cctf.AppendRows(1);
            //////TODO TABLE FIELD -- GRAND TOTAL

            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\tableField2_Workflow_generared.xml");

        }

        [Test]
        public void GridTableFieldTest()
        {
            string lang = "en";
            string templateName = "Grid table-field Form Template";
            string submissionFormName = "Grid Table-field Form";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);

            DataItem form = template.GetRootDataItem(true);
            Assert.IsNotNull(form);

            string[] categories = new string[] { "Faculty", "Students", "Other" };

            TableField tf = form.CreateField<TableField>("Attendees", lang, false, categories.Length, categories.Length);
            tf.FieldLabelCssClass = "col-md-12";
            tf.FieldValueCssClass = "col-md-12";
            tf.TableHead.CreateField<InfoSection>("", lang); 
            var uofaPeople = tf.TableHead.CreateField<IntegerField>("U of A", lang);
            var otherCanada = tf.TableHead.CreateField<IntegerField>("Other Canada", lang);
            var other = tf.TableHead.CreateField<IntegerField>("Other Countries", lang);

            var totalPeople = tf.TableHead.CreateField<IntegerField>("Total # of People", lang);
            totalPeople.ValueExpression
                .AppendValue(uofaPeople)
                .AppendOperator(ComputationExpression.eMath.PLUS)
                .AppendValue(otherCanada)
                .AppendOperator(ComputationExpression.eMath.PLUS)
                .AppendValue(other);
            totalPeople.Readonly = true;

            var regRate = tf.TableHead.CreateField<DecimalField>("Registration Fee per Person", lang);

            var totRegFee = tf.TableHead.CreateField<DecimalField>("Total Registration Fee", lang);
            totRegFee.ValueExpression
                .AppendValue(totalPeople)
                .AppendOperator(ComputationExpression.eMath.MULT)
                .AppendValue(regRate);
            totRegFee.Readonly = true;

            //NOTE: we MUST finish defining all columns before setting any column values.
            tf.SetColumnValues(0, categories, lang);

            //Inseting the footer row and setting its elements to 
            //sum of respective columns
            TableRow footer = tf.AppendRow(TableField.eRowTarget.Footer);
            footer.Fields[0].SetValue("Total", lang);
            footer.SetReadOnly();
            for (var i = 1; i < footer.Fields.Count; ++i)
            {
                //The footer doesn't need value expressions inherited 
                //from the header elements, so we clear them first
                footer.Fields[i].ValueExpression.Clear();

                //Set the column-sum as the value expression.
                footer.Fields[i].ValueExpression.AppendColumnSum(tf, i);
            }

            //Don't allow adding more rows.
            tf.AllowAddRows = false;

            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\gridTableField_Workflow_generared.xml");

        }

        [Test]
        public void ReportTest()
        {
            string lang = "en";
            string templateName = "Report Test Template";
            string submissionFormName = "Report Test";

            ItemTemplate template = SubmissionItemTemplate(templateName, submissionFormName, lang);
            DataItem form = template.GetRootDataItem(false);
            Assert.IsNotNull(form);
            var txt1 = form.CreateField<TextField>("Text 1", lang, false);
            var ta1 = form.CreateField<TextArea>("Area 1", lang, false);

            string[] options = new string[] { "Option 1", "Option 2", "Option 3", "Option 4" };
            var dd1 = form.CreateField<SelectField>("DD 1", lang, options, false);
            var rb1 = form.CreateField<RadioField>("RB 1", lang, options, false);
            var chk1 = form.CreateField<CheckboxField>("CB 1", lang, options, false);

            DataItem childForm = template.GetDataItem("Child Form", true, lang);
            var childName = childForm.CreateField<TextField>("Child Name", lang, true);
            childForm.CreateField<DateField>("DOB", lang, false);
            //var cf1 = form.CreatedFied<CompositeField>("CF 1", lang);

            CompositeField cf = form.CreateField<CompositeField>("Child Info", lang);
            cf.Min = 1;
            cf.Max = 1;
            cf.AllowMultipleValues = false;
            cf.ChildTemplate = childForm;

            var report = template.GetReport<SubmissionReport>("Submission Report",template.Id,  true);
            report
                .AddField(form.Id, txt1.Id,"")
                .AddField(form.Id, ta1.Id, "")
                .AddField(form.Id, cf.Id, childName.Id, "Child's Name"); //adding compositeField's field ==> cf.Id = compositeFieldId, childName => field in composteField
            //    .AddField(cf1, chidFormField);



            _db.SaveChanges();
            template.Data.Save("..\\..\\..\\..\\Examples\\reportTestFormWorkflow_generared.xml");

        }

        [Test]
        public void SASReportTest()
        {
            string file = "..\\..\\..\\..\\Examples\\production\\_prod_sas.xml";
            ItemTemplate template = ItemTemplate.Parse(XDocument.Parse(File.ReadAllText(file)).Root) as ItemTemplate;
            var mainForm = template.GetRootDataItem(false);

            //Pull the fields of Applicant Name, Email and Department out of the mainForm and build a report
            //to show those entries + submission date + status in it.
            //074fe283-40e8-43b2-b015-9a5221f856a1 -- template Id

            var applicantName = mainForm.Fields.Where(f => f.Id.ToString() == "cd51f792-d01c-421f-a0b4-84c5e3c81482").FirstOrDefault();
            var applicantEmail = mainForm.Fields.Where(f => f.Id.ToString() == "5c0403c1-1fa7-4281-853a-f8c2904ac825").FirstOrDefault();
            var department = mainForm.Fields.Where(f => f.Id.ToString() == "ecd7a734-3d26-4794-99cb-c393ab0779f1").FirstOrDefault();
            var report = template.GetReport<SubmissionReport>("SAS Report", template.Id, true);
            report
                .AddField(mainForm.Id, applicantName.Id, "")
                .AddField(mainForm.Id, applicantEmail.Id, "")
                .AddField(mainForm.Id, department.Id, "");
               

            template.Data.Save(file.Substring(0, file.LastIndexOf(".")) + "_with_report.xml");
        }

        [Test]
        public void ConferenceFundReportTest()
        {
            string file = "..\\..\\..\\..\\Examples\\production\\_prod_conf.xml";
            ItemTemplate template = ItemTemplate.Parse(XDocument.Parse(File.ReadAllText(file)).Root) as ItemTemplate;
            var mainForm = template.GetRootDataItem(false);

            var applicantName = mainForm.Fields.Where(f => f.Id.ToString() == "398350ea-a5dc-4fd8-9cf5-1c8aceca2f39").FirstOrDefault();
            var applicantEmail = mainForm.Fields.Where(f => f.Id.ToString() == "14491728-17f4-45b4-87ce-5dccfd878c08").FirstOrDefault();
            var department = mainForm.Fields.Where(f => f.Id.ToString() == "235a4d58-b6fc-4118-ad32-f4a93667ec3d").FirstOrDefault();
            var report = template.GetReport<SubmissionReport>("GAP Conference Report", template.Id, true);
            report
                .AddField(mainForm.Id, applicantName.Id, "")
                .AddField(mainForm.Id, applicantEmail.Id, "")
                .AddField(mainForm.Id, department.Id, "");


            template.Data.Save(file.Substring(0, file.LastIndexOf(".")) + "_with_report.xml");
        }

        [Test]
        public void CovidInspectionReportTest()
        {
            string file = "..\\..\\..\\..\\Examples\\production\\_prod_covid_inspection.xml";
            ItemTemplate template = ItemTemplate.Parse(XDocument.Parse(File.ReadAllText(file)).Root) as ItemTemplate;
            var mainForm = template.GetRootDataItem(false);

            var inspectionDate = mainForm.Fields.Where(f => f.Id.ToString() == "6fdef487-d218-497f-b5a3-95951a463e18").FirstOrDefault();
            var building = mainForm.Fields.Where(f => f.Id.ToString() == "cb221ab8-a7b3-4a98-b206-d133daffd64a").FirstOrDefault();
            var room     = mainForm.Fields.Where(f => f.Id.ToString() == "f50d20fe-4b3e-4039-aba5-49f969211148").FirstOrDefault();
            var inspectedBy = mainForm.Fields.Where(f => f.Id.ToString() == "28ceba59-04f7-4224-b73d-c636ae95b9de").FirstOrDefault();
            var report = template.GetReport<SubmissionReport>("GAP Conference Report", template.Id, true);
            report
                .AddField(mainForm.Id, inspectionDate.Id, "")
                .AddField(mainForm.Id, building.Id, "")
                .AddField(mainForm.Id, room.Id, "")
                .AddField(mainForm.Id, inspectedBy.Id, "");



            template.Data.Save(file.Substring(0, file.LastIndexOf(".")) + "_with_report.xml");
        }


    }
}
