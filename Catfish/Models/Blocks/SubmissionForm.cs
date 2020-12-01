
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Models.Fields;
using ElmahCore;
using Piranha.AspNetCore.Identity.SQLServer;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Catfish.Models.Blocks
{

    // private Enum eCollection = Enum.TryParse(EType, "Collection");
    [BlockType(Name = "Submission Form", Category = "Workflow", Component = "submission-form", Icon = "fab fa-wpforms")]
    public class SubmissionForm : Block
    {
        [Display(Name = "Css Class")]
        public StringField CssClass { get; set; }

        [Display(Name = "Submission Confirmation")]
        public TextField SubmissionConfirmation { get; set; }

        public CatfishSelectList<Collection> Collections { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }

        public TextField SelectedCollection { get; set; }

        public TextField SelectedItemTemplate { get; set; }

        public TextField WorkflowFunction { get; set; }

        public TextField WorkflowGroup { get; set; }

        public IList<Group> AvailableGroups { get; set; }
        public TextField SelectedGroupIds { get; set; }

        public void Init(AppDbContext db, IdentitySQLServerDb pdb, IGroupService srv, ErrorLog errorLog)
        {
            if (srv == null)
                srv = new GroupService(db, pdb, errorLog);

            AvailableGroups = srv.GetGroupList();
        }


    }
}
