using Catfish.Core.Models;
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
        public void WorkflowBuildTest()
        {
            EntityTemplate template = new EntityTemplate();
            template.Name.SetContent("Trust Funded GRA/GTA Contract");
            
            IWorkflowService ws = _testHelper.WorkflowService;
            ws.SetModel(template);
            
            EmailTemplate trustAccountHolderNotification = ws.GetEmailTemplate("Trust Account Holder Notification", true);
            trustAccountHolderNotification.SetSubject("Trust-funded Contract Review");
            trustAccountHolderNotification.SetBody("Please review the contract letter below and provide your approval decision. Thanks you.");

            template.Data.Save("entityTemplate.xml");
        }

    }
}
