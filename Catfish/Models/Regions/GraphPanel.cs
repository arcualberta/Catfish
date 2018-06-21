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
        [Display(Name = "Min X Parameter")]
        public string MinX_Parameter { get; set; }
        [Display(Name = "Max X Parameter")]
        public string MaxX_Parameter { get; set; }
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
        public List<SelectListItem> MetadataFields { get; set; }
        [ScriptIgnore]
        public List<SelectListItem> GraphTypes { get; set; }

        public string SelectedMetadataSetX { get; set; }
        public string SelectedMetadataSetY { get; set; }

        public string SelectedMetadataSetCat { get; set; }



        public GraphPanel()
        {
            ListMetadataSets = new List<SelectListItem>();
            MetadataFields = new List<SelectListItem>();
            GraphTypes = new List<SelectListItem>();
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
                if(metadataSets.ToArray()[0].Fields.Count > 0)
                {
                    foreach (FormField f in metadataSets.ToArray()[0].Fields)
                    {
                        MetadataFields.Add(new SelectListItem { Text = f.Name, Value = f.Guid });
                    }
                }
            }

            base.InitManager(model);
        }



        public override object GetContent(object model)
        {
            CatfishDbContext db = new CatfishDbContext();

            string xQuerySelect = "SELECT a.Year as YValue, SUM(a.Amount) AS XValue, COUNT(*) as 'Count'" +
                                   " FROM(" +
                                   " SELECT TOP 200 Content.value('(/item/metadata/metadata-set[@guid=\"" +SelectedMetadataSetX + "\"]/fields/field[@guid=\"" +XaxisField + "\"]/value/text)[1]', 'INT') AS Year ," +
                                    " Content.value('(/item/metadata/metadata-set[@guid=\"" + SelectedMetadataSetY+ "\"]/fields/field[@guid=\"" +YaxisField + "\"]/value/text)[1]', 'DECIMAL') AS Amount" +
                                    " FROM[dbo].[CFXmlModels]" +
                                    " WHERE Discriminator = 'CFItem' AND Content.exist('/item/metadata/metadata-set[@guid=\"" +SelectedMetadataSetX + "\"]') = 1" +
                                    " ) as a" +
                                     " GROUP BY a.Year" +
                                     " ORDER BY a.Year";
            var result = db.Database.SqlQuery<GraphQueryObject>(xQuerySelect, new object[] { SelectedMetadataSetX, XaxisField, SelectedMetadataSetY, YaxisField, SelectedMetadataSetX });

            foreach(var r in result)
            {
                Console.WriteLine("{0}, {1}, {2}", r.YValue, r.XValue, r.Count);
            }

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

            return base.GetContent(model);
        }
    }
}