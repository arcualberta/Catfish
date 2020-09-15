using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.AttributeBuilder;
using Piranha.Models;

namespace Catfish.Pages.Workflow
{
    [PageType(Title = "Submission Entry List Page")]
    [PageTypeRoute(Title = "Default", Route = "/workflow/submission-entries")]
    public class SubmissionEntryListModel : Page<SubmissionEntryListModel>
    {
    }
}
