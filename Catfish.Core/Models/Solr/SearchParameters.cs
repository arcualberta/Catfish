using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SearchParameters
    {
        public const int DefaultPageSize = 10;

        public SearchParameters()
        {
            Facets = new Dictionary<string, string>();
            PageSize = DefaultPageSize;
            PageIndex = 1;
        }

        public string FreeSearch { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IDictionary<string, string> Facets { get; set; }
        public string Sort { get; set; }

        public int FirstItemIndex
        {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
        }

        public int LastItemIndex
        {
            get
            {
                return FirstItemIndex + PageSize;
            }
        }
    }
}
