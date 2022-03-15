using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Reports
{
    public class ReportCell
    {
        public Guid FormTemplateId { get; set; }
        public Guid FieldId { get; set; }
        public List<ReportCellValue> Values { get; set; } = new List<ReportCellValue>();
    }

    public class ReportCellValue
	{
        public Guid FormInstanceId { get; set; }
        public List<Text> Values { get; set; } = new List<Text>();
    }
}
