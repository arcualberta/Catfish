using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Reports
{
    public class ReportDataFields
    {
        public Guid FormTemplateId { get; set; }
        public string FormName { get; set; }
        public Guid FieldId { get; set; }
        public string FieldName { get; set; }
    }
}
