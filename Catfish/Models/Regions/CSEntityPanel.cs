using Catfish.Helpers;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Razor;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "CSEntityPanel")]
    [ExportMetadata("Name", "CSharp Entity Panel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class CSEntityPanel : CatfishRegion
    {
        [Display(Name = "Default Entity Id")]
        public int? DefaultEntityId { get; set; }

        [Display(Name = "CSHTML")]
        [Required]
        public string CsHtml { get; set; }

        public byte[] CompiledCode { get; set; }

        public CSEntityPanel() : base()
        {
            CsHtml = "@model Catfish.Models.ViewModels.EntityViewModel\n\n" +
                "@Html.HiddenFor(m => m.Id)\n" +
                "<p>@Model.Id</p>";

        }

        public override void InitManager(object model)
        {
            base.InitManager(model);
        }

        public override void OnManagerSave(object model)
        {
            Assembly result = ViewHelper.CompileView(CsHtml, "CustomCSEntityPanelView", "Catfish.Models.Regions.CSEntityPanel");

            // Convert the code to binary
            CompiledCode = System.IO.File.ReadAllBytes(result.Location);

            base.OnManagerSave(model);
        }

        public override object GetContent(object model)
        {
            return base.GetContent(model);
        }
    }
}