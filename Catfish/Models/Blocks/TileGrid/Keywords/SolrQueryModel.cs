using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid.Keywords
{
    public class SolrQueryModel
    {
        public eAggregation Aggregation { get; set; }
        public string JoinQueryParts(IEnumerable<string> queryParts)
        {
            queryParts = queryParts.Where(qp => !string.IsNullOrEmpty(qp));
            switch (queryParts.Count())
            {
                case 0:
                    return null;
                case 1:
                    return queryParts.First();
                default:
                    string combineOperator = Aggregation == eAggregation.Intersection ? " AND " : " OR ";
                    string query = string.Join(combineOperator, queryParts);

                    return string.Format("({0})", query);
            }
        }
    }
}
