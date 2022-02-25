using Catfish.Models.Blocks;
using Piranha.Extend;

using Piranha;

using Piranha.Extend.Fields;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Catfish.Models.Fields;
using Catfish.Core.Models;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Report", Category = "Submissions", Component = "report", Icon = "fas fa-chart-pie")]
    public class Report : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<Report>();

        public List<SelectList> Collections { get; set; } = new List<SelectList>();
        public TextField SelectedCollectionId { get; set; }
        public TextField SelectedGroupId { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }
        public TextField SelectedItemTemplateId { get; set; }
        //  public SelectListItem[] SelectedFields { get; set; } 
        public ReportField[] SelectedFields { get; set; }
        public TextField SelectedField { get; set; }
        public List<TextField> AvailableFormIds { get; }
        public TextField SelectedFormId { get; set; }

        //public List<ReportField> GetSelectedFields()
        //{
        //    List<ReportField> fields = new List<ReportField>();
        //    for(int i=0; i < SelectedFields.Length; i++)
        //    {
        //        string[] group = SelectedFields[i].Group.Name.Split(":");
        //        ReportField fld = new ReportField() { FieldId = SelectedFields[i].Value, FieldName = SelectedFields[i].Text, FormId = group[0], FormName = group[1] };

        //        fields.Add(fld);
        //    }

        //    return fields;
        //}
    }

    public class ReportField
    {
        public string FormId { get; set; }
        public string FormName { get; set; }
        public string FieldId { get; set; }
        public string FieldName { get; set; }
    }
}
