namespace Catfish.API.Repository.Models.Import
{
    public class ExcelData
    {
        public Guid TemplateId { get; set; }
        public string TemplateName { get; set; }
        public bool ImportTemplateOnly { get; set; }
        public IFormFile ExcelFile { get; set; }
        public string PivotColumn { get; set; }//common column among the spreat sheets

        public string PrimarySheet { get; set; }
    }
}
