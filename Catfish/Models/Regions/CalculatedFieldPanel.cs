using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piranha.Extend;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Core.Models.Forms;
using Catfish.Services;
using Newtonsoft.Json;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "CalculatedFieldPanel")]
    [ExportMetadata("Name", "CalculatedFieldPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class CalculatedFieldPanel : CatfishRegion
    {
       
        
        [ScriptIgnore]
        public List<SelectListItem> FunctionModes { get; set; }

        public ItemQueryService.eFunctionMode SelectedFunction { get; set; }

        [ScriptIgnore]
        public List<SelectListItem> ListMetadataSets { get; set; }

        [ScriptIgnore]
        public IDictionary<string, List<SelectListItem>> MetadataFields { get; set; }

        public string Prefix { get; set; }

        public int DecimalPlaces { get; set; }

        public string SelectedFieldMetadataSet { get; set; }
        public string SelectedField { get; set; }
       
        public string Title { get; set; }

        public CalculatedFieldPanel()
        {
            ListMetadataSets = new List<SelectListItem>();
            MetadataFields = new Dictionary<string, List<SelectListItem>>();
            FunctionModes = new List<SelectListItem>();
            DecimalPlaces = 0;
            foreach (string am in Enum.GetNames(typeof(ItemQueryService.eFunctionMode)))
            {
                FunctionModes.Add(new SelectListItem { Text = am, Value = am });
            }
        }
        public override void InitManager(object model)
        {
            // get db context
            MetadataService metadataService = new MetadataService(db);
            IQueryable<CFMetadataSet> metadataSets = metadataService.GetMetadataSets();
            foreach (CFMetadataSet m in metadataSets)
            {
                ListMetadataSets.Add(new SelectListItem { Text = m.Name, Value = m.Guid });
            }

            //set the default metadatasEt Fields to the first metadataSet in the list
            if (metadataSets.Count() > 0)
            {
                foreach (var metadataSet in metadataSets)
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
    }
}