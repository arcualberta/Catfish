using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.ViewModels
{
    public class FormViewModel
    {
        public Form Form { get; set; }

        public List<DataFile> Files { get; set; }

        /// <summary>
        /// The Id of the Item which encapsulates this form data. This value is 0 for newly created forms.
        /// </summary>
        public int ItemId { get; set; }

        public string FormSubmissionRef { get; set; }

        public IDictionary<string, string[]> Errors { get; set; }
    }
}