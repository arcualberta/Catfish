using Piranha;
using Piranha.Entities;
using Piranha.Extend;
using Piranha.Models.Manager.PageModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "SimpleSearchExtension")]
    [ExportMetadata("Name", "SimpleSearch")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class SimpleSearch : CatfishRegion
    {
        public const string QUERY_PARAM = "query";

        [Display(Name = "Result page")]
        public string ResultPage { get; set; }

        [Display(Name = "Search on all language codes? (Defaults to the currently selected language code.)")]
        public bool SearchAllLanguage { get; set; }

        [NotMapped]
        public SelectList Pages { get; set; }

        [NotMapped]
        public string PageLink { get; set; }

        public override void InitManager(object model)
        {
            var internalId = Config.SiteTree;
            var listModel = ListModel.Get(internalId);

            Pages = new SelectList(listModel.Pages, "Id", "Title", ResultPage);

            base.InitManager(model);
        }

        public override object GetContent(object model)
        {
            if (ResultPage != null)
            {
                var page = Piranha.Models.Page.GetSingle(new Guid(ResultPage));
                PageLink = page.Permalink;
            }
                
            return base.GetContent(model);
        }
    }
}