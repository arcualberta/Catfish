using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Reports
{
    public class Report
    {
        public int Total { get; set; }
        public List<ReportRow> Rows { get; set; } = new List<ReportRow>();
    }
}
