using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SearchResult
    {
        public List<ResultEntry> Entries { get; set; }

        public int Offset { get; set; }

        /// <summary>
        /// The total number of matches found, which may be larger than the page size.
        /// </summary>
        public int TotalMatches { get; set; }

    }
}
