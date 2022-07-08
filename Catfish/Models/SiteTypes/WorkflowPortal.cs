using Microsoft.AspNetCore.Mvc;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.SiteTypes
{
    [SiteType(Title = "Workflow Portal")]
    public class WorkflowPortal : CatfishWebsite
    {
        [Region(Title = "Workflow Pages", Display = RegionDisplayMode.Setting)]
        public WorkflowPages WorkflowPagesContent { get; set; }
    }

    public class WorkflowPages
    {
        [Field(Title = "Submission Entry Page Title", Options = FieldOption.HalfWidth, Placeholder = "Start a Submission")]
        public StringField SubmissionEntryPage { get; set; }
        public StringField SubmissionEntryPageId { get; set; }

        [Field(Title = "", Options = FieldOption.HalfWidth, Placeholder = "Create if not exist")]
        public CheckBoxField CreateSubmissionEntryPage { get; set; }


        [Field(Title = "Submission List Page Title", Options = FieldOption.HalfWidth, Placeholder = "List Submissions")]
        public StringField SubmissionListPage { get; set; }

        [Field(Title = "", Options = FieldOption.HalfWidth, Placeholder = "Create if not exist")]
        public CheckBoxField CreateSubmissionListPage { get; set; }

    }
}
