using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Reports
{
    public class ReportCell
    {
        public Guid FormId { get; set; }
        public Guid FieldId { get; set; }
        public string Value { get; set; }
    }
}
