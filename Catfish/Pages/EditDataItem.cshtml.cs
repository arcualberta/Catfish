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
    public class EditDataItemModel : CatfishPageModelModel
    {
        private readonly IWorkflowService _workflowService;

        public DataItem DataItem { get; set; }
        public EditDataItemModel(IWorkflowService workflow) : base(null, null)
        {
            _workflowService = workflow;
        }
        public void OnGet(Guid? id = null)
        {
            string lang = "en";
            EntityTemplate template = new EntityTemplate();
            template.Name.SetContent("Trust Funded GRA/GRAF Contract");

            IWorkflowService ws = _workflowService;
            ws.SetModel(template);

            //Contract letter
            DataItem = ws.GetDataItem("Contract Letter", true);

            DataItem.SetDescription("This is the template for the contract letter.", lang);
            DataItem.CreateField<TextField>("First Name", lang, true);
            DataItem.CreateField<TextField>("Last Name", lang, true);
            DataItem.CreateField<TextField>("Student ID", lang, true);
            DataItem.CreateField<TextField>("Student Email", lang, true, true);
            DataItem.CreateField<TextField>("Department", lang, true, false, "East Asian Studies");
            DataItem.CreateField<SelectField>("Type of Appointment", lang, new string[] { "Graduate Research Assistant", "Graduate Research Assistantship Fellowship" }, true, 0);
            DataItem.CreateField<TextField>("Assignment", lang, true);

            DataItem.CreateField<InfoSection>("Period of Appointment", lang, "alert alert-info");
            DataItem.CreateField<DateField>("Appointment Start", lang, true);
            DataItem.CreateField<DateField>("Appointment End", lang, true);

            DataItem.CreateField<InfoSection>("Stipend", lang, "alert alert-info");
            DataItem.CreateField<DecimalField>("Rate", lang, true);
            DataItem.CreateField<DecimalField>("Award", lang, true);
            DataItem.CreateField<DecimalField>("Salary", lang, true);
        }

        public void OnPost(DataItem item)
        {
            DataItem = item;
        }
    }
}