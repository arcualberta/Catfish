using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid.Keywords
{
    public class KeywordField : SolrQueryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; } = new List<string>();
        public List<bool> Selected { get; set; } = new List<bool>();

        public KeywordField() { Aggregation = eAggregation.Intersection; }

        public KeywordField(OptionsField field, eAggregation fieldValueAggregation)
        {
            Id = field.Id;
            Name = field.Name.GetConcatenatedContent(" / ");
            Aggregation = fieldValueAggregation;
            AddKeywords(field);
        }

        public void AddKeywords(OptionsField field)
        {
            foreach(var opt in field.Options)
            {
                Values.Add(opt.OptionText.GetConcatenatedContent(" / "));
                Selected.Add(false);
            }
        }
        public string BuildSolrQuery(string fieldNamePrefix, Guid containerId)
        {
            string solrFieldName = string.Format("{0}_{1}_{2}_ts", fieldNamePrefix, containerId, Id);
            List<string> queryParts = new List<string>();
            for(int i=0; i<Selected.Count; ++i)
            {
                if (Selected[i])
                    queryParts.Add(string.Format("{0}:\"{1}\"", solrFieldName, Values[i]));
            }

            string query = JoinQueryParts(queryParts);
            return query;
        }
    }
}
