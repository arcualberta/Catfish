using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Reports
{
    public class SubmissionReport : BaseReport
    {
        //public const string TagName = "report";
        public SubmissionReport() {}
       public SubmissionReport(XElement data) : base(data) { }
    }
}
