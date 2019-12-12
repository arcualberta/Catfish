using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using Piranha.Extend;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Composition;
using Catfish.Core.Models.Forms;
using Catfish.Core.Models;
//using Catfish.Core.Services;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "GridPanel")]
    [ExportMetadata("Name", "GridPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class GridPanel : CatfishRegion
    {
        [Display(Name = "Desktop Column")]
        public string DesktopColNum { get;set;} //Number of colum display on Desktop

        [Display(Name = "Mobile Column")]
        public string MobileColNum {get;set;   } //Number of colum display on Mobile devices
        [Display(Name = "Entries Content")]
        public List<string> Entries { get; set; }
        [NotMapped]
        [ScriptIgnore]
        public SelectList AllowColumns { get; set; }
       
        public GridPanel()
        {
            Entries = new List<string> {  };
            Entries.Add(string.Empty);
        }
        public override void InitManager(object model)
        {
            AllowColumns = new SelectList(
                                new List<SelectListItem>
                                {
                                    new SelectListItem { Text = "1", Value = "1"},
                                    new SelectListItem {  Text = "2", Value = "2"},
                                    new SelectListItem {  Text = "3", Value = "3"},
                                    new SelectListItem {  Text = "4", Value = "4"},
                                    new SelectListItem {  Text = "6", Value = "6"},
                                    new SelectListItem {  Text = "12", Value = "12"}
                                }, "Value", "Text");
            base.InitManager(model);
        }
        //public override object GetContent(object model)
        //{
        //For testing -- go to the page that use this region and add ?entity=[entityId]
        //HttpContext context = HttpContext.Current;

        //if (context != null)
        //{
        //    string entityId = context.Request.QueryString[EntityContainer.ENTITY_PARAM];

        //    if (!string.IsNullOrEmpty(entityId))
        //    {
        //        CatfishDbContext db = new CatfishDbContext();
        //        EntityService es = new EntityService(db);

        //        Entity = es.GetAnEntity(Convert.ToInt32(entityId));
        //    }
        //}

        //    return base.GetContent(model);
        //}


    }
}
