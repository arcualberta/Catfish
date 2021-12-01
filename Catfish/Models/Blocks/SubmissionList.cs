using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Submission List", Category = "Workflow", Component = "submission-list", Icon = "fas fa-th-list")]
    public class SubmissionList : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<SubmissionList>();

        /// <summary>
        /// Specifies the title for the list in the public view
        /// </summary>
        [Display(Name = "List Title")]
        public StringField ListTitle { get; set; }

        public SubmissionList()
        {
            ListTitle = new StringField();
        }
    }
}
