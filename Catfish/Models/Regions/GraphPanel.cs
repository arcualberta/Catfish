using Catfish.Core.Models;
using Catfish.Core.Services;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Catfish.Core.Models.Forms;

namespace Catfish.Models.Regions
{
   
    public class GraphQueryObject
    {
        public int YValue { get; set; }
        public decimal XValue { get; set; }
        public int? Count  { get; set; }
        public string Category { get; set; }
    }

    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "GraphPanel")]
    [ExportMetadata("Name", "GraphPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class GraphPanel : CatfishRegion
    {

        public enum GraphType { Line = 0, Bar }

        [Display(Name = "X-Axis Label")]
        public string XaxisLabel { get; set; }
        [Display(Name = "X-Axis Field")]
        public string XaxisField { get; set; } //the guid of metadataset Field
        public string MetadataSet { get; set; } //the guid of MetadataSet

        [Display(Name = "Y-Axis Label")]
        public string YaxisLabel { get; set; }

        [Display(Name = "Y-Axis Field")]
        public string YaxisField { get; set; }

        public string Category { get; set; }
        [Display(Name = "Graph Type")]
        public GraphType SelectedGraphType { get; set; }
        [Display(Name = "Graph Title")]
        public string GraphTitle { get; set; }

        [Display(Name = "X-data Scale")]
        public int XScale {get; set;}

        [Display(Name = "Y-data Scale")]
        public int YScale {get; set;}
        
        [ScriptIgnore]
        public List<SelectListItem> ListMetadataSets { get; set; }

        [ScriptIgnore]
        public IDictionary<string, List<SelectListItem>> MetadataFields { get; set; }
        [ScriptIgnore]
        public List<SelectListItem> GraphTypes { get; set; }

        [ScriptIgnore]
        public bool IsCategoryOptionsField { get; set; }

        public string SelectedMetadataSetX { get; set; }
        public string SelectedMetadataSetY { get; set; }

        public string SelectedMetadataSetCat { get; set; }



        public GraphPanel()
        {
            ListMetadataSets = new List<SelectListItem>();
            MetadataFields = new Dictionary<string, List<SelectListItem>>();
            GraphTypes = new List<SelectListItem>();
            IsCategoryOptionsField = false;

            foreach (GraphType am in Enum.GetValues(typeof(GraphType)))
            {
                GraphTypes.Add(new SelectListItem { Text = am.ToString(), Value = am.ToString() });
            }

            XScale = 1;
            YScale = 1;
        }

        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
            MetadataService metadataService = new MetadataService(db);
            IQueryable<CFMetadataSet> metadataSets = metadataService.GetMetadataSets();
            foreach(CFMetadataSet m in metadataSets)
            {
                ListMetadataSets.Add(new SelectListItem { Text = m.Name, Value = m.Guid });
            }

            //set the default metadatasEt Fields to the first metadataSet in the list
            if(metadataSets.Count() > 0)
            {
                foreach(var metadataSet in metadataSets)
                {
                    var metadataFields = new List<SelectListItem>();

                    foreach (FormField f in metadataSet.Fields)
                    {
                        metadataFields.Add(new SelectListItem { Text = f.Name, Value = f.Guid });
                    }

                    if (MetadataFields.Keys.Contains(metadataSet.Guid))
                    {
                        MetadataFields[metadataSet.Guid] = metadataFields;
                    }
                    else
                    {
                        MetadataFields.Add(metadataSet.Guid, metadataFields);
                    }
                }
            }

            base.InitManager(model);
        }



        public override object GetContent(object model)
        {
            CatfishDbContext db = new CatfishDbContext();
            MetadataService metadataSrv = new MetadataService(db);

            if (!String.IsNullOrEmpty(Category))
            {
                var metadataSet = metadataSrv.GetMetadataSet(SelectedMetadataSetCat);
                if(metadataSet != null)
                {
                    IsCategoryOptionsField = metadataSet.Fields.Where(f => f.Guid == Category && typeof(OptionsField).IsAssignableFrom(f.GetType())).Any();
                }
            }
            return base.GetContent(model);
        }
    }
}