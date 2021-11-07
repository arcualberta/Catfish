using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Models.KeywordSearch
{
    public class SearchOutput
    {
        public List<ResultItem> Items { get; set; } = new List<ResultItem>();
        public int First { get; set; }
        public int Last { get; set; }
        public int Count { get; set; }
    }
}
