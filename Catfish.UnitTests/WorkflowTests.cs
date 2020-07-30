using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.Workflow;
using Catfish.Services;
using Catfish.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.UnitTests
{
    public class WorkdlowTests
    {
        protected AppDbContext _db;
        protected TestHelper _testHelper;

        [SetUp]
        public void Setup()
        {
            _testHelper = new TestHelper();
            _db = _testHelper.Db;
        }


        [Test]
        public void ContractLetterWorkflowBuildTest()
        {
            string lang = "en";
            EntityTemplate template = new EntityTemplate();
            template.Name.SetContent("Trust Funded GRA/GRAF Contract");
            
            IWorkflowService ws = _testHelper.WorkflowService;
            ws.SetModel(template);
            
            //Email template for trust account hoder
            EmailTemplate trustAccountHolderNotification = ws.GetEmailTemplate("Trust Account Holder Notification", true);
            trustAccountHolderNotification.SetDescription("This metadata set defines the email template to be sent for the trust-account holder when a new contract is created.", lang);
            trustAccountHolderNotification.SetSubject("Trust-funded Contract Review");
            trustAccountHolderNotification.SetBody("Please review @Link[this contract letter|@Model] to be funded by one of your trust accounts and provide your decision within 2 weeks.\n\nThank you");

            //Email template for the associate chair
            EmailTemplate deptChairNotification = ws.GetEmailTemplate("Associate Chair Notification", true);
            deptChairNotification.SetDescription("This metadata set defines the email template to be sent for the associate chair when a new contract is created.", lang);
            deptChairNotification.SetSubject("Graduate Contract Review (Trust Funded)");
            deptChairNotification.SetBody("Please review @Link[this contract letter|@Model] to be funded by a trust accounts and provide your decision within 2 weeks.\n\nThank you");

            //Email template for the department admin
            EmailTemplate deptAdminNotification = ws.GetEmailTemplate("Department Admin Notification", true);
            deptAdminNotification.SetDescription("This metadata set defines the email template to be sent for the program admin when another party makes a change to a contract.", lang);
            deptAdminNotification.SetSubject("Cnotract Status Update (Trust Funded)");
            deptAdminNotification.SetBody("The status of @Link[this contract letter|@Model] has been updated.\n\nThank you");

            //Contract letter
            DataItem contract = ws.GetDataItem("Contract Letter", true);
            contract.SetDescription("This is the template for the contract letter.", lang);
            contract.CreateField<TextField>("First Name", lang, true);
            contract.CreateField<TextField>("Last Name", lang, true);
            contract.CreateField<TextField>("Student ID", lang, true);
            contract.CreateField<TextField>("Student Email", lang, true);
            contract.CreateField<TextField>("Department", lang, true, "East Asian Studies");
            contract.CreateField<SelectField>("Type of Appointment", lang, new string[] { "Graduate Research Assistant", "Graduate Research Assistantship Fellowship" }, true, 0);
            contract.CreateField<TextField>("TextField", lang, true);

            contract.CreateField<InfoSection>("Period of Appointment", lang, "alert alert-info");
            contract.CreateField<DateField>("Appointment Start", lang, true);
            contract.CreateField<DateField>("Appointment End", lang, true);

            contract.CreateField<InfoSection>("Stipend", lang, "alert alert-info");
            contract.CreateField<NumberField>("Rate", lang, true);
            contract.CreateField<NumberField>("Award", lang, true);
            contract.CreateField<NumberField>("Salary", lang, true);



            //Save the template
            template.Data.Save("..\\..\\..\\..\\Examples\\ContractLetterWorkflow.xml");
        }

        public void CalendarManagementSystemWorkflowBuildTest()
        {
            string lang = "en";
            EntityTemplate template = new EntityTemplate();
            template.Name.SetContent("Calendar management System Workflow");

            IWorkflowService ws = _testHelper.WorkflowService;
            ws.SetModel(template);



            template.Data.Save("..\\..\\..\\..\\Examples\\CalendarManagementWorkflow.xml");

        }
    }
}
