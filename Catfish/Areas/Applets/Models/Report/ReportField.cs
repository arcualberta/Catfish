using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Models.Report
{
	public class ReportField
	{
        public Guid FormTemplateId { get; set; }
        public string FormName { get; set; }
        public Guid FieldId { get; set; }
        public string FieldName { get; set; }
    }
}

