using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid.Keywords
{
    public class KeywordField
    {
        public eAggregation Aggregation { get; set; } = eAggregation.Intersection;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; } = new List<string>();

        public KeywordField() { }

        public KeywordField(OptionsField field, eAggregation fieldValueAggregation)
        {
            Aggregation = fieldValueAggregation;
            AddKeywords(field);
        }

        public void AddKeywords(OptionsField field)
        {
            foreach(var opt in field.Options)
            {
                Values.Add(opt.OptionText.GetConcatenatedContent(" / "));
            }
        }
    }
}
