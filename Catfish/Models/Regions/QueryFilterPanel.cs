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

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "QueryFilterPanel")]
    [ExportMetadata("Name", "QueryFilterPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class QueryFilterPanel :CatfishRegion
    {
       
        [Display(Name ="Min Filter")]
        public string MinLabel { get; set; }

        [Display(Name ="Max Label")]
        public string MaxLabel { get; set; }

        public QueryFilterPanel()
        {
            MinLabel = "Min Year";
            MaxLabel = "Max year";
        }
    }
}