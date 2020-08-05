using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class EntityEditPageModel : CatfishPageModelModel
    {
        private readonly IWorkflowService _workflowService;

        public DataItem Item { get; set; }
        
        [BindProperty]
        public TestData Data { get; set; }
        public EntityEditPageModel(IWorkflowService workflow) : base(null, null)
        {
            _workflowService = workflow;
        }
        public void OnGet()
        {
            string lang = "en";
            EntityTemplate template = new EntityTemplate();
            template.Name.SetContent("Trust Funded GRA/GRAF Contract");

            IWorkflowService ws = _workflowService;
            ws.SetModel(template);

            //Contract letter
            Item = ws.GetDataItem("Contract Letter", true);

            Item.SetDescription("This is the template for the contract letter.", lang);
            Item.CreateField<TextField>("First Name", lang, true);
            Item.CreateField<TextField>("Last Name", lang, true);
            Item.CreateField<TextField>("Student ID", lang, true);
            Item.CreateField<TextField>("Student Email", lang, true, true);
            Item.CreateField<TextField>("Department", lang, true, false, "East Asian Studies");
            
            SelectField typeOfAppointment = Item.CreateField<SelectField>("Type of Appointment", lang, new string[] { "Graduate Research Assistant", "Graduate Research Assistantship Fellowship" }, true, 0);
            typeOfAppointment.UpdateOptions(new string[] { "Assistant de recherche diplômé", "Bourse d'assistanat de recherche aux cycles supérieurs" }, "fr");

            Item.CreateField<TextField>("Assignment", lang, true);

            Item.CreateField<InfoSection>("Period of Appointment", lang, "alert alert-info");
            Item.CreateField<DateField>("Appointment Start", lang, true);
            Item.CreateField<DateField>("Appointment End", lang, true);

            Item.CreateField<InfoSection>("Stipend", lang, "alert alert-info");
            Item.CreateField<IntegerField>("Rate", lang, true);
            Item.CreateField<IntegerField>("Award", lang, true);
            Item.CreateField<IntegerField>("Salary", lang, true);

            Data = new TestData();
            Data.Children.Add(new TestChild() { Id = Guid.NewGuid(), ChildName = "Child 1" });
            Data.Children.Add(new TestChild() { Id = Guid.NewGuid(), ChildName = "Child 2" });

        }

        public void OnPost()
        {
            var data = Data;
        }
    }
}

public class TestData
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<TestChild> Children { get; set; }

    public TestData()
    {
        Children = new List<TestChild>();
    }
}

public class TestChild
{
    public Guid Id { get; set; }

    public string ChildName { get; set; }
}