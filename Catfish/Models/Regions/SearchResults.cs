using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Helpers;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "SearchResultsExtension")]
    [ExportMetadata("Name", "SearchResults")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class SearchResults : CatfishRegion
    {
        public const string PAGE_PARAM = "p";
        public const string PERPAGE_PARAM = "pp";

        [NotMapped]
        public IEnumerable<Entity> Results { get; set; }

        [NotMapped]
        public int Page { get; set; }

        [NotMapped]
        public int TotalPerPage { get; set; }

        public int ParseInt(string input, int defaultVal, int min, int max = int.MaxValue)
        {
            if (string.IsNullOrEmpty(input))
            {
                return defaultVal;
            }

            try{
                int result = int.Parse(input);
                return result < min ? min : ((result > max) ? max : result);
            }catch(Exception ex)
            {
                return defaultVal;
            }
        }

        public override object GetContent(object model)
        {
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                string query = context.Request.QueryString[SimpleSearch.QUERY_PARAM];
                Page = ParseInt(context.Request.QueryString[SearchResults.PAGE_PARAM], 1, 1);
                TotalPerPage = ParseInt(context.Request.QueryString[SearchResults.PERPAGE_PARAM], 10, 1);

                if (!string.IsNullOrEmpty(query))
                {
                    query = string.Format("*{0}*", SolrService.EscapeQueryString(query.Trim()));
                    CatfishDbContext db = new CatfishDbContext();
                    EntityService es = new EntityService(db);

                    Results = es.GetEntitiesTextSearch(query, new string[] { ViewHelper.GetActiveLanguage().TwoLetterISOLanguageName });
                }
            }

            return base.GetContent(model);
        }
    }
}