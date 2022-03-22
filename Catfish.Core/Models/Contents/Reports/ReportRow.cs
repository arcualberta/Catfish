using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Reports
{
    public class ReportRow
    {
        public Guid ItemId { get; set; }
        public List<ReportCell> Cells { get; set; } = new List<ReportCell>();
    }
}
